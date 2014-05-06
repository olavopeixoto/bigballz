using System.Web;
using System.Web.Compilation;
using System.Web.Routing;
using System.Web.UI;

namespace BigBallz.Core.Web.Modules
{
    public class WebFormRouteHandler : IRouteHandler
    {
        public WebFormRouteHandler(string virtualPath)
        {
            VirtualPath = virtualPath;
        }

        public string VirtualPath { get; private set; }

        public IHttpHandler GetHttpHandler(RequestContext
              requestContext)
        {
            var page = BuildManager.CreateInstanceFromVirtualPath
                 (VirtualPath, typeof(Page)) as IHttpHandler;

            foreach (var urlParm in requestContext.RouteData.Values)
            {
                requestContext.HttpContext.Items[urlParm.Key] = urlParm.Value;
            }

            return page;
        }
    }

}
