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
    public class BonusExpirationWarningTask : ICronJobTask
    {
        private readonly IMailService _mailService;
        private readonly DateTime _expiration;
        private readonly IBigBallzService _bigBallzService;
        private readonly DataContextProvider _provider;

        public BonusExpirationWarningTask(IMailService mailService, DateTime expiration, IBigBallzService bigBallzService, DataContextProvider provider)
        {
            _mailService = mailService;
            _expiration = expiration;
            _bigBallzService = bigBallzService;
            _provider = provider;
        }

        public string Name
        {
            get { return "Envio de EMail fim do Bonus"; }
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
                loadOptions.LoadWith<User>(x => x.BonusBets);
                loadOptions.LoadWith<User>(x => x.UserRoles);
                loadOptions.LoadWith<UserRole>(x => x.Role);
                context.LoadOptions = loadOptions;

                var startTime = _bigBallzService.GetFirstMatch().StartTime;

                var bonus = context.Bonus;

                var players = context.Users
                    .Where(x => x.Authorized
                            && bonus.Any(b => x.BonusBets
                                                .All(bb => bb.Bonus != b.BonusId || bb.Team == null || bb.Team == "")))
                    .ToArray();

                foreach (var user in players)
                {
                    _mailService.SendBonusExpirationWarning(user, startTime);
                }
            }
        }

        public static void AddTask(int numberOfAlerts)
        {
            var service = ServiceLocator.Resolve<IBigBallzService>();

            var startTime = service.GetFirstMatch().StartTime;

            for(var i=numberOfAlerts+1 ; i > 1 ; i--)
            {
                var triggerDate = startTime.AddHours(i * -1);

                if (triggerDate > DateTime.UtcNow.BrazilTimeZone())
                {
                    continue;
                }

                CronJob.AddTask(new BonusExpirationWarningTask(ServiceLocator.Resolve<IMailService>(), triggerDate, ServiceLocator.Resolve<IBigBallzService>(), ServiceLocator.Resolve<DataContextProvider>()));
            }
        }

        public void Dispose()
        {}
    }
}