using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BigBallz.Core.Web.MVC
{
    public class PartialRequest
    {
        public RouteValueDictionary RouteValues { get; private set; }

        public PartialRequest(object routeValues)
        {
            RouteValues = new RouteValueDictionary(routeValues);
        }

        public void Invoke(ControllerContext context)
        {
            var rd = new RouteData(context.RouteData.Route, context.RouteData.RouteHandler);
            foreach (var pair in RouteValues)
                rd.Values.Add(pair.Key, pair.Value);
            IHttpHandler handler = new MvcHandler(new RequestContext(context.HttpContext, rd));
            handler.ProcessRequest(System.Web.HttpContext.Current);
        }
    }
}