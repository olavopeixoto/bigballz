using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using BigBallz.Core;
using BigBallz.Infrastructure;
using BigBallz.Models;
using BigBallz.Services.L2S;

namespace BigBallz.Services
{
    public class AlertEndBetTask : ICronJobTask
    {
        private readonly IMailService _mailService;
        private readonly DateTime _expiration;

        public AlertEndBetTask(IMailService mailService, DateTime expiration)
        {
            _mailService = mailService;
            _expiration = expiration;
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
            using (var context = DataContextProvider.Get())
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
            using (var context = DataContextProvider.Get())
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
            CronJob.AddTask(new AlertEndBetTask(new MailService(), startTime));
        }
    }
}