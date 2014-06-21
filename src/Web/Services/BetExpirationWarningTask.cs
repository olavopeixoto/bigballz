using System;
using System.Collections;
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

        private const int HoursBeforeStartTime = -3;

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
            using (var context = _provider.CreateContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<User>(x => x.Bets);
                loadOptions.LoadWith<User>(x => x.UserRoles);
                loadOptions.LoadWith<UserRole>(x => x.Role);
                loadOptions.LoadWith<Match>(x => x.Team1);
                loadOptions.LoadWith<Match>(x => x.Team2);
                loadOptions.LoadWith<Match>(x => x.Bets);
                context.LoadOptions = loadOptions;

                var matches = context.Matches.Where(match => match.StartTime.AddHours(HoursBeforeStartTime) != AbsoluteExpiration).ToList();

                var players = context.Users.Where(x => x.UserRoles.Any(y => y.Role.Name == BBRoles.Player)
                                    && x.Bets.All(b => b.Match1.StartTime.AddHours(HoursBeforeStartTime) != AbsoluteExpiration));

                players.ForEach(player => _mailService.SendBetWarning(player, matches));
            }
        }

        public static void AddAllMatches()
        {
            var provider = ServiceLocator.Resolve<DataContextProvider>();

            using (var context = provider.CreateContext())
            {
                context.Matches
                    .Where(x => x.StartTime.AddHours(HoursBeforeStartTime) >= DateTime.Now.BrazilTimeZone())
                    .GroupBy(x => x.StartTime)
                    .ForEach(x => AddTask(x.Key.AddHours(HoursBeforeStartTime)));
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