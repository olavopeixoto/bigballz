using System;
using System.Data.Linq;
using System.Linq;
using BigBallz.Core;
using BigBallz.Core.IoC;
using BigBallz.Infrastructure;
using BigBallz.Models;
using BigBallz.Services;

namespace BigBallz.Tasks
{
    public class BetExpirationWarningTask : ICronJobTask
    {
        private readonly IMailService _mailService;
        private readonly DateTime _expiration;
        private readonly DateTime _matchStart;
        private readonly DataContextProvider _provider;

        private static readonly TimeSpan[] AlertsTime = { TimeSpan.FromHours(2), TimeSpan.FromHours(3), TimeSpan.FromHours(12), TimeSpan.FromHours(24) };

        public BetExpirationWarningTask(IMailService mailService, DateTime expiration, TimeSpan timeBefore, DataContextProvider provider)
        {
            _mailService = mailService;
            _expiration = expiration.Add(timeBefore.Negate());
            _matchStart = expiration;
            _provider = provider;
        }

        public string Name
        {
            get { return string.Format("Envio de E-Mail aviso sem aposta ({0:s})", _matchStart); }
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

                var matches = context.Matches
                    .Where(match => match.StartTime == _matchStart)
                    .ToList();

                var players = context.Users
                    .Where(x => x.UserRoles.Any(y => y.Role.Name == BBRoles.Player)
                                && x.Bets.All(b => b.Match1.StartTime != _matchStart));

                players.ForEach(player => _mailService.SendBetWarning(player, matches.Where(m => m.Bets.All(b => b.User != player.UserId)).ToList()));
            }
        }

        public static void AddAllMatches()
        {
            var provider = ServiceLocator.Resolve<DataContextProvider>();

            using (var context = provider.CreateContext())
            {
                context.Matches
                    .Where(x => !x.Score1.HasValue && !x.Score2.HasValue)
                    .GroupBy(x => x.StartTime)
                    .ForEach(x => AddTask(x.Key));
            }
        }

        public static void AddTask(DateTime startTime)
        {
            AlertsTime.ForEach(t =>
            {
                if (startTime.Add(t.Negate()) > DateTime.Now.BrazilTimeZone())
                    CronJob.AddTask(new BetExpirationWarningTask(ServiceLocator.Resolve<IMailService>(), startTime, t, ServiceLocator.Resolve<DataContextProvider>()));
            });
        }

        public void Dispose()
        {}
    }
}