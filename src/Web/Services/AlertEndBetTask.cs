using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BigBallz.Core;
using BigBallz.Core.IoC;
using BigBallz.Infrastructure;
using BigBallz.Models;

namespace BigBallz.Services
{
    public class AlertEndBetTask : ICronJobTask
    {
        private readonly IMailService _mailService;
        private readonly DateTime _expiration;
        private readonly DataContextProvider _provider;

        public AlertEndBetTask(IMailService mailService, DateTime expiration, DataContextProvider provider)
        {
            _mailService = mailService;
            _expiration = expiration;
            _provider = provider;
        }

        public string Name
        {
            get { return "Envio de EMail fim das apostas"; }
        }

        public bool Recurring
        {
            get { return false; }
        }

        public DateTime? AbsoluteExpiration
        {
            get
            {
                return _expiration;
            }
        }

        public TimeSpan? SlidingExpiration
        {
            get { return null; }
        }

        public void Run()
        {
            IList<Bet> bets;
            IList<User> players;
            using (var context = _provider.CreateContext())
            {
                //var loadOptions = new DataLoadOptions();
                //loadOptions.LoadWith<Bet>(x => x.Match1);
                //loadOptions.LoadWith<Bet>(x => x.User1);
                //loadOptions.LoadWith<Match>(x => x.Team1Id);
                //loadOptions.LoadWith<Match>(x => x.Team2Id);
                //context.LoadOptions = loadOptions;

                bets = context.Bets
                                .Where(x => DbFunctions.AddHours(x.Match.StartTime, -1) == AbsoluteExpiration)
                                .OrderBy(x => x.MatchId)
                                .ThenBy(x => x.User.UserName)
                                .ToList();

                players = context.Users.Where(x => x.Roles.Any(y => y.Name == BBRoles.Player)).ToList();
            }

            foreach (var player in players)
            {
                if (bets.Count > 0) _mailService.SendEndBetAlert(player, bets);
            }
        }

        public static void AddAllMatches()
        {
            var provider = ServiceLocator.Resolve<DataContextProvider>();

            using (var context = provider.CreateContext())
            {
                var now = DateTime.Now.BrazilTimeZone();
                var betEndTime = context.Matches
                                    .Where(x => !x.Score1.HasValue
                                                && !x.Score2.HasValue
                                                && DbFunctions.AddHours(x.StartTime, -1) >= now)
                                    .GroupBy(x => x.StartTime)
                                    .Select(x => DbFunctions.AddHours(x.Key, -1).Value)
                                    .OrderBy(x => x)
                                    .ToList();

                foreach(var startTime in betEndTime)
                {
                    AddTask(startTime);
                }
            }
        }

        public static void AddTask(DateTime startTime)
        {
            CronJob.AddTask(new AlertEndBetTask(ServiceLocator.Resolve<IMailService>(), startTime, ServiceLocator.Resolve<DataContextProvider>()));
        }

        public void Dispose()
        {}
    }
}