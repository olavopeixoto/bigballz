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
    public class GetMatchResultsTask : ICronJobTask
    {
        private readonly DataContextProvider _provider;

        public GetMatchResultsTask(DataContextProvider provider)
        {
            _provider = provider;
        }

        public string Name
        {
            get { return "Atualiza resultado de partidas"; }
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
            get { return TimeSpan.FromMinutes(10); }
        }

        public void Run()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        public async Task RunAsync()
        {
            var matchDaysUrl = "https://feedmonster.onefootball.com/feeds/il/br/competitions/12/7932/matchdaysOverview.json";
            var matchDayResultsUrlFormat = "https://feedmonster.onefootball.com/feeds/il/br/competitions/12/7932/matchdays/{0}.json";
            int matchdayId = -1;

            using (var httpClient = new HttpClient(new WebRequestHandler
            {
                CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default),
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            }))
            {
                using (var templateRequest = new HttpRequestMessage(HttpMethod.Get, matchDaysUrl))
                {
                    using (var response = await httpClient.SendAsync(templateRequest).ConfigureAwait(false))
                    {
                        if (!response.IsSuccessStatusCode)
                            throw new InvalidOperationException(string.Format(
                                "Unable to load data. Url: {1} -- Status: {0}", (int) response.StatusCode,
                                templateRequest.RequestUri.AbsoluteUri));

                        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        var results = JsonConvert.DeserializeObject<MatchDaysResult>(json);

                        foreach (var matchDay in results.MatchDays)
                        {
                            if (DateTime.UtcNow >= matchDay.KickoffFirst && DateTime.UtcNow <= matchDay.KickoffLast)
                            {
                                matchdayId = matchDay.Id;
                                break;
                            }
                        }
                    }
                }

                if (matchdayId<0)
                {
                    return;
                }

                var matchDayResultsUrl = string.Format(matchDayResultsUrlFormat, matchdayId);

                using (var templateRequest = new HttpRequestMessage(HttpMethod.Get, matchDayResultsUrl))
                {
                    using (var response = await httpClient.SendAsync(templateRequest).ConfigureAwait(false))
                    {
                        if (!response.IsSuccessStatusCode)
                            throw new InvalidOperationException(string.Format(
                                "Unable to load data. Url: {1} -- Status: {0}", (int) response.StatusCode,
                                templateRequest.RequestUri.AbsoluteUri));

                        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        var results = JsonConvert.DeserializeObject<KickoffResult>(json);

                        using (var context = _provider.CreateContext())
                        {
                            var options = new DataLoadOptions();
                            options.LoadWith<Match>(x => x.Team1);
                            options.LoadWith<Match>(x => x.Team2);

                            context.LoadOptions = options;

                            var now = DateTime.UtcNow.BrazilTimeZone();

                            var matches = context.Matches
                                                    .Where(m => m.Stage.FifaId == matchdayId
                                                                && m.StartTime > now)
                                                    .ToList();

                            if (matches.Count == 0)
                                return;

                            foreach (var kickoff in results.Kickoffs)
                            {
                                if (kickoff.Kickoff < DateTime.UtcNow)
                                {
                                    foreach (var group in kickoff.Groups)
                                    {
                                        foreach (var match in group.Matches)
                                        {
                                            var bbMatch = matches.Single(x => x.StartTime == kickoff.Kickoff.BrazilTimeZone()
                                                && x.Team1.FifaId == match.TeamHome.IdInternal
                                                && x.Team2.FifaId == match.TeamAway.IdInternal);

                                            if (match.ScoreHome >= 0)
                                            {
                                                bbMatch.Score1 = match.ScoreHome;
                                            }

                                            if (match.ScoreAway >= 0)
                                            {
                                                bbMatch.Score2 = match.ScoreAway;
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
            CronJob.AddTask(new GetMatchResultsTask(ServiceLocator.Resolve<DataContextProvider>()));
        }

        public void Dispose()
        {}
    }
}