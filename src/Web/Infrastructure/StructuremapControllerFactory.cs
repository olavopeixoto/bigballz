using System;
using System.Web.Mvc;
using System.Web.Routing;
using BigBallz.Core.IoC;
using BigBallz.Core.Log;
using StructureMap;

namespace BigBallz.Infrastructure
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null) return base.GetControllerInstance(requestContext, null);

            try
            {
                return ServiceLocator.Resolve<IController>(controllerType);
            }
            catch (StructureMapException ex)
            {
                var appex = new ApplicationException(
                    ex.Message + Environment.NewLine + ObjectFactory.WhatDoIHave(), ex);
                Logger.Error(appex);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return base.GetControllerInstance(requestContext, controllerType);
        }
    }
}