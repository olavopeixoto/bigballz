using System;
using System.ComponentModel;

namespace BigBallz.Core
{
    public static class EnumExtensions
    {
        public static string Description(this Enum value)
        {
            var da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false));
            return da.Length > 0 ? da[0].Description : value.ToString();
        }

        public static int Value(this Enum value)
        {
            return Convert.ToInt32(value);
        }
    }
}