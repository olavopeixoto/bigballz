using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace BigBallz.Core
{
    public static class ToolsExtension
    {
        [DebuggerStepThrough]
        public static T NullSafe<TClass, T>(this TClass input, Expression<Func<TClass, T>> action, T alternativeValue) where TClass : class
        {
            return input != null ? action.Compile().Invoke(input) : alternativeValue;
        }

        [DebuggerStepThrough]
        public static T NullSafe<TClass, T>(this TClass input, Expression<Func<TClass, T>> action) where TClass : class
        {
            return input != null ? action.Compile().Invoke(input) : default(T);
        }

        [DebuggerStepThrough]
        public static T NullAlternative<T>(this T target, T alternativeValue) where T : class
        {
            return (target ?? alternativeValue);
        }

        public static T Conditional<T>(this T value, bool condition)
        {
            return condition ? value : default(T);
        }

        public static T Conditional<T>(this T value, bool condition, T alternateValue)
        {
            return condition ? value : alternateValue;
        }
    }
}
