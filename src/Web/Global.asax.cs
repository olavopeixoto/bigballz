using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BigBallz.Core.Bootstrapper;
using BigBallz.Core.Web.MVC.Filters;
using BigBallz.Infrastructure;
using BigBallz.Services;
using StackExchange.Profiling;
using StackExchange.Profiling.Mvc;
using StructureMap.Web.Pipeline;

namespace BigBallz
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
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
                new { controller = "Auth", action = "confirmacaopagamento" }
                );

            routes.MapRoute(
                "sso",
                "sso",
                new { controller = "Auth", action = "HelpLogin", id = "" }
                );
            
            routes.MapRoute(
                "notification",
                "notificacao",
                new { controller = "Auth", action = "notification", id = "" }
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
                "sucesso",
                "sucesso",
                new {controller = "auth", action = "newaccountsuccess"});

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            Bootstrapper.Run();

            MvcHandler.DisableMvcResponseHeader = true; //Não incluir header de identificação do framework (Segurança)

            AreaRegistration.RegisterAllAreas();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ProfilingViewEngine(new WebFormViewEngine()));

            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            if (!Context.IsDebuggingEnabled)
            {
                AlertEndBetTask.AddAllMatches();
                BetExpirationWarningTask.AddAllMatches();
            }

            InitializeMiniProfiler();
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (FormsAuthentication.RequireSSL)
                filters.Add(new RequireHttpsAttribute());

            filters.Add(new ProfilingActionFilter());
            filters.Add(new ElmahExceptionHandlerAttribute());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            MiniProfiler.Start();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            HttpContextLifecycle.DisposeAndClearAll();

            if (MiniProfiler.Current != null) // they had a cookie
            {
                var allowed = AllowProfiler(Request);
                MiniProfiler.Stop(discardResults: !allowed);
            }
        }

        private bool AllowProfiler(HttpRequest request)
        {
            return request.Cookies["x-profiler"] != null && request.RequestContext.HttpContext.User.IsInRole("admin");
        }

        private void InitializeMiniProfiler()
        {
            // different RDBMS have different ways of declaring sql parameters - SQLite can understand inline sql parameters just fine
            // by default, sql parameters won't be displayed
            MiniProfiler.Settings.SqlFormatter = new StackExchange.Profiling.SqlFormatters.SqlServerFormatter();

            // these settings are optional and all have defaults, any matching setting specified in .RenderIncludes() will
            // override the application-wide defaults specified here, for example if you had both:
            //    MiniProfiler.Settings.PopupRenderPosition = RenderPosition.Right;
            //    and in the page:
            //    @MiniProfiler.RenderIncludes(position: RenderPosition.Left)
            // then the position would be on the left that that page, and on the right (the app default) for anywhere that doesn't
            // specified position in the .RenderIncludes() call.
            MiniProfiler.Settings.PopupRenderPosition = RenderPosition.Right; //defaults to left
            MiniProfiler.Settings.PopupMaxTracesToShow = 15;                  //defaults to 15
            //MiniProfiler.Settings.RouteBasePath = "~/mini-profiler-resources";               //e.g. /profiler/mini-profiler-includes.js

            // optional settings to control the stack trace output in the details pane
            // the exclude methods are not thread safe, so be sure to only call these once per appdomain

            //MiniProfiler.Settings.ExcludeType("SessionFactory"); // Ignore any class with the name of SessionFactory
            //MiniProfiler.Settings.ExcludeAssembly("NHibernate"); // Ignore any assembly named NHibernate
            //MiniProfiler.Settings.ExcludeMethod("Flush");        // Ignore any method with the name of Flush
            // MiniProfiler.Settings.ShowControls = true;
            MiniProfiler.Settings.StackMaxLength = 256;          // default is 120 characters

            // because profiler results can contain sensitive data (e.g. sql queries with parameter values displayed), we
            // can define a function that will authorize clients to see the json or full page results.
            // we use it on http://stackoverflow.com to check that the request cookies belong to a valid developer.
            MiniProfiler.Settings.Results_Authorize = AllowProfiler;

            // the list of all sessions in the store is restricted by default, you must return true to alllow it
            MiniProfiler.Settings.Results_List_Authorize = AllowProfiler;
        }

        //protected void FormsAuthentication_OnAuthenticate(object sender, FormsAuthenticationEventArgs args)
        //{
        //    args.Context.SkipAuthorization = true;
        //    args.User = new GenericPrincipal(new GenericIdentity("Olavo Castro"), new[] { BBRoles.Admin, BBRoles.Player });
        //}
    }
}