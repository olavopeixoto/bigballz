using System;
using System.Diagnostics;

namespace BigBallz.Core
{
    public static class DateTimeExtension
    {
        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        [DebuggerStepThrough]
        public static bool IsValid(this DateTime target)
        {
            return (target >= MinDate) && (target <= MaxDate);
        }

        public static string FormatDate(this DateTime date)
        {
            return String.Format("{0:d2}/{1:d2}/{2:d4}", date.Day, date.Month, date.Year);
        }

        public static string FormatDate(this DateTime? date)
        {
            return date.HasValue ? FormatDate(date.Value) : String.Empty;
        }

        public static string FormatDateTime(this DateTime? date)
        {
            return date.HasValue ? FormatDateTime(date.Value) : String.Empty;
        }

        public static string FormatDateTime(this DateTime date)
        {
            return String.Format("{0:d2}/{1:d2}/{2:d4} {3:d2}:{4:d2}:{5:d2}", date.Day, date.Month, date.Year, date.Hour, date.Minute, date.Second);
        }

        public static string RenderDateMonthYear(this DateTime date)
        {
            return String.Format("{1:d2}/{0:d4}", date.Year, date.Month);
        }

        public static DateTime ToFirstDayOfMonth(this DateTime date)
        {
            return date.AddDays(-date.Day + 1);
        }

        public static string ToString(this DateTime? date, string format)
        {
            return date.HasValue ? date.Value.ToString(format) : string.Empty;
        }

        public static DateTime BrazilTimeZone(this DateTime date)
        {
            return date.ToUniversalTime().Subtract(new TimeSpan(0, 3, 0, 0));
        }
    }
}