using System;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUserService userService, IMatchService matchService, IBigBallzService bigBallzService) : base(userService, matchService, bigBallzService)
        {
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (requestContext.HttpContext.User.Identity is WindowsIdentity)
            {
                throw new InvalidOperationException("Windows authentication is not supported.");
            }
            
            base.Initialize(requestContext);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}