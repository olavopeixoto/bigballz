using System;
using System.Collections.Generic;
using System.Data.Linq;
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
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Bet>(x => x.Match1);
                loadOptions.LoadWith<Bet>(x => x.User1);
                loadOptions.LoadWith<Match>(x => x.Team1);
                loadOptions.LoadWith<Match>(x => x.Team2);
                context.LoadOptions = loadOptions;
                bets =
                    context.Bets.Where(x => x.Match1.StartTime.AddHours(-1) == AbsoluteExpiration).OrderBy(
                        x => x.Match).ThenBy(x => x.User1.UserName).ToList();
                players = context.Users.Where(x => x.UserRoles.Any(y => y.Role.Name == BBRoles.Player)).ToList();
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
                var betEndTime = context.Matches.Where(x => !x.Score1.HasValue && !x.Score2.HasValue && x.StartTime.AddHours(-1) >= DateTime.Now.BrazilTimeZone()).GroupBy(x => x.StartTime).Select(x => x.Key.AddHours(-1)).OrderBy(x => x).ToList();
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