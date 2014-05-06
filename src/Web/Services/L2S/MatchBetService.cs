using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BigBallz.Core;
using BigBallz.Services;

namespace BigBallz.Models
{
    public class MatchBetService : IMatchBetService
    {
        readonly BigBallzDataContext db = new BigBallzDataContext();

        public IQueryable<Bet> GetAll()
        {
            return db.Bets.OrderBy(x => x.BetId);
        }

        public IQueryable<Bet> GetAll(string userName)
        {
            return db.Bets.Where(d => d.User1.UserName == userName);
        }

        public IQueryable<Bet> GetAll(int userId)
        {
            return db.Bets.Where(d => d.User == userId);
        }

        public IQueryable<Bet> GetAllExpired(int userId)
        {
            return db.Bets.Where(d => d.User == userId && d.Match1.StartTime.AddHours(-1) < DateTime.Now.BrazilTimeZone());
        }

        public Bet Get(int betId)
        {
            return db.Bets.SingleOrDefault(d => d.BetId == betId);
        }

        public void Add(Bet bet)
        {
            var match = db.Matches.FirstOrDefault(x => x.MatchId == bet.Match);
            if (match.StartTime.AddHours(-1) >= DateTime.Now.BrazilTimeZone())
            {
                db.Bets.InsertOnSubmit(bet);
            }
            else
            {
                throw new ValidationException(string.Format("Apostas encerradas para a partida {0} X {1}", match.Team1.Name, match.Team2.Name));
            }
        }

        public void Add(System.Collections.Generic.IList<Bet> bets)
        {
            db.Bets.InsertAllOnSubmit(bets.Where(x => db.Matches.Where(y => y.StartTime.AddHours(-1) >= DateTime.Now.BrazilTimeZone()).Select(y => y.MatchId).Contains(x.Match)));
        }

        public void Delete(Bet bet)
        {
            db.Bets.DeleteOnSubmit(bet);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

    }
}
