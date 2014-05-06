using System.Diagnostics;
using BigBallz.Core.Helper;
using BigBallz.Core.IoC;

namespace BigBallz.Core.Log
{
    using System;
    using System.Runtime.CompilerServices;

    public static class Logger
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Info(string message)
        {
            Check.Argument.IsNotEmpty(message, "message");

            GetLog().Info(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Info(string format, params object[] args)
        {
            Check.Argument.IsNotEmpty(format, "format");

            GetLog().Info(Format(format, args));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Warn(string message)
        {
            Check.Argument.IsNotEmpty(message, "message");

            GetLog().Warn(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Warn(string format, params object[] args)
        {
            Check.Argument.IsNotEmpty(format, "format");

            GetLog().Warn(Format(format, args));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Error(string message)
        {
            Check.Argument.IsNotEmpty(message, "message");

            GetLog().Error(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Error(string format, params object[] args)
        {
            Check.Argument.IsNotEmpty(format, "format");

            GetLog().Error(Format(format, args));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Error(Exception exception)
        {
            Check.Argument.IsNotNull(exception, "exception");

            GetLog().Error(exception);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Fatal(string message)
        {
            Check.Argument.IsNotEmpty(message, "message");

            GetLog().Fatal(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Fatal(string format, params object[] args)
        {
            Check.Argument.IsNotEmpty(format, "format");

            GetLog().Fatal(Format(format, args));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [DebuggerStepThrough]
        public static void Fatal(Exception exception)
        {
            Check.Argument.IsNotNull(exception, "exception");

            GetLog().Fatal(exception);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ILogger GetLog()
        {
            return ServiceLocator.Resolve<ILogger>();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string Format(string format, params object[] args)
        {
            Check.Argument.IsNotEmpty(format, "format");

            return format.FormatWith(args);
        }
    }
}