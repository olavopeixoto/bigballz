using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BigBallz.Core.Extension.Web.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string SiteRoot(this UrlHelper url)
        {
            var root = HttpContext.Current.Request.ApplicationPath;
            return root.EndsWith("/") ? root.Substring(0, root.Length - 1) : root;
        }

        // Site

        public static string UrlLogin(this UrlHelper url)
        {
            return url.SiteRoot();
        }

        public static string UrlLogout(this UrlHelper url)
        {
            return url.SiteRoot() + "/Default.aspx?fi=1&LogOut=1";
        }

        // Helpers

        public static string Estrutura(this UrlHelper url, int funcionalidade, string queryString)
        {
            var uri = string.Format("{0}/Estrutura/Estrutura.aspx?CodFun={1}", url.SiteRoot(), funcionalidade);
            if (!string.IsNullOrEmpty(queryString))
            {
                if (!queryString.StartsWith("&"))
                {
                    uri += "&";
                }

                uri += queryString;
            }
            return uri;
        }

        public static string Estrutura(this UrlHelper url, int funcionalidade, object values)
        {
            var uri = string.Format("{0}/Estrutura/Estrutura.aspx?CodFun={1}", url.SiteRoot(), funcionalidade);
            var valuesDictionary = new RouteValueDictionary(values);
            var sb = new StringBuilder(uri);
            foreach (var pair in valuesDictionary)
            {
                sb.AppendFormat("&{0}={1}", Uri.EscapeDataString(pair.Key), Uri.EscapeDataString(Convert.ToString(pair.Value)));
            }
            return sb.ToString();
        }

        public static string Estrutura(this UrlHelper url, int funcionalidade)
        {
            return url.Estrutura(funcionalidade, null);
        }

        public static string CurrentAction(this UrlHelper url)
        {
            return url.RouteCollection.GetRouteData(url.RequestContext.HttpContext).GetRequiredString("action");
        }

        public static string ActionForDataFW(this UrlHelper url)
        {
            var routeValueDictionary = url.RouteCollection.GetRouteData(url.RequestContext.HttpContext).Values;
            routeValueDictionary.Remove("year");
            routeValueDictionary.Remove("month");
            return url.Action(CurrentAction(url), routeValueDictionary);
        }
    }
}