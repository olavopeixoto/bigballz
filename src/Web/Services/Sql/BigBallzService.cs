using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using BigBallz.Core;
using BigBallz.Models;
using Dapper;

namespace BigBallz.Services.Sql
{
    public class BigBallzService : IBigBallzService
    {
        private readonly DbConnection _connection;

        public BigBallzService(DbConnection connection)
        {
            _connection = connection;
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _connection.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IList<Match> GetUserPendingBets(string userName)
        {
            return _connection.QueryAsync<Match>(@"
SELECT b.*
FROM dbo.Bet b
JOIN dbo.[User] u ON b.[User] = u.UserId
JOIN dbo.Match m ON m.MatchId = b.Match
WHERE u.UserName = @startTime
AND m.StartTime <= @startTime",
                new { userName, startTime = DateTime.Now.BrazilTimeZone().AddHours(24) })
                .Result
                .ToList();
        }

        public IList<UserPoints> GetStandings()
        {
            var users = _db.Users.Where(x => x.Authorized).ToList();
            var userPointsList = users.Select(user => new UserPoints
            {
                User = user,
                TotalPoints = GetTotalUserPoints(user.UserName),
                TotalDayPoints = GetLastRoundPoints(user.UserName),
                TotalExactScore = GetTotalUserExactScores(user.UserName),
                TotalBonusPoints = GetTotalUserBonusPoints(user.UserName)
            })
            .OrderByDescending(x => x.TotalPoints)
            .ThenByDescending(x => x.TotalExactScore)
            .ThenByDescending(x => x.TotalBonusPoints)
            .ThenBy(x => x.User.UserName)
            .ToList();

            var j = 1;
            for (var i = 0; i < userPointsList.Count(); i++)
            {
                if (i > 0)
                {
                    var lastUser = userPointsList[i - 1];
                    var currentUser = userPointsList[i];
                    if (lastUser.TotalPoints > currentUser.TotalPoints
                        || (lastUser.TotalPoints == currentUser.TotalPoints && lastUser.TotalExactScore > currentUser.TotalExactScore)
                        || (lastUser.TotalPoints == currentUser.TotalPoints && lastUser.TotalExactScore == currentUser.TotalExactScore && lastUser.TotalBonusPoints > currentUser.TotalBonusPoints))
                    {
                        currentUser.Position = lastUser.Position + j;
                        j = 1;
                    }
                    else
                    {
                        currentUser.Position = lastUser.Position;
                        j++;
                    }
                }
                else
                {
                    userPointsList[i].Position = i + 1;
                }
            }

            return userPointsList;
        }

        public IList<UserPoints> GetLastRoundStandings()
        {
            throw new NotImplementedException();
        }

        public IList<BetPoints> GetUserPointsByMatch(string userName)
        {
            throw new NotImplementedException();
        }

        public IList<BonusPoints> GetUserPointsByBonus(string userName)
        {
            throw new NotImplementedException();
        }

        public IList<UserMatchPoints> GetUserPointsByExpiredMatch(int matchId)
        {
            throw new NotImplementedException();
        }

        public MatchBetStatistic GetMatchBetStatistics(int matchId)
        {
            throw new NotImplementedException();
        }

        public BonusBetStatistic GetBonusBetStatistics(int bonusId)
        {
            throw new NotImplementedException();
        }

        public Match GetFirstMatch()
        {
            throw new NotImplementedException();
        }

        public decimal GetTotalPrize()
        {
            throw new NotImplementedException();
        }

        public DateTime GetBonusBetExpireDate()
        {
            throw new NotImplementedException();
        }
    }
}