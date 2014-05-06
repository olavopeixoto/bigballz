using System;
using System.Configuration;

namespace BigBallz.Helpers
{
    public class ConfigurationHelper
    {
        public static decimal Price
        {
            get { return Convert.ToDecimal(ConfigurationManager.AppSettings["Price"]); }
        }
    }
}