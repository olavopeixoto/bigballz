using System;
using System.Collections.Generic;
using BigBallz.Core.Caching;
using BigBallz.Models;

namespace BigBallz.Services.Cache
{
    public class BigBallsServiceCache : IBigBallzService
    {
        private readonly ICache _cache;
        private readonly IBigBallzService _service;

        public BigBallsServiceCache(ICache cache, IBigBallzService service)
        {
            _cache = cache;
            _service = service;
        }

        public IList<Match> GetUserPendingBets(string userName)
        {
            var key = string.Format("BB-GetUserPendingBets-{0}", userName ?? string.Empty);

            if (_cache.Contains(key))
            {
                return _cache.Get<IList<Match>>(key);
            }

            var results = _service.GetUserPendingBets(userName);
            _cache.Set(key, results);

            return results;
        }

        public IList<UserPoints> GetStandings()
        {
            var key = "BB-GetStandings";

            if (_cache.Contains(key))
            {
                return _cache.Get<IList<UserPoints>>(key);
            }

            var results = _service.GetStandings();
            _cache.Set(key, results);

            return results;
        }

        public IList<UserPoints> GetLastRoundStandings()
        {
            var key = "BB-GetLastRoundStandings";

            if (_cache.Contains(key))
            {
                return _cache.Get<IList<UserPoints>>(key);
            }

            var results = _service.GetLastRoundStandings();
            _cache.Set(key, results);

            return results;
        }

        public IList<BetPoints> GetUserPointsByMatch(User user)
        {
            var key = string.Format("BB-GetUserPointsByMatch-{0}", user.UserId);

            if (_cache.Contains(key))
            {
                return _cache.Get<IList<BetPoints>>(key);
            }

            var results = _service.GetUserPointsByMatch(user);
            _cache.Set(key, results);

            return results;
        }

        public IList<BonusPoints> GetUserPointsByBonus(User user)
        {
            var key = string.Format("BB-GetUserPointsByBonus-{0}", user.UserId);

            if (_cache.Contains(key))
            {
                return _cache.Get<IList<BonusPoints>>(key);
            }

            var results = _service.GetUserPointsByBonus(user);
            _cache.Set(key, results);

            return results;
        }

        public IList<UserMatchPoints> GetUserPointsByExpiredMatch(int matchId)
        {
            var key = string.Format("BB-GetUserPointsByExpiredMatch-{0}", matchId);

            if (_cache.Contains(key))
            {
                return _cache.Get<IList<UserMatchPoints>>(key);
            }

            var results = _service.GetUserPointsByExpiredMatch(matchId);
            _cache.Set(key, results);

            return results;
        }

        public MatchBetStatistic GetMatchBetStatistics(int matchId)
        {
            var key = string.Format("BB-GetMatchBetStatistics-{0}", matchId);

            if (_cache.Contains(key))
            {
                return _cache.Get<MatchBetStatistic>(key);
            }

            var results = _service.GetMatchBetStatistics(matchId);

            if (results!=null) _cache.Set(key, results);

            return results;
        }

        public BonusBetStatistic GetBonusBetStatistics(int bonusId)
        {
            var key = string.Format("BB-GetBonusBetStatistics-{0}", bonusId);

            if (_cache.Contains(key))
            {
                return _cache.Get<BonusBetStatistic>(key);
            }

            var results = _service.GetBonusBetStatistics(bonusId);
            if (results != null) _cache.Set(key, results);

            return results;
        }

        public Match GetFirstMatch()
        {
            var key = "BB-GetFirstMatch";

            if (_cache.Contains(key))
            {
                return _cache.Get<Match>(key);
            }

            var results = _service.GetFirstMatch();
            if (results != null) _cache.Set(key, results);

            return results;
        }

        public Prizes GetPrizes()
        {
            var key = "BB-GetPrizes";

            if (_cache.Contains(key))
            {
                return _cache.Get<Prizes>(key);
            }

            var results = _service.GetPrizes();
            _cache.Set(key, results);

            return results;
        }

        public DateTime GetBonusBetExpireDate()
        {
            var key = "BB-GetBonusBetExpireDate";

            if (_cache.Contains(key))
            {
                return _cache.Get<DateTime>(key);
            }

            var results = _service.GetBonusBetExpireDate();
            _cache.Set(key, results);

            return results;
        }

        public void Dispose()
        {
            _service.Dispose();
        }
    }
}