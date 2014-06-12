using System.Collections.Generic;
using System.Linq;
using BigBallz.Core.Caching;
using BigBallz.Models;

namespace BigBallz.Services.Cache
{
    public class BonusBetServiceCache : IBonusBetService
    {
        private readonly ICache _cache;
        private readonly IBonusBetService _service;

        public BonusBetServiceCache(ICache cache, IBonusBetService service)
        {
            _cache = cache;
            _service = service;
        }

        public IEnumerable<BonusBet> GetAll()
        {
            var key = string.Format("BB-BonusBetServiceCache-GetAll");

            if (_cache.Contains(key))
            {
                return _cache.Get<IEnumerable<BonusBet>>(key);
            }

            var results = _service.GetAll().ToList();
            _cache.Set(key, results);

            return results;
        }

        public IEnumerable<BonusBet> GetAll(string userName)
        {
            var key = string.Format("BB-BonusBetServiceCache-GetAll-{0}", userName ?? string.Empty);

            if (_cache.Contains(key))
            {
                return _cache.Get<IEnumerable<BonusBet>>(key);
            }

            var results = _service.GetAll(userName).ToList();
            _cache.Set(key, results);

            return results;
        }

        public IEnumerable<BonusBet> GetAll(int userId)
        {
            var key = string.Format("BB-BonusBetServiceCache-GetAll-{0}", userId);

            if (_cache.Contains(key))
            {
                return _cache.Get<IEnumerable<BonusBet>>(key);
            }

            var results = _service.GetAll(userId).ToList();
            _cache.Set(key, results);

            return results;
        }

        public BonusBet Get(int bonusBetId)
        {
            var key = string.Format("BB-BonusBetServiceCache-Get-{0}", bonusBetId);

            if (_cache.Contains(key))
            {
                return _cache.Get<BonusBet>(key);
            }

            var results = _service.Get(bonusBetId);
            _cache.Set(key, results);

            return results;
        }

        public void Add(BonusBet bonusBet)
        {
            _cache.Clear();
            _service.Add(bonusBet);
        }

        public void Add(IList<BonusBet> bonusBetList)
        {
            _cache.Clear();
            _service.Add(bonusBetList);
        }

        public void Delete(BonusBet bonusBetBet)
        {
            _cache.Clear();
           _service.Delete(bonusBetBet);
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