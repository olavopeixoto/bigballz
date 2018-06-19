using System;
using System.Data.Linq;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Threading.Tasks;
using BigBallz.Core;
using BigBallz.Core.Caching;
using BigBallz.Core.IoC;
using BigBallz.Infrastructure;
using BigBallz.Models;
using BigBallz.Models.FootballApi;
using BigBallz.Services;
using Elmah;
using Newtonsoft.Json;

namespace BigBallz.Tasks
{
    public class GetMatchResultsTask : ICronJobTask
    {
        private readonly DataContextProvider _provider;
        private readonly ICache _cache;

        public GetMatchResultsTask(DataContextProvider provider, ICache cache)
        {
            _provider = provider;
            _cache = cache;
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
            get { return TimeSpan.FromMinutes(1); }
        }

        public void Run()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        public async Task RunAsync()
        {
            const int matchDurationOffset = -3;
            var utcNow = DateTime.UtcNow;
            var utcNowOffset = utcNow.AddHours(matchDurationOffset);

            var matchDaysUrl = "https://feedmonster.onefootball.com/feeds/il/br/competitions/12/7932/matchdaysOverview.json";
            var matchDayResultsUrlFormat = "https://feedmonster.onefootball.com/feeds/il/br/competitions/12/7932/matchdays/{0}.json";

            using (var httpClient = new HttpClient(new WebRequestHandler
            {
                CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default),
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            }))
            {
                int[] matchDays;
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

                        matchDays = results.MatchDays
                            .Where(matchDay => utcNow >= matchDay.KickoffFirst && utcNowOffset <= matchDay.KickoffLast)
                            .Select(matchDay => matchDay.Id).ToArray();
                    }
                }

                foreach (var matchdayId in matchDays)
                {
                    var matchDayResultsUrl = string.Format(matchDayResultsUrlFormat, matchdayId);

                    using (var templateRequest = new HttpRequestMessage(HttpMethod.Get, matchDayResultsUrl))
                    {
                        using (var response = await httpClient.SendAsync(templateRequest).ConfigureAwait(false))
                        {
                            if (!response.IsSuccessStatusCode)
                            {
                                ErrorLog.GetDefault(null).Log(new Error(new InvalidOperationException(string.Format(
                                    "Unable to load data. Url: {1} -- Status: {0}", (int) response.StatusCode,
                                    templateRequest.RequestUri.AbsoluteUri))));

                                continue;
                            }

                            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                            var results = JsonConvert.DeserializeObject<KickoffResult>(json);

                            var changedResult = false;

                            using (var context = _provider.CreateContext())
                            {
                                var options = new DataLoadOptions();
                                options.LoadWith<Match>(x => x.Team1);
                                options.LoadWith<Match>(x => x.Team2);

                                context.LoadOptions = options;

                                var matches = context.Matches
                                                        .Where(m => m.Stage.FifaId == matchdayId)
                                                        .ToList();

                                if (matches.Count == 0)
                                    continue;

                                foreach (var kickoff in results.Kickoffs.Where(k => k.Kickoff <= utcNow && k.Kickoff >= utcNowOffset))
                                {
                                    foreach (var group in kickoff.Groups)
                                    {
                                        foreach (var match in group.Matches.Where(match => match.Period != "PreMatch"))
                                        {
                                            var bbMatch = matches.FirstOrDefault(x =>
                                                x.StartTime == kickoff.Kickoff.BrazilTimeZone()
                                                && x.Team1.FifaId == match.TeamHome.IdInternal
                                                && x.Team2.FifaId == match.TeamAway.IdInternal);

                                            if (bbMatch == null)
                                            {
                                                ErrorLog.GetDefault(null).Log(new Error(new Exception(
                                                    "Match not found: {0} x {1} - {2}\nMatches: {3}".FormatWith(
                                                        match.TeamHome.IdInternal, match.TeamAway.IdInternal,
                                                        kickoff.Kickoff.BrazilTimeZone(),
                                                        JsonConvert.SerializeObject(matches)))));

                                                continue;
                                            }

                                            if (match.ScoreHome >= 0 && bbMatch.Score1 != match.ScoreHome)
                                            {
                                                bbMatch.Score1 = match.ScoreHome;
                                                changedResult = true;
                                            }

                                            if (match.ScoreAway >= 0 && bbMatch.Score2 != match.ScoreAway)
                                            {
                                                bbMatch.Score2 = match.ScoreAway;
                                                changedResult = true;
                                            }
                                        }
                                    }
                                }

                                if (changedResult) context.SubmitChanges();
                            }

                            if (changedResult) _cache.Clear();
                        }
                    }
                }
            }
        }

        public static void AddTask()
        {
            CronJob.AddTask(new GetMatchResultsTask(ServiceLocator.Resolve<DataContextProvider>(), ServiceLocator.Resolve<ICache>()));
        }

        public void Dispose()
        {}
    }
}