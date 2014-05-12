using System;
using Elmah;
using System.Web;

namespace BigBallz.Core.Log
{
    public class ElmahLogger : ILogger
    {

        public void Info(string message)
        {
            Log(message);
        }

        public void Warn(string message)
        {
            Log(message);
        }

        public void Debug(string message)
        {
            Log(message);
        }

        public void Error(string message)
        {
            Log(message);
        }

        public void Error(Exception x)
        {
            Log(x);
        }

        public void Fatal(string message)
        {
            Log(message);
        }

        public void Fatal(Exception x)
        {
            Log(x);
        }

        private static void Log(string message)
        {
            Log(new Elmah.ApplicationException(message));
        }

        private static void Log(Exception e)
        {
            if (RaiseErrorSignal(e) || IsFiltered(e)) return;
            LogException(e);
        }

        private static bool RaiseErrorSignal(Exception e)
        {
            var context = HttpContext.Current;
            if (context == null)
                return false;
            var signal = ErrorSignal.FromContext(context);
            if (signal == null)
                return false;
            signal.Raise(e, context);
            return true;
        }

        private static bool IsFiltered(Exception e)
        {
            var context = HttpContext.Current;
            var config = context.NullSafe(x => x.GetSection("elmah/errorFilter") as ErrorFilterConfiguration);

            if (config == null)
                return false;

            var testContext = new ErrorFilterModule.AssertionHelperContext(e, context);

            return config.Assertion.Test(testContext);
        }

        private static void LogException(Exception e)
        {
            var context = HttpContext.Current;
            ErrorLog.GetDefault(context).Log(new Error(e, context));
        }
    }
}