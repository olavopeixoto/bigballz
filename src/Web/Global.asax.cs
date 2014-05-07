using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BigBallz.Core.Web.MVC.Filters;
using BigBallz.Infrastructure.Mvc;

namespace BigBallz
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("public/{*pathInfo}");

            routes.MapRoute(
                "rpx",
                "rpx",
                new { controller = "Auth", action = "HandleResponse", id = "" }
                );

            routes.MapRoute(
                "confirmacaopagamento",
                "confirmacaopagamento",
                new { controller = "Auth", action = "confirmacaopagamento", id = "" }
                );

            routes.MapRoute(
                "regulamento",
                "regulamento",
                new { controller = "Home", action = "Rules", id = "" }
                );

            routes.MapRoute(
                "inscricao",
                "inscricao",
                new { controller = "Auth", action = "Join", id = "" }
                );

            routes.MapRoute(
                "aposta",
                "aposta",
                new { controller = "bet", action = "index", id = "" }
                );

            routes.MapRoute(
                "apostaUsuario",
                "aposta/{id}",
                new { controller = "bet", action = "expired" }
                );

            routes.MapRoute(
                "classificacao",
                "classificacao",
                new { controller = "standings", action = "index", id = "" }
                );

            routes.MapRoute(
                "verificaremail",
                "verificaremail/{id}",
                new { controller = "auth", action = "activate", id = "" }
                );

            routes.MapRoute(
                "pagamento",
                "pagamento",
                new { controller = "auth", action = "payment", id = "" }
                );

            routes.MapRoute(
                "comentarios",
                "comentarios",
                new { controller = "comments", action = "index", id = "" }
                );

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true; //Não incluir header de identificação do framework (Segurança)

            AreaRegistration.RegisterAllAreas();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new BigBallzViewEngine());

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            AlertEndBetTask.AddAllMatches();
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (FormsAuthentication.RequireSSL)
                filters.Add(new RequireHttpsAttribute());

            filters.Add(new ElmahExceptionHandlerAttribute());
        }

#if DEBUG2
        protected void FormsAuthentication_OnAuthenticate(object sender, FormsAuthenticationEventArgs args)
        {
            args.Context.SkipAuthorization = true;
            args.User = new GenericPrincipal(new GenericIdentity("Olavo Castro"), new[] { BBRoles.Admin, BBRoles.Player });
        }
#endif
    }
}