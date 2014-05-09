using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using BigBallz.Core.Helper;

namespace BigBallz.Helpers
{
    public static class ConfigurationHelper
    {
        public static decimal Price
        {
            get { return Convert.ToDecimal(ConfigurationManager.AppSettings["Price"]); }
        }

        public static Version GetVersion(this HttpContext context)
        {
            Check.Argument.IsNotNull(context, "context");

            return Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}