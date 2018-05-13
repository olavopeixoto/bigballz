using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BigBallz.Core;
using BigBallz.Core.Log;
using BigBallz.Filters;
using BigBallz.Helpers;
using BigBallz.Models;
using BigBallz.Services;
using RPXLib.Data;
using RPXLib.Interfaces;
using Uol.PagSeguro.Resources;
using Uol.PagSeguro.Service;

namespace BigBallz.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IRPXService _rpxService;
        private readonly IAccountService _accountService;
        private readonly IMailService _mailService;
        private readonly IMatchService _matchService;

        public AuthController(IRPXService rpxService, IAccountService accountService, IMailService mailService,
            IMatchService matchService, IUserService userService, IBigBallzService bigBallzService)
            : base(userService, matchService, bigBallzService)
        {
            _rpxService = rpxService;
            _accountService = accountService;
            _mailService = mailService;
            _matchService = matchService;
        }

        [HttpPost, AllowAnonymous]
        public ActionResult HandleResponse(string token, string returnUrl)
        {
            //according to the spec, it is possible to get here without a token
            //which means that the user cancelled the login request
            if (string.IsNullOrEmpty(token))
                return string.IsNullOrEmpty(returnUrl)
                    ? (ActionResult) RedirectToAction("Index", "Home")
                    : Redirect(returnUrl);

            RPXAuthenticationDetails authenticationDetails;

            try
            {
                //This service call will throw an exception if it fails. 
                //You may want to catch it explicitly.
                authenticationDetails = _rpxService.GetAuthenticationDetails(token, true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.FlashError("Ocorreu um erro na autenticação");
                return RedirectToAction("index", "home");
            }

            var user = _accountService.FindUserByIdentifier(authenticationDetails.Identifier);
            if (user == null)
            {
                Session.RemoveAll();
                TempData["UserDetails"] = authenticationDetails;
                return RedirectToAction("NewAccount");
            }

            string photoUrl;
            if (string.IsNullOrEmpty(authenticationDetails.PhotoUrl))
            {
                var gravatar = new GravatarHelper
                {
                    Email = user.EmailAddress
                };
                photoUrl = gravatar.GetGravatarUrl();
            }
            else
            {
                photoUrl = authenticationDetails.PhotoUrl;
            }

            if (!string.Equals(user.PhotoUrl, photoUrl, StringComparison.InvariantCultureIgnoreCase))
            {
                user.PhotoUrl = photoUrl;
                _accountService.UpdateUserInformation(user);
            }

            SignIn(user.UserName, true);
            Response.Cookies.Add(new HttpCookie("photoUrl", user.PhotoUrl));

            try
            {
                var newUserDetails = Session["UserDetails"] as RPXAuthenticationDetails;
                if (newUserDetails != null)
                {
                    Session.Remove("UserDetails");
                    if (_accountService.FindUserByIdentifier(newUserDetails.Identifier) != null)
                    {
                        this.FlashError(
                            $"A conta do {newUserDetails.ProviderName} não foi associada. Já existe uma associação com essa conta.");
                    }
                    else
                    {
                        _accountService.AssociateExistingUser(user.UserId, newUserDetails.Identifier,
                            newUserDetails.ProviderName);
                        this.FlashInfo($"Conta do {newUserDetails.ProviderName} associada com sucesso!");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.FlashError("Ocorreu um erro na tentativa associar a nova conta a uma existente");
            }

            return string.IsNullOrEmpty(returnUrl)
                ? user.Authorized
                    ? (ActionResult) RedirectToAction("index", "bet")
                    : RedirectToAction("payment", "auth")
                : Redirect(returnUrl);
        }

        [AllowAnonymous]
        [HttpPost]
        public void Notification(string notificationCode, string notificationType)
        {
            var credentials = PagSeguroConfiguration.Credentials;

            var transaction = NotificationService.CheckTransaction(credentials, notificationCode);

            int uid;
            var itemId = transaction.Items.First().Id;
            var user = int.TryParse(itemId, NumberStyles.Integer, CultureInfo.CurrentCulture.NumberFormat,
                out uid)
                ? _accountService.FindUserByLocalId(uid)
                : _accountService.FindUserByUserName(itemId);

            if (user != null)
            {
                try
                {
                    _accountService.UpdateTransactionStatus(user, transaction);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }

                if (transaction.TransactionStatus == (int) PagSeguroTransactionStatus.Paga)
                {
                    if (_accountService.AuthorizeUser(user.UserName, "PagSeguro", true))
                    {
                        _mailService.SendPaymentConfirmation(user);
                    }
                }
            }
            else
            {
                throw new ApplicationException($"Usuário {itemId} não encontrado para autorização");
            }
        }

        [HttpGet]
        public ActionResult ConfirmacaoPagamento(string tid)
        {
            if (string.IsNullOrWhiteSpace(tid)) return RedirectToAction("index", "home");
            
            var credentials = PagSeguroConfiguration.Credentials;
            var transaction = TransactionSearchService.SearchByCode(credentials, tid);
            var item = transaction.Items.Single();

            int uid;
            if (!int.TryParse(item.Id, NumberStyles.Integer, CultureInfo.CurrentCulture.NumberFormat, out uid))
            {
                Logger.Error("Identificação de usuário inválida ({0})", item.Id);
                return RedirectToAction("index", "home");
            }

            var user = _accountService.FindUserByLocalId(uid);
            if (user == null)
            {
                Logger.Error("Usuário {0} não encontrado para autorização", item.Id);
                return RedirectToAction("index", "home");
            }
            if (user.UserName != User.Identity.Name)
            {
                Logger.Error("Usuário {0} ({1}) incorreto para autorização com o {2}", item.Id, user.UserName, User.Identity.Name);
                return RedirectToAction("index", "home");
            }

            if (transaction.TransactionStatus == (int)PagSeguroTransactionStatus.Paga)
            {
                if (_accountService.AuthorizeUser(user.UserName, "PagSeguro", true))
                {
                    _mailService.SendPaymentConfirmation(user);
                }

                return View("ConfirmacaoPagamentoPago");
            }

            return View((PagSeguroTransactionStatus)transaction.TransactionStatus);
        }

        [HttpGet]
        public RedirectToRouteResult SignOut()
        {
            SignOutCurrentUser();
            return RedirectToAction("Index", "Home");
        }  

        [HttpGet, AllowAnonymous]
        public ActionResult Join()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = _accountService.FindUserByUserName(User.Identity.Name);
                if (user != null && user.Authorized)
                {
                    return RedirectToAction("index", "home");
                }
            }

            ViewData["StartDate"] = _matchService.GetStartDate().AddDays(-1).FormatDate();
            return View();
        }

        [HttpGet, AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet]
        public ActionResult NewAccount()
        {
            var authenticationDetails = TempData.Peek("UserDetails") as RPXAuthenticationDetails;
            
            if (authenticationDetails == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Session["UserDetails"] = authenticationDetails;

            var user = new User
            {
                UserName = authenticationDetails.PreferredUsername ?? authenticationDetails.DisplayName,
                EmailAddress = authenticationDetails.VerifiedEmail ?? authenticationDetails.Email,
                PhotoUrl = authenticationDetails.PhotoUrl
            };

            return View(user);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult NewAccount(User user)
        {
            var authenticationDetails = Session["UserDetails"] as RPXAuthenticationDetails;
            if (authenticationDetails == null) return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid) return View(user);

            try
            {
                if (string.IsNullOrEmpty(authenticationDetails.PhotoUrl))
                {
                    var gravatar = new GravatarHelper
                    {
                        Email = user.EmailAddress
                    };
                    user.PhotoUrl = gravatar.GetGravatarUrl();
                }
                else
                {
                    user.PhotoUrl = authenticationDetails.PhotoUrl;
                }

                using (var tran = new TransactionScope())
                {
                    _accountService.CreateUser(authenticationDetails.Identifier, user.UserName,
                        authenticationDetails.ProviderName, user.EmailAddress,
                        authenticationDetails.VerifiedEmail == user.EmailAddress,
                        authenticationDetails.PhotoUrl);

                    var paymentUrl = Url.Action("payment", "auth", new {}, Uri.UriSchemeHttps);
                    var activationUrl = Url.Action("activate", "auth", new {id = CryptHelper.EncryptAES256(user.UserName)}, Uri.UriSchemeHttps);

                    _mailService.SendRegistration(user, paymentUrl, activationUrl);

                    tran.Complete();
                }

                SignIn(user.UserName, true);

                TempData["UserDetails"] = authenticationDetails;
                return RedirectToAction("NewAccountSuccess");
            }
            catch (SqlException ex)
            {
                switch(ex.Number)
                {
                    case 2627:
                        this.FlashError("Usuário já cadastrado no sistema");
                        return View(user);
                    default:
                        Logger.Error(ex);
                        this.FlashError(ex.Message);
                        return View(user);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.FlashError(ex.Message);
                return View(user);
            }
        }

        [HttpGet]
        public ActionResult NewAccountSuccess()
        {
            var authenticationDetails = TempData["UserDetails"] as RPXAuthenticationDetails;

            if (authenticationDetails == null) return RedirectToAction("index", "home");

            ViewData["nomeProvedor"] = authenticationDetails.ProviderName;
            return View();
        }

        [HttpGet, UserNameFilter]
        public ActionResult Payment(string userName)
        {
            var user = _accountService.FindUserByUserName(userName);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (user.Authorized)
            {
                return RedirectToAction("index", "bet");
            }
            return View(user);
        }

        [Authorize(Roles = BBRoles.Admin)]
        public ActionResult EnableProfiler()
        {
            Response.SetCookie(new HttpCookie("x-profiler", "true")
            {
                Secure = FormsAuthentication.RequireSSL,
                HttpOnly = true,
            });

            return Redirect(Convert.ToString(Request.UrlReferrer));
        }

        [Authorize(Roles = BBRoles.Admin)]
        public ActionResult DisableProfiler()
        {
            if (Request.Cookies["x-profiler"] != null)
            {
                var myCookie = new HttpCookie("x-profiler") { Expires = DateTime.Now.AddDays(-1d) };
                Response.SetCookie(myCookie);
            }

            return Redirect(Convert.ToString(Request.UrlReferrer));
        }

        [HttpGet]
        public ActionResult HelpLogin()
        {
            if (!User.Identity.IsAuthenticated) RedirectToAction("index", "home");

            var key = ConfigurationManager.AppSettings["freshdesk-ssokey"];
            var baseUrl = ConfigurationManager.AppSettings["freshdesk-baseUrl"];
            var pathTemplate = baseUrl + "/login/sso?name={0}&email={1}&timestamp={2}&hash={3}";

            var user = _accountService.FindUserByUserName(User.Identity.Name);

            var username = user.UserName;
            var email = user.EmailAddress;

            var timems = DateTime.UtcNow.ToUnixTime().ToString(CultureInfo.InvariantCulture);
            var hash = GetHash(key, username, email, timems);
            var path = string.Format(pathTemplate, Server.UrlEncode(username), Server.UrlEncode(email), timems, hash);

            return Redirect(path);
        }

        private static string GetHash(string secret, string name, string email, string timems)
        {
            var input = name + email + timems;
            var keybytes = Encoding.Default.GetBytes(secret);
            var inputBytes = Encoding.Default.GetBytes(input);

            var crypto = new HMACMD5(keybytes);
            var hash = crypto.ComputeHash(inputBytes);

            return BitConverter.ToString(hash).ToLowerInvariant().Replace("-", string.Empty);
        }

        private static void SignIn(string localId, bool rememberMe)
        {
            FormsAuthentication.SetAuthCookie(localId, rememberMe);
        }

        private void SignOutCurrentUser()
        {
            FormsAuthentication.SignOut();
            //Session.Abandon();
        }
    }
}