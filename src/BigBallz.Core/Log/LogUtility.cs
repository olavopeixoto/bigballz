﻿using System;

namespace BigBallz.Core.Log
{
    public static class LogUtility {

        public static string BuildExceptionMessage(Exception x) {
			
			var logException=x;
			if(x.InnerException!=null)
				logException=x.InnerException;

			var strErrorMsg= Environment.NewLine+"Error in Path :" + System.Web.HttpContext.Current.Request.Path;

			// Get the QueryString along with the Virtual Path
            strErrorMsg += Environment.NewLine + "Raw Url :" + System.Web.HttpContext.Current.Request.RawUrl;
			
			// Get the error message
            strErrorMsg += Environment.NewLine + "Message :" + logException.Message;

			// Source of the message
            strErrorMsg += Environment.NewLine + "Source :" + logException.Source;

			// Stack Trace of the error
            strErrorMsg += Environment.NewLine + "Stack Trace :" + logException.StackTrace;

			// Method where the error occurred
            strErrorMsg += Environment.NewLine + "TargetSite :" + logException.TargetSite;

			return strErrorMsg;
        }
    }
}
