using System;
using System.Data.Linq;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Threading.Tasks;
using BigBallz.Core;
using BigBallz.Core.IoC;
using BigBallz.Infrastructure;
using BigBallz.Models;
using BigBallz.Models.FootballApi;
using BigBallz.Services;
using Newtonsoft.Json;

namespace BigBallz.Tasks
{
    public class AddMatchesTask : ICronJobTask
    {
        private readonly DataContextProvider _provider;

        public AddMatchesTask(DataContextProvider provider)
        {
            _provider = provider;
        }

        public string Name
        {
            get { return "Cadastra novas partidas"; }
        }

        public bool Recurring
        {
            get { return true; }
        }

        public DateTime? AbsoluteExpiration
        {
            get
            {
                return null;
            }
        }

        public TimeSpan? SlidingExpiration
        {
            get { return TimeSpan.FromMinutes(5); }
        }

        public void Run()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        public async Task RunAsync()
        {
            using (var context = _provider.CreateContext())
            {
                var options = new DataLoadOptions();
                options.LoadWith<Match>(x => x.Team1);
                options.LoadWith<Match>(x => x.Team2);

                context.LoadOptions = options;

                var stages = context.Stages.ToList();
                var teams = context.Teams.ToList();

                var matchDaysUrl = "https://feedmonster.onefootball.com/feeds/il/br/competitions/12/7932/matchdaysOverview.json";
                var matchDayResultsUrlFormat = "https://feedmonster.onefootball.com/feeds/il/br/competitions/12/7932/matchdays/{0}.json";

                using (var httpClient = new HttpClient(new WebRequestHandler
                {
                    CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default),
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
                }))
                {
                    using (var matchdaysRequest = new HttpRequestMessage(HttpMethod.Get, matchDaysUrl))
                    {
                        using (var matchdaysResponse = await httpClient.SendAsync(matchdaysRequest).ConfigureAwait(false))
                        {
                            if (!matchdaysResponse.IsSuccessStatusCode)
                                throw new InvalidOperationException(string.Format(
                                    "Unable to load data. Url: {1} -- Status: {0}", (int) matchdaysResponse.StatusCode,
                                    matchdaysRequest.RequestUri.AbsoluteUri));

                            var matchdaysJson = await matchdaysResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                            var matchdaysResult = JsonConvert.DeserializeObject<MatchDaysResult>(matchdaysJson);

                            foreach (var matchDay in matchdaysResult.MatchDays)
                            {
                                if (!matchDay.IsCurrentMatchday || !matchDay.HasMatchdayFeed || matchDay.KickoffLast < DateTime.UtcNow)
                                    continue;

                                var matchDayResultsUrl = string.Format(matchDayResultsUrlFormat, matchDay.Id);

                                using (var matchdayRequest = new HttpRequestMessage(HttpMethod.Get, matchDayResultsUrl))
                                {
                                    using (var matchdayResponse = await httpClient.SendAsync(matchdayRequest).ConfigureAwait(false))
                                    {
                                        if (!matchdayResponse.IsSuccessStatusCode)
                                            throw new InvalidOperationException(string.Format(
                                                "Unable to load data. Url: {1} -- Status: {0}",
                                                (int) matchdayResponse.StatusCode,
                                                matchdayRequest.RequestUri.AbsoluteUri));

                                        var matchdayJson = await matchdayResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                                        var matchdayResult = JsonConvert.DeserializeObject<KickoffResult>(matchdayJson);

                                        var dbMatches = context.Matches.Where(m => m.Stage.FifaId == matchDay.Id).ToList();

                                        foreach (var kickoff in matchdayResult.Kickoffs)
                                        {
                                            foreach (var group in kickoff.Groups)
                                            {
                                                foreach (var match in group.Matches)
                                                {
                                                    if (dbMatches.Any(m =>
                                                        m.StartTime == kickoff.Kickoff.BrazilTimeZone()
                                                        && m.Team1.FifaId == match.TeamHome.IdInternal
                                                        && m.Team2.FifaId == match.TeamAway.IdInternal))
                                                    {
                                                        continue;
                                                    }

                                                    if (teams.All(t => t.FifaId != match.TeamHome.IdInternal)
                                                        || teams.All(t => t.FifaId != match.TeamAway.IdInternal)
                                                        || stages.All(s => s.FifaId != matchDay.Id))
                                                    {
                                                        continue;
                                                    }

                                                    var matchToAdd = new Match
                                                    {
                                                        StartTime = kickoff.Kickoff.BrazilTimeZone(),
                                                        Team1Id       = teams.Where(t => t.FifaId == match.TeamHome.IdInternal).Select(x => x.TeamId).Single(),
                                                        Team2Id       = teams.Where(t => t.FifaId == match.TeamAway.IdInternal).Select(x => x.TeamId).Single(),
                                                        StageId = stages.Where(s => s.FifaId == matchDay.Id).Select(s => s.StageId).Single()
                                                    };

                                                    context.Matches.InsertOnSubmit(matchToAdd);
                                                }
                                            }   
                                        }
                                    }
                                }
                            }

                            context.SubmitChanges();
                        }
                    }
                }
            }
        }

        public static void AddTask()
        {
            CronJob.AddTask(new AddMatchesTask(ServiceLocator.Resolve<DataContextProvider>()));
        }

        public void Dispose()
        {}
    }
}