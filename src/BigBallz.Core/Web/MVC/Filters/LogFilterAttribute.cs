using System;
using System.Web.Mvc;
using BigBallz.Core.IoC;
using BigBallz.Core.Log;
using System.Diagnostics;

namespace BigBallz.Core.Web.MVC.Filters
{
	[DebuggerStepThrough]
	public class LogFilterAttribute : ActionFilterAttribute, IExceptionFilter
    {
        private static ILogger Log
        {
            get
            {
                return ServiceLocator.Resolve<ILogger>();
            }
        }

        //private DateTime startActionExecution;
        //private DateTime startResultExecution;

        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    startActionExecution = DateTime.Now;
        //    Log.Debug("OnActionExecuting");
        //    base.OnActionExecuting(filterContext);
        //}

        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    Log.Debug(string.Format("OnActionExecuted:{0}", DateTime.Now.Subtract(startActionExecution)));
        //    base.OnActionExecuted(filterContext);
        //}

        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    startResultExecution = DateTime.Now;
        //    Log.Debug("OnResultExecuting");
        //    base.OnResultExecuting(filterContext);
        //}

        //public override void OnResultExecuted(ResultExecutedContext filterContext)
        //{
        //    Log.Debug(string.Format("OnResultExecuted:{0}", DateTime.Now.Subtract(startResultExecution)));
        //    base.OnResultExecuted(filterContext);
        //}

        public void OnException(ExceptionContext filterContext)
        {
            Log.Error(filterContext.Exception);
        }
    }
}
