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
    public class BetExpirationWarningTask : ICronJobTask
    {
        private readonly IMailService _mailService;
        private readonly DateTime _expiration;
        private readonly DataContextProvider _provider;

        public BetExpirationWarningTask(IMailService mailService, DateTime expiration, DataContextProvider provider)
        {
            _mailService = mailService;
            _expiration = expiration;
            _provider = provider;
        }

        public string Name
        {
            get { return "Envio de E-Mail aviso sem aposta"; }
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
            IList<User> players;
            using (var context = _provider.CreateContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<User>(x => x.Bets);
                loadOptions.LoadWith<Bet>(x => x.Match1);
                loadOptions.LoadWith<Match>(x => x.Team1);
                loadOptions.LoadWith<Match>(x => x.Team2);
                context.LoadOptions = loadOptions;

                players = context.Users.Where(x => x.UserRoles.Any(y => y.Role.Name == BBRoles.Player) && x.Bets.Any(b => b.Match1.StartTime.AddHours(-2) == AbsoluteExpiration && !(b.Score1.HasValue && b.Score2.HasValue))).ToList();
            }

            foreach (var player in players)
            {
                _mailService.SendBetWarning(player, player.Bets.Where(b => b.Match1.StartTime.AddHours(-2) == AbsoluteExpiration && !(b.Score1.HasValue && b.Score2.HasValue)).ToList());
            }
        }

        public static void AddAllMatches()
        {
            var provider = ServiceLocator.Resolve<DataContextProvider>();

            using (var context = provider.CreateContext())
            {
                var betAlertTime = context.Matches.Where(x => x.StartTime.AddHours(-2) >= DateTime.Now.BrazilTimeZone()).GroupBy(x => x.StartTime).Select(x => x.Key.AddHours(-2)).OrderBy(x => x).ToList();
                foreach (var startTime in betAlertTime)
                {
                    AddTask(startTime);
                }
            }
        }

        public static void AddTask(DateTime startTime)
        {
            CronJob.AddTask(new BetExpirationWarningTask(ServiceLocator.Resolve<IMailService>(), startTime, ServiceLocator.Resolve<DataContextProvider>()));
        }

        public void Dispose()
        {}
    }
}