using System;
using System.Web.Mvc;
using BigBallz.Helpers;
using BigBallz.Models;
using BigBallz.Services;
using BigBallz.Services.L2S;

namespace BigBallz.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IAccountService _accountService;
        private readonly IMailService _mailService;


        public UserController(IUserService userService, IRoleService roleService, IMailService mailService, IAccountService accountService)
        {
            _userService = userService;
            _roleService = roleService;
            _accountService = accountService;
        }

        public UserController()
        {
            _userService = new UserService();
            _roleService = new RoleService();
            _mailService = new MailService();
            _accountService = new AccountService();
        }

        public ActionResult Index()
        {
            var userList = _userService.GetAll();

            ViewData["TotalUsuarios"] = _accountService.GetTotalPlayers();

            return View(userList);
        }

        public ActionResult Edit(int id)
       {
            User user = _userService.Get(id);


            return View(user);
        }

       [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(User user)
        {
            try
            {
                var currentUser = _userService.Get(User.Identity.Name);
              
                var roleProvider = new BBRoleProvider();

                //Verifica se usuario tem papel de admin
                if (!roleProvider.IsUserInRole(currentUser.UserName, "Admin"))
                {
                    throw new Exception("Usuário não tem permissão para editar outros usuários.");
                }

                var dbUser = _userService.Get(user.UserId);

                dbUser.Authorized = user.Authorized;

                if (dbUser.Authorized)
                {
                    var rolePlayer = _roleService.Get(2); // player
                    var userRolePlayer = new UserRole {UserId = user.UserId, RoleId = rolePlayer.RoleId};
                    dbUser.UserRoles.Add(userRolePlayer);
                }
                else
                {
                    dbUser.UserRoles.Clear();
                }

                TryUpdateModel(dbUser, "user");
                
                _userService.Save();

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                this.FlashError(ex.Message);

                return RedirectToAction("Index");
            }
        }

        public ActionResult SendNotification()
        {
            foreach (var player in _accountService.GetAllUnAuthorizedUsers())
            {
                var paymentUrl = Url.SiteRoot() + Url.Action("payment", "auth");
                var activationUrl = Url.SiteRoot() + Url.Action("activate", "auth", new { id = CryptHelper.EncryptAES256(player.UserName) });
                _mailService.SendRegistration(player, paymentUrl, activationUrl);
            }

            return RedirectToAction("index");
        }
    }
}
