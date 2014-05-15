using System.Configuration;
using System.Net;
using System.Web;
using BigBallz.Core.Caching;
using BigBallz.Core.Log;
using BigBallz.Models;
using BigBallz.Services;
using BigBallz.Services.Cache;
using BigBallz.Services.L2S;
using RPXLib;
using RPXLib.Interfaces;
using StructureMap.Configuration.DSL;

namespace BigBallz.Infrastructure
{
    public class DependenciesRegistry : Registry
    {
        public DependenciesRegistry()
        {
            For<ILogger>()
                .Singleton()
                .Use<ElmahLogger>();

            For<IRPXService>()
                .HttpContextScoped()
                .Use(x =>
                {
                    var baseUrl = ConfigurationManager.AppSettings["rpxnow-baseurl"];
                    var apiKey = ConfigurationManager.AppSettings["rpxnow-apikey"];

                    //if you need to access the service via a web proxy set the proxy details here
                    const IWebProxy webProxy = null;

                    var settings = new RPXApiSettings(baseUrl, apiKey, webProxy);
                    return new RPXService(settings);
                });

            For<System.Web.Caching.Cache>()
                .HttpContextScoped()
                .Use(x => HttpContext.Current.Cache);

            For<ICache>()
                .HttpContextScoped()
                .Use<WebCacheWrapper>();

            For<DataContextProvider>()
                .Singleton()
                .Use<DataContextProvider>();

            For<BigBallzDataContext>()
                .HttpContextScoped()
                .Use(x => x.GetInstance<DataContextProvider>().CreateContext());

            For<IAccountService>()
                .HttpContextScoped()
                .Use<AccountService>();

            For<IBigBallzService>()
                .HttpContextScoped()
                .Use(x => new BigBallsServiceCache(x.GetInstance<ICache>(), new BigBallzService(x.GetInstance<BigBallzDataContext>())));

            For<IBonusBetService>()
                .HttpContextScoped()
                .Use(x => new BonusBetServiceCache(x.GetInstance<ICache>(), new BonusBetService(x.GetInstance<BigBallzDataContext>())));

            For<IBonusService>()
                .HttpContextScoped()
                .Use<BonusService>();

            For<ICommentsService>()
                .HttpContextScoped()
                .Use<CommentsService>();

            For<IGroupService>()
                .HttpContextScoped()
                .Use<GroupService>();

            For<IMailService>()
                .HttpContextScoped()
                .Use<MailService>();

            For<IMatchBetService>()
                .HttpContextScoped()
                .Use<MatchBetService>();

            For<IMatchService>()
                .HttpContextScoped()
                .Use(x => new MatchServiceCache(x.GetInstance<ICache>(), new MatchService(x.GetInstance<DataContextProvider>())));

            For<IRoleService>()
                .HttpContextScoped()
                .Use<RoleService>();

            For<IStageService>()
                .HttpContextScoped()
                .Use<StageService>();

            For<ITeamService>()
                .HttpContextScoped()
                .Use<TeamService>();

            For<IUserService>()
                .HttpContextScoped()
                .Use<UserService>();
        }
    }
}