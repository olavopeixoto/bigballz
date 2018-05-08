using System.Web;
using System.Web.Mvc;

namespace BigBallz.Core.Extension.Web.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string SiteRoot(this UrlHelper url)
        {
            var root = HttpContext.Current.Request.ApplicationPath;
            return root.EndsWith("/") ? root.Substring(0, root.Length - 1) : root;
        }

        // Helpers

        public static string CurrentAction(this UrlHelper url)
        {
            return url.RouteCollection.GetRouteData(url.RequestContext.HttpContext).GetRequiredString("action");
        }
    }
}