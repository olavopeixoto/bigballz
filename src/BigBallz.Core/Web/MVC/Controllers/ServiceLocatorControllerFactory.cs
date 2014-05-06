using System;
using System.Web.Mvc;
using System.Web.Routing;
using BigBallz.Core.IoC;

namespace BigBallz.Core.Web.MVC
{
    public class ServiceLocatorControllerFactory : DefaultControllerFactory {

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType) {
            return ServiceLocator.Resolve<IController>(controllerType);
        }
    }
}
