using System;
using System.Collections.Generic;
using System.Linq;
using BigBallz.Core.Caching;
using BigBallz.Models;

namespace BigBallz.Services.Cache
{
    public class MatchServiceCache : IMatchService
    {
        private readonly ICache _cache;
        private readonly IMatchService _service;

        public MatchServiceCache(ICache cache, IMatchService service)
        {
            _cache = cache;
            _service = service;
        }

        public IEnumerable<Match> GetAll()
        {
            var key = string.Format("BB-MatchService-GetAll");

            if (_cache.Contains(key))
            {
                return _cache.Get<IEnumerable<Match>>(key);
            }

            var results = _service.GetAll().ToList();
            _cache.Set(key, results);

            return results;
        }

        public Match Get(int id)
        {
            var key = string.Format("BB-MatchService-Get-{0}", id);

            if (_cache.Contains(key))
            {
                return _cache.Get<Match>(key);
            }

            var results = _service.Get(id);
            _cache.Set(key, results);

            return results;
        }

        public IEnumerable<Match> GetNextMatches()
        {
            var key = string.Format("BB-MatchService-GetNextMatches");

            if (_cache.Contains(key))
            {
                return _cache.Get<IEnumerable<Match>>(key);
            }

            var results = _service.GetNextMatches().ToList();
            _cache.Set(key, results);

            return results;
        }

        public IEnumerable<Match> GetLastPlayedMatches()
        {
            var key = string.Format("BB-MatchService-GetLastPlayedMatches");

            if (_cache.Contains(key))
            {
                return _cache.Get<IEnumerable<Match>>(key);
            }

            var results = _service.GetLastPlayedMatches().ToList();
            _cache.Set(key, results);

            return results;
        }

        public void Add(Match match)
        {
            _cache.Clear();
            _service.Add(match);
        }

        public void Delete(Match match)
        {
            _cache.Clear();
            _service.Delete(match);
        }

        public void Save()
        {
            _cache.Clear();
            _service.Save();
        }

        public DateTime GetStartDate()
        {
            var key = string.Format("BB-MatchService-GetStartDate");

            if (_cache.Contains(key))
            {
                return _cache.Get<DateTime>(key);
            }

            var results = _service.GetStartDate();
            _cache.Set(key, results);

            return results;
        }
    }
}