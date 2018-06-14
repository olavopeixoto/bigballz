using System;
using System.Linq;
using BigBallz.Core;
using BigBallz.Core.IoC;
using BigBallz.Services;

namespace BigBallz.Tasks
{
    public class AlertEndBonusTask : ICronJobTask
    {
        private readonly IMailService _mailService;
        private readonly DateTime _expiration;
        private readonly IBonusBetService _bonusBetService;
        private readonly IAccountService _accountService;

        public AlertEndBonusTask(IMailService mailService, DateTime expiration, IBonusBetService bonusBetService, IAccountService accountService)
        {
            _mailService = mailService;
            _expiration = expiration;
            _bonusBetService = bonusBetService;
            _accountService = accountService;
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
            foreach (var user in _accountService.GetAllPlayers())
            {
                _mailService.SendEndBonusAlert(user, _bonusBetService.GetAll().ToList());
            }
        }

        public static void AddTask()
        {
            var service = ServiceLocator.Resolve<IBigBallzService>();

            var startTime = service.GetFirstMatch().StartTime.AddHours(-1);

            if (startTime < DateTime.UtcNow.BrazilTimeZone())
            {
                return;
            }

            CronJob.AddTask(new AlertEndBonusTask(ServiceLocator.Resolve<IMailService>(), startTime, ServiceLocator.Resolve<IBonusBetService>(), ServiceLocator.Resolve<IAccountService>()));
        }

        public void Dispose()
        {}
    }
}