using System.Web.Mvc;
using BigBallz.Models;

namespace BigBallz.Helpers
{
    public static class AuthorizationHelper
    {
        public static bool IsAdmin(this HtmlHelper htmlHelper)
        {
            if (htmlHelper.ViewContext.HttpContext.User == null) return false;
            return htmlHelper.ViewContext.HttpContext.User.IsInRole(BBRoles.Admin);
        }

        public static bool IsAuthorized(this HtmlHelper htmlHelper)
        {
            if (htmlHelper.ViewContext.HttpContext.User == null) return false;
            return htmlHelper.ViewContext.HttpContext.User.IsInRole(BBRoles.Player);
        }

        public static bool IsAdmin(this Controller controller)
        {
            if (controller.ControllerContext.HttpContext.User == null) return false;
            return controller.ControllerContext.HttpContext.User.IsInRole(BBRoles.Admin);
        }

        public static bool IsAuthorized(this Controller controller)
        {
            if (controller.ControllerContext.HttpContext.User == null) return false;
            return controller.ControllerContext.HttpContext.User.IsInRole(BBRoles.Player);
        }
    }
}