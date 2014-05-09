using System;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace BigBallz.Controllers
{
    public class AccountController : BaseController
    {
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