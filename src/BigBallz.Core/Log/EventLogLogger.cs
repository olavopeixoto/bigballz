﻿using System;
using System.Diagnostics;

namespace BigBallz.Core.Log
{
    public class EventLogLogger:ILogger {
        
        #region ILogger Members

        public void Info(string message) {
            WriteToEventLog(message, EventLogEntryType.Information);
        }

        public void Warn(string message) {
            WriteToEventLog(message, EventLogEntryType.Warning);
        }

        public void Debug(string message) {
            //no need to send this to the EventLog - output it 
            //to the DEBUG window
            System.Diagnostics.Debug.WriteLine(message);
        }

        public void Error(string message) {
            WriteToEventLog(message, EventLogEntryType.Error);
        }

        public void Error(Exception x) {
            Error(LogUtility.BuildExceptionMessage(x));
        }

        public void Fatal(string message) {
            WriteToEventLog(message, EventLogEntryType.Error);
        }
        public void Fatal(Exception x) {
            WriteToEventLog(LogUtility.BuildExceptionMessage(x), EventLogEntryType.Error);
        }


        void WriteToEventLog(string message, EventLogEntryType logType) {

            EventLog eventLog = new EventLog();

            eventLog.Source = "BigBallz";
            
            eventLog.WriteEntry(message, logType);
        }
        #endregion
    }
}
