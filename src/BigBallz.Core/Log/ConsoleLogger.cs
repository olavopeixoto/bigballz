using System;
using System.Diagnostics;

namespace BigBallz.Core.Log
{
	[DebuggerStepThrough]
	public class ConsoleLogger:ILogger {
        public void Info(string message) {
            System.Diagnostics.Debug.WriteLine(message);
        }

        public void Warn(string message) {
            System.Diagnostics.Debug.WriteLine(message);
        }

        public void Debug(string message) {
            System.Diagnostics.Debug.WriteLine(message);
        }

        public void Error(string message) {
            System.Diagnostics.Debug.WriteLine(message);
        }
        public void Error(Exception x) {
            Error(LogUtility.BuildExceptionMessage(x));
        }
        public void Fatal(string message) {
            System.Diagnostics.Debug.WriteLine(message);
        }
        public void Fatal(Exception x) {
            Fatal(LogUtility.BuildExceptionMessage(x));
        }
    }
}