using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using BigBallz.Core;
using BigBallz.Models;
using Dapper;

namespace BigBallz.Services.Sql
{
    public class MatchService : IMatchService
    {
        private readonly DbConnection _connection;

        public MatchService(DbConnection connection)
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

        public IEnumerable<Match> GetAll()
        {
            return _connection.QueryAsync<Match>(@"
SELECT *
FROM dbo.Match").Result;
        }

        public Match Get(int id)
        {
            return _connection.QueryAsync<Match>(@"
SELECT *
FROM dbo.Match
WHERE MatchId = @Id", new { id }).Result.FirstOrDefault();
        }

        public IEnumerable<Match> GetNextMatches()
        {
            return _connection.QueryAsync<Match>(@"
SELECT TOP 5 *
FROM dbo.Match
WHERE StartTime > @startTime
ORDER BY StartTime", new { startTime = DateTime.Now.BrazilTimeZone() }).Result;
        }

        public IEnumerable<Match> GetLastPlayedMatches()
        {
            //db.Matches.Where(x => x.StartTime < DateTime.Now.BrazilTimeZone())
            //            .OrderByDescending(x => x.StartTime)
            //            .Take(5).ToList();

            return _connection.QueryAsync<Match>(@"
SELECT TOP 5 *
FROM dbo.Match
WHERE StartTime < @startTime
ORDER BY StartTime DESC", new { startTime = DateTime.Now.BrazilTimeZone() }).Result;
        }

        public void Add(Match match)
        {
            throw new NotImplementedException();
        }

        public void Delete(Match match)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public DateTime GetStartDate()
        {
            return _connection.QueryAsync<DateTime?>(@"
SELECT Min(StartTime) StartTime 
FROM dbo.Match", new { startTime = DateTime.Now.BrazilTimeZone() }).Result.FirstOrDefault() ?? DateTime.Now.BrazilTimeZone();
        }
    }
}