using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BigBallz.Core;
using BigBallz.Core.Caching;
using BigBallz.Models;

namespace BigBallz.Services.Cache
{
    public class MatchBetServiceCache : IMatchBetService
    {
        readonly BigBallzDataContext _db;
        private readonly ICache _cache;

        public MatchBetServiceCache(BigBallzDataContext context, ICache cache)
        {
            _db = context;
            _cache = cache;
        }

        public IQueryable<Bet> GetAll()
        {
            return _db.Bets.OrderBy(x => x.BetId);
        }

        public IQueryable<Bet> GetAll(string userName)
        {
            return _db.Bets.Where(d => d.User1.UserName == userName);
        }

        public IQueryable<Bet> GetAll(int userId)
        {
            return _db.Bets.Where(d => d.User == userId);
        }

        public IQueryable<Bet> GetAllExpired(int userId)
        {
            return _db.Bets.Where(d => d.User == userId && d.Match1.StartTime.AddHours(-1) < DateTime.Now.BrazilTimeZone());
        }

        public Bet Get(int betId)
        {
            return _db.Bets.SingleOrDefault(d => d.BetId == betId);
        }

        public void Add(Bet bet)
        {
            var match = _db.Matches.FirstOrDefault(x => x.MatchId == bet.Match);
            if (match.StartTime.AddHours(-1) >= DateTime.Now.BrazilTimeZone())
            {
                _db.Bets.InsertOnSubmit(bet);
            }
            else
            {
                throw new ValidationException(string.Format("Apostas encerradas para a partida {0} X {1}", match.Team1.Name, match.Team2.Name));
            }
        }

        public void Add(System.Collections.Generic.IList<Bet> bets)
        {
            _db.Bets.InsertAllOnSubmit(bets.Where(x => _db.Matches.Where(y => y.StartTime.AddHours(-1) >= DateTime.Now.BrazilTimeZone()).Select(y => y.MatchId).Contains(x.Match)));
        }

        public void Delete(Bet bet)
        {
            _db.Bets.DeleteOnSubmit(bet);
        }

        public void Save()
        {
            _cache.Clear();

            _db.SubmitChanges();
        }

    }
}
