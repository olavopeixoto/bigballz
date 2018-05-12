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
            get { return Convert.ToDecimal(ConfigurationManager.AppSettings["PagSeguroPercentageFee"]); }
        }

        public static decimal PagSeguroFixedValueFee
        {
            get { return Convert.ToDecimal(ConfigurationManager.AppSettings["PagSeguroFixedValueFee"]); }
        }

        public static decimal PrizeFirstPercentage
        {
            get { return Convert.ToDecimal(ConfigurationManager.AppSettings["PrizeFirstPercentage"]); }
        }

        public static decimal PrizeSecondPercentage
        {
            get { return Convert.ToDecimal(ConfigurationManager.AppSettings["PrizeSecondPercentage"]); }
        }

        public static decimal PrizeThirdPercentage
        {
            get { return Convert.ToDecimal(ConfigurationManager.AppSettings["PrizeThirdPercentage"]); }
        }

        public static decimal Revenue(bool pagSeguro)
        {
            return pagSeguro ? Price - Math.Round(Price * PagSeguroPercentageFee + PagSeguroFixedValueFee, 2) : Price;
        }

        public static Version GetVersion(this HttpContext context)
        {
            Check.Argument.IsNotNull(context, nameof(context));

            return Assembly.GetExecutingAssembly().GetName().Version;
        }
    }
}