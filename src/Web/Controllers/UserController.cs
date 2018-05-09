using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using BigBallz.Core.Extension.Web.Mvc;
using BigBallz.Core.Log;
using BigBallz.Helpers;
using BigBallz.Models;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IAccountService _accountService;
        private readonly IMailService _mailService;

        public UserController(IUserService userService, IRoleService roleService, IAccountService accountService, IMailService mailService, IMatchService matchService, IBigBallzService bigBallzService) : base(userService, matchService, bigBallzService)
        {
            _userService = userService;
            _roleService = roleService;
            _accountService = accountService;
            _mailService = mailService;
        }

        public ActionResult Index()
        {
            var userList = _userService.GetAll();

            ViewData["TotalUsuarios"] = _accountService.GetTotalPlayers();

            return View(userList);
        }

        public ActionResult Edit(int id)
       {
            var user = _userService.Get(id);

            return View(user);
        }

       [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(User user)
        {
            try
            {
                var sendConfirmationIfSucceed = false;

                var dbUser = _userService.Get(user.UserId);

                if (user.Authorized && !dbUser.Authorized)
                {
                    var rolePlayer = _roleService.Get(2); // player
                    var userRolePlayer = new UserRole {UserId = user.UserId, RoleId = rolePlayer.RoleId};
                    dbUser.UserRoles.Add(userRolePlayer);
                    dbUser.AuthorizedBy = User.Identity.Name;
                    sendConfirmationIfSucceed = true;
                }
                else if (!user.Authorized && dbUser.Authorized)
                {
                    var userRole = dbUser.UserRoles.FirstOrDefault(x => x.Role.Name.ToLowerInvariant() == BBRoles.Player);
                    if (userRole != null) dbUser.UserRoles.Remove(userRole);
                    dbUser.AuthorizedBy = null;
                }

                if (user.IsAdmin && !dbUser.IsAdmin)
                {
                    var adminRole = _roleService.Get(1); // admin
                    var adminUserRole = new UserRole { UserId = user.UserId, RoleId = adminRole.RoleId };
                    dbUser.UserRoles.Add(adminUserRole);
                }
                else if (!user.IsAdmin && dbUser.IsAdmin)
                {
                    var userRole = dbUser.UserRoles.FirstOrDefault(x => x.Role.Name.ToLowerInvariant() == BBRoles.Admin);
                    if (userRole!=null) dbUser.UserRoles.Remove(userRole);
                }

                dbUser.Authorized = user.Authorized;
                dbUser.IsAdmin = user.IsAdmin;
                dbUser.PagSeguro = user.Authorized && user.PagSeguro;

                TryUpdateModel(dbUser, "user");
                
                _userService.Save();

                if (sendConfirmationIfSucceed) _mailService.SendPaymentConfirmation(dbUser);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
                this.FlashError(ex.Message);

                return View(user);
            }
        }

        public ActionResult SendNotification()
        {
            foreach (var player in _accountService.GetAllUnAuthorizedUsers())
            {
                var paymentUrl = Url.Action("payment", "auth", new {}, Uri.UriSchemeHttps);
                var activationUrl = Url.Action("activate", "auth", new { id = CryptHelper.EncryptAES256(player.UserName) }, Uri.UriSchemeHttps);
                _mailService.SendRegistration(player, paymentUrl, activationUrl);
            }

            return RedirectToAction("index");
        }
    }
}
