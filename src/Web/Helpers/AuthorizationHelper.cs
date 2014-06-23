using System;
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

        public static bool IsAuthorized(this Controller controller)
        {
            if (controller.ControllerContext.HttpContext.User == null) return false;
            return controller.ControllerContext.HttpContext.User.IsInRole(BBRoles.Player);
        }

        public static DateTime FromUnixTime(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static long ToUnixTime(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
    }
}