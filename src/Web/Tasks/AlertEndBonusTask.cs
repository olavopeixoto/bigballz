using System;
using System.Linq;
using BigBallz.Core;
using BigBallz.Core.IoC;
using BigBallz.Infrastructure;
using BigBallz.Services;

namespace BigBallz.Tasks
{
    public class AlertEndBonusTask : ICronJobTask
    {
        private readonly IMailService _mailService;
        private readonly DateTime _expiration;
        private readonly DataContextProvider _dataContextProvider;

        public AlertEndBonusTask(IMailService mailService, DateTime expiration, DataContextProvider dataContextProvider)
        {
            _mailService = mailService;
            _expiration = expiration;
            _dataContextProvider = dataContextProvider;
        }

        public string Name
        {
            get { return "Envio de EMail Bonus encerrado"; }
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
            using (var db = _dataContextProvider.CreateContext())
            {
                var allPlayers = db.Users.Where(x => x.Authorized).ToList();
                var allBonus = db.BonusBets.OrderBy(x => x.User).ThenBy(x => x.BonusBetId).ToList();

                foreach (var user in allPlayers)
                {
                    _mailService.SendEndBonusAlert(user, allBonus);
                }
            }
        }

        public static void AddTask()
        {
            var bigBallzService = ServiceLocator.Resolve<IBigBallzService>();

            var startTime = bigBallzService.GetFirstMatch().StartTime.AddHours(-1);

            if (startTime < DateTime.UtcNow.BrazilTimeZone())
            {
                return;
            }

            CronJob.AddTask(new AlertEndBonusTask(ServiceLocator.Resolve<IMailService>(), startTime, ServiceLocator.Resolve<DataContextProvider>()));
        }

        public void Dispose()
        {}
    }
}