using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;
using BigBallz.Core;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class MatchBetService : IMatchBetService
    {
        readonly BigBallzDataContext _db;

        public MatchBetService(BigBallzDataContext context)
        {
            _db = context;

            var options = new DataLoadOptions();
            options.LoadWith<BonusBet>(x => x.Bonus11);
            options.LoadWith<BonusBet>(x => x.Team1);
            options.LoadWith<Bet>(x => x.Match1);
            options.LoadWith<Match>(x => x.Team1);
            options.LoadWith<Match>(x => x.Team2);

            _db.LoadOptions = options;
        }

        public IEnumerable<Bet> GetAll()
        {
            return _db.Bets.OrderBy(x => x.BetId).ToList();
        }

        public IEnumerable<Bet> GetAll(string userName)
        {
            return _db.Bets.Where(d => d.User1.UserName == userName).ToList();
        }

        public IEnumerable<Bet> GetAll(int userId)
        {
            return _db.Bets.Where(d => d.User == userId).ToList();
        }

        public IEnumerable<Bet> GetAllExpired(int userId)
        {
            return _db.Bets.Where(d => d.User == userId && d.Match1.StartTime.AddHours(-1) < DateTime.Now.BrazilTimeZone()).ToList();
        }

        public Bet Get(int betId)
        {
            return _db.Bets.SingleOrDefault(d => d.BetId == betId);
        }

        public void Add(Bet bet)
        {
            var match = _db.Matches.Single(x => x.MatchId == bet.Match);
            if (match.StartTime.AddHours(-1) >= DateTime.Now.BrazilTimeZone())
            {
                _db.Bets.InsertOnSubmit(bet);
            }
            else
            {
                throw new ValidationException(string.Format("Apostas encerradas para a partida {0} X {1}", match.Team1.Name, match.Team2.Name));
            }
        }

        public void Add(IList<Bet> bets)
        {
            _db.Bets.InsertAllOnSubmit(bets.Where(x => _db.Matches.Where(y => y.StartTime.AddHours(-1) >= DateTime.Now.BrazilTimeZone()).Select(y => y.MatchId).Contains(x.Match)));
        }

        public void Delete(Bet bet)
        {
            _db.Bets.DeleteOnSubmit(bet);
        }

        public void Save()
        {
            _db.SubmitChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
