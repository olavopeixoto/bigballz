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
                //var loadOptions = new DataLoadOptions();
                //loadOptions.LoadWith<User>(x => x.Bets);
                //loadOptions.LoadWith<Bet>(x => x.Match1);
                //loadOptions.LoadWith<Match>(x => x.Team1Id);
                //loadOptions.LoadWith<Match>(x => x.Team2Id);
                //context.LoadOptions = loadOptions;

                players = context.Users
                    .Include(x => x.Bets)
                    .Where(x => x.Roles.Any(y => y.Name == BBRoles.Player)
                            && x.Bets.Any(b => DbFunctions.AddHours(b.Match.StartTime, -2) == AbsoluteExpiration))
                    .ToList();
            }

            foreach (var player in players)
            {
                _mailService.SendBetWarning(player, player.Bets.Where(b => b.Match.StartTime.AddHours(-2) == AbsoluteExpiration).ToList());
            }
        }

        public static void AddAllMatches()
        {
            var provider = ServiceLocator.Resolve<DataContextProvider>();

            using (var context = provider.CreateContext())
            {
                var now = DateTime.Now.BrazilTimeZone();
                var betAlertTime = context.Matches
                                            .Where(x => DbFunctions.AddHours(x.StartTime, -2) >= now)
                                            .GroupBy(x => x.StartTime)
                                            .Select(x => DbFunctions.AddHours(x.Key, -2).Value)
                                            .OrderBy(x => x)
                                            .ToList();

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