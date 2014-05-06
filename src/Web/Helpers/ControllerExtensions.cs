namespace System.Web.Mvc
{
    public static class ControllerExtensions
    {
        public static ActionResult AjaxView(this Controller controller)
        {
            return controller.Request.IsAjaxRequest()
                       ? new ViewResult { ViewName = "_" + controller.RouteData.GetRequiredString("action") }
                       : new ViewResult();
        }

        public static ActionResult AjaxView(this Controller controller, object model)
        {
            controller.ViewData.Model = model;
            return controller.Request.IsAjaxRequest()
                       ? new ViewResult { ViewName = "_" + controller.RouteData.GetRequiredString("action") }
                       : new ViewResult();
        }
        
    }
}