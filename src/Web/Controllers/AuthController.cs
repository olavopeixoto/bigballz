using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BigBallz.Core;
using BigBallz.Filters;
using BigBallz.Helpers;
using BigBallz.Models;
using BigBallz.Services;
using BigBallz.Services.L2S;
using RPXLib;
using RPXLib.Data;
using RPXLib.Interfaces;

namespace BigBallz.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IRPXService _rpxService;
        private readonly IAccountService _accountService;
        private readonly IMailService _mailService;
        private readonly IMatchService _matchService;

        public AuthController(IRPXService rpxService, IAccountService accountService, IMailService mailService, IMatchService matchService)
        {
            _rpxService = rpxService;
            _accountService = accountService;
            _mailService = mailService;
            _matchService = matchService;
        }

        public  AuthController()
        {
            const string baseUrl = "https://rpxnow.com/api/v2/";
            var apiKey = ConfigurationManager.AppSettings["rpxnow-apikey"];

            //if you need to access the service via a web proxy set the proxy details here
            const IWebProxy webProxy = null;

            var settings = new RPXApiSettings(baseUrl, apiKey, webProxy);
            _rpxService = new RPXService(settings);

            _accountService = new AccountService();
            _mailService = new MailService();
            _matchService = new MatchService();
        }

        [HttpPost]
        public ActionResult HandleResponse(string token, string returnUrl)
        {
            //according to the spec, it is possible to get here without a token
            //which means that the user cancelled the login request
            if (string.IsNullOrEmpty(token))
                return string.IsNullOrEmpty(returnUrl) ? (ActionResult) RedirectToAction("Index", "Home") : Redirect(returnUrl);

            //This service call will throw an exception if it fails. 
            //You may want to catch it explicitly.
            var authenticationDetails = _rpxService.GetAuthenticationDetails(token, true);
            TempData["id"] = authenticationDetails.Identifier;

            Session["UserDetails"] = authenticationDetails;

            var user = _accountService.FindUserByIdentifier(authenticationDetails.Identifier);
            if (user == null)
            {
                return RedirectToAction("NewAccount");
            }

            user.PhotoUrl = authenticationDetails.PhotoUrl;
            _accountService.UpdateUserInformation(user);
            SignIn(user.UserName, true);
            Response.Cookies.Add(new HttpCookie("photoUrl", user.PhotoUrl));

            return string.IsNullOrEmpty(returnUrl) ? (ActionResult)RedirectToAction("Index", "Home") : Redirect(returnUrl);
        }

        //o método POST indica que a requisição é o retorno da validação NPI.
        [HttpPost]
        public void ConfirmacaoPagamento(string prodID_1, string cliEmail, string statusTransacao, FormCollection info)
        {
            var token = ConfigurationManager.AppSettings["pagseguro-token"];
#if DEBUG
            const string pagina = "http://localhost:9090/pagseguro-ws/checkout/NPI.jhtml";
#else
            const string pagina = "https://pagseguro.uol.com.br/pagseguro-ws/checkout/NPI.jhtml";
#endif
            var dados = Request.Form + "&Comando=validar" + "&Token=" + token;

            var req = (HttpWebRequest) WebRequest.Create(pagina);

            req.Method = "POST";
            req.ContentLength = dados.Length;
            req.ContentType = "application/x-www-form-urlencoded";

            using (var stOut = new System.IO.StreamWriter(req.GetRequestStream(),
                                                   System.Text.Encoding.GetEncoding("ISO-8859-1")))
            {
                stOut.Write(dados);
            }

            using (var stIn = new System.IO.StreamReader(req.GetResponse().GetResponseStream(),
                                                  System.Text.Encoding.GetEncoding("ISO-8859-1")))
            {
                var result = stIn.ReadToEnd();

                if (result.ToLowerInvariant() == "verificado" && (statusTransacao.ToLowerInvariant() == "aprovado"))
                {
                    //o post foi validado
                    var user = _accountService.FindUserByUserName(prodID_1);
                    if (user != null)
                    {
                        _accountService.AuthorizeUser(user.UserName);
                        _mailService.SendPaymentConfirmation(user);
                    }
                }
            }
        }

        [HttpGet, Authorize]
        public ActionResult ConfirmacaoPagamento()
        {
            return View();
        }

        [HttpGet]
        public RedirectToRouteResult SignOut()
        {
            SignOutCurrentUser();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Join()
        {
            ViewData["StartDate"] = _matchService.GetStartDate().AddDays(-1).FormatDate();
            return View();
        }

        [HttpGet]
        public ActionResult NewAccount()
        {
            var authenticationDetails = Session["UserDetails"] as RPXAuthenticationDetails;
            if (authenticationDetails == null)
            {
                SignOutCurrentUser();
                return RedirectToAction("Index", "Home");
            }
            var user = new User
                           {
                               UserName = authenticationDetails.PreferredUsername ?? authenticationDetails.DisplayName,
                               EmailAddress = authenticationDetails.VerifiedEmail ?? authenticationDetails.Email,
                               PhotoUrl = authenticationDetails.PhotoUrl
                           };

            return View(user);
        }

        [HttpGet]
        public ActionResult Activate(string id)
        {
            var userName = CryptHelper.DecryptAES256(id);

            if (!_accountService.VerifyEmail(userName))
            {
                this.FlashError("Chave de ativação inválida");
                return RedirectToAction("index", "home");
            }

            this.FlashInfo("E-Mail verificado com sucesso!");
            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public ActionResult NewAccount(User user)
        {
            var authenticationDetails = Session["UserDetails"] as RPXAuthenticationDetails;
            if (authenticationDetails == null) return RedirectToAction("Index", "Home");
            if (string.IsNullOrEmpty(authenticationDetails.PhotoUrl))
            {
                var gravatar = new GravatarHelper
                {
                    email = user.EmailAddress
                };
                user.PhotoUrl = gravatar.GetGravatarUrl();
            }
            else
            {
                user.PhotoUrl = authenticationDetails.PhotoUrl;
            }
            if (!ModelState.IsValid) return View(user);

            try
            {
                _accountService.CreateUser(authenticationDetails.Identifier, user.UserName,
                                           authenticationDetails.ProviderName, user.EmailAddress,
                                           authenticationDetails.VerifiedEmail == user.EmailAddress,
                                           authenticationDetails.PhotoUrl);

                SignIn(user.UserName, true);

                var paymentUrl = Url.SiteRoot() + Url.Action("payment", "auth");
                var activationUrl = Url.SiteRoot() + Url.Action("activate", "auth", new { id = CryptHelper.EncryptAES256(user.UserName) });
                
                _mailService.SendRegistration(user, paymentUrl, activationUrl);

                return RedirectToAction("NewAccountSuccess", new {pn = authenticationDetails.ProviderName});
            }
            catch (SqlException ex)
            {
                switch(ex.Number)
                {
                    case 2627:
                        this.FlashError("Usuário já cadastrado no sistema");
                        return View(user);
                    default:
                        this.FlashError(ex.Message);
                        return View(user);
                }
            }
            catch (Exception ex)
            {
                this.FlashError(ex.Message);
                return View(user);
            }
        }

        [HttpGet]
        public ActionResult NewAccountSuccess(string pn)
        {
            ViewData["nomeProvedor"] = pn;
            return View();
        }

        [HttpGet, UserNameFilter]
        public ActionResult Payment(string userName)
        {
            var user = _accountService.FindUserByUserName(userName);
            if (user == null || user.Authorized)
            {
                SignOutCurrentUser();
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        private static void SignIn(string localId, bool rememberMe)
        {
            FormsAuthentication.SetAuthCookie(localId, rememberMe);
        }

        private void SignOutCurrentUser()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
        }
    }
}