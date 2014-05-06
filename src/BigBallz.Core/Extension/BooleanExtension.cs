using System.Diagnostics;

namespace BigBallz.Core
{
    public static class BooleanExtension
    {
        [DebuggerStepThrough]
        public static T Substitute<T>(this bool? condition, T defaultValue, T trueValue, T falseValue)
        {
            return !condition.HasValue ? defaultValue : condition.Value ? trueValue : falseValue;
        }

        [DebuggerStepThrough]
        public static T Substitute<T>(this bool condition, T trueValue, T falseValue)
        {
            return condition ? trueValue : falseValue;
        }

        [DebuggerStepThrough]
        public static string ListItem(this bool? condition, string trueValue, string falseValue)
        {
            return condition.HasValue ? ListItem(condition.Value, trueValue, falseValue) : string.Empty;
        }

        [DebuggerStepThrough]
        public static string ListItem(this bool condition, string trueValue, string falseValue)
        {
            return "<li>" + (condition ? trueValue : falseValue) + "</li>";
        }
    }
}