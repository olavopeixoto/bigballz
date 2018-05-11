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

        public static decimal PagSeguroPercentageFee
        {
            get { return 0.0399M; }
        }

        public static decimal PagSeguroFixedValueFee
        {
            get { return 0.4M; }
        }

        public static decimal Revenue(bool pagSeguro)
        {
            return pagSeguro ? Price - Math.Round(Price * PagSeguroPercentageFee + PagSeguroFixedValueFee, 2) : Price;
        }

        public static Version GetVersion(this HttpContext context)
        {
            Check.Argument.IsNotNull(context, "context");

            return Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}