using System;
using System.Collections.Generic;
using System.Linq;
using BigBallz.Core;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class MatchService : IMatchService
    {
        readonly BigBallzDataContext _db = new BigBallzDataContext();

        public IQueryable<Match> GetAll()
        {
            return _db.Matches;
        }

        public Match Get(int id)
        {
            return _db.Matches.SingleOrDefault(d => d.MatchId == id);
        }

        public IEnumerable<Match> GetNextMatches()
        {
            return _db.Matches.Where(x => x.StartTime > DateTime.Now.BrazilTimeZone()).Take(5).OrderBy(x => x.StartTime);
        }

        public IEnumerable<Match> GetLastPlayedMatches()
        {
            return _db.Matches.Where(x => x.StartTime < DateTime.Now.BrazilTimeZone()).OrderByDescending(x => x.StartTime).Take(5);
        }

        public void Add(Match match)
        {
            _db.Matches.InsertOnSubmit(match);
            AlertEndBetTask.AddTask(match.StartTime.AddHours(-1));
        }

        public void Delete(Match match)
        {
            _db.Matches.DeleteOnSubmit(match);
        }

        public void Save()
        {
            _db.SubmitChanges();
        }

        public DateTime GetStartDate()
        {
            return _db.Matches.Min(x => (DateTime?)x.StartTime) ?? DateTime.Now;
        }
    }
}