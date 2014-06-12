using System.Collections.Generic;
using System.Linq;
using BigBallz.Core.Caching;
using BigBallz.Models;

namespace BigBallz.Services.Cache
{
    public class MatchBetServiceCache : IMatchBetService
    {
        private readonly ICache _cache;
        private readonly IMatchBetService _service;

        public MatchBetServiceCache(ICache cache, IMatchBetService service)
        {
            _cache = cache;
            _service = service;
        }

        public IEnumerable<Bet> GetAll()
        {
            var key = string.Format("BB-MatchBetServiceCache-GetAll");

            if (_cache.Contains(key))
            {
                return _cache.Get<IEnumerable<Bet>>(key);
            }

            var results = _service.GetAll().ToList();

            _cache.Set(key, results);

            return results;
        }

        public IEnumerable<Bet> GetAll(string userName)
        {
            var key = string.Format("BB-MatchBetServiceCache-GetAll-{0}", userName ?? string.Empty);

            if (_cache.Contains(key))
            {
                return _cache.Get<IEnumerable<Bet>>(key);
            }

            var results = _service.GetAll(userName).ToList();
            _cache.Set(key, results);

            return results;
        }

        public IEnumerable<Bet> GetAll(int userId)
        {
            var key = string.Format("BB-MatchBetServiceCache-GetAll-{0}", userId);

            if (_cache.Contains(key))
            {
                return _cache.Get<IEnumerable<Bet>>(key);
            }

            var results = _service.GetAll(userId).ToList();
            _cache.Set(key, results);

            return results;
        }

        public IEnumerable<Bet> GetAllExpired(int userId)
        {
            var key = string.Format("BB-MatchBetServiceCache-GetAllExpired-{0}", userId);

            if (_cache.Contains(key))
            {
                return _cache.Get<IEnumerable<Bet>>(key);
            }

            var results = _service.GetAllExpired(userId).ToList();
            _cache.Set(key, results);

            return results;
        }

        public Bet Get(int betId)
        {
            var key = string.Format("BB-MatchBetServiceCache-Get-{0}", betId);

            if (_cache.Contains(key))
            {
                return _cache.Get<Bet>(key);
            }

            var results = _service.Get(betId);
            _cache.Set(key, results);

            return results;
        }

        public void Add(Bet bet)
        {
            _cache.Clear();
            _service.Add(bet);
        }

        public void Add(IList<Bet> bets)
        {
            _cache.Clear();
            _service.Add(bets);
        }

        public void Delete(Bet bet)
        {
            _cache.Clear();
            _service.Delete(bet);
        }

        public void Save()
        {
            _cache.Clear();
            _service.Save();
        }

        public void Dispose()
        {
            _service.Dispose();
        }
    }
}
