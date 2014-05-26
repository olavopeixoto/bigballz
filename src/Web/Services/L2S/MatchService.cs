using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using BigBallz.Core;
using BigBallz.Infrastructure;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class MatchService : IMatchService
    {
        private readonly DataContextProvider _provider;

        public MatchService(DataContextProvider provider)
        {
            _provider = provider;
        }

        public IEnumerable<Match> GetAll()
        {
            var options = new DataLoadOptions();
            options.LoadWith<Match>(x => x.Team1);
            options.LoadWith<Match>(x => x.Team2);
            options.LoadWith<Match>(x => x.Stage);
            options.LoadWith<Team>(x => x.Group);

            using (var db = _provider.CreateContext())
            {
                db.LoadOptions = options;

                return db.Matches.ToList();
            }
        }

        public Match Get(int id)
        {
            var options = new DataLoadOptions();
            options.LoadWith<Match>(x => x.Team1);
            options.LoadWith<Match>(x => x.Team2);
            options.LoadWith<Match>(x => x.Stage);

            using (var db = _provider.CreateContext())
            {
                db.LoadOptions = options;

                return db.Matches.SingleOrDefault(d => d.MatchId == id);
            }
        }

        public IEnumerable<Match> GetNextMatches()
        {
            var options = new DataLoadOptions();
            options.LoadWith<Match>(x => x.Team1);
            options.LoadWith<Match>(x => x.Team2);

            using (var db = _provider.CreateContext())
            {
                db.LoadOptions = options;

                return db.Matches.Where(x => x.StartTime > DateTime.Now.BrazilTimeZone())
                        .Take(5)
                        .OrderBy(x => x.StartTime).ToList();
            }
        }

        public IEnumerable<Match> GetLastPlayedMatches()
        {
            var options = new DataLoadOptions();
            options.LoadWith<Match>(x => x.Team1);
            options.LoadWith<Match>(x => x.Team2);

            using (var db = _provider.CreateContext())
            {
                db.LoadOptions = options;

                return
                    db.Matches.Where(x => x.StartTime < DateTime.Now.BrazilTimeZone())
                        .OrderByDescending(x => x.StartTime)
                        .Take(5).ToList();
            }
        }

        public void Add(Match match)
        {
            using (var db = _provider.CreateContext())
            {
                db.Matches.InsertOnSubmit(match);
                AlertEndBetTask.AddTask(match.StartTime.AddHours(-1));
                BetExpirationWarningTask.AddTask(match.StartTime.AddHours(-2));
            }
        }

        public void Delete(Match match)
        {
            using (var db = _provider.CreateContext())
            {
                db.Matches.DeleteOnSubmit(match);
            }
        }

        public void Save()
        {
            using (var db = _provider.CreateContext())
            {
                db.SubmitChanges();
            }
        }

        public DateTime GetStartDate()
        {
            using (var db = _provider.CreateContext())
            {
                return db.Matches.Min(x => (DateTime?) x.StartTime) ?? DateTime.Now;
            }
        }
    }
}