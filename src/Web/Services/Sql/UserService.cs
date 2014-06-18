using System;
using System.Data.Common;
using System.Linq;
using BigBallz.Models;
using Dapper;

namespace BigBallz.Services.Sql
{
    public class UserService : IUserService
    {
        private readonly DbConnection _connection;

        public UserService(DbConnection connection)
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

        public IQueryable<User> GetAll()
        {
            return _connection.QueryAsync<User>(@"
SELECT *
FROM dbo.[User] u
ORDER BY Nome"
                ).Result.AsQueryable();
        }

        public User Get(string userName)
        {
            return _connection.QueryAsync<User>(@"
SELECT *
FROM dbo.[User] u
WHERE u.UserName = @userName ", new { userName }
                ).Result.FirstOrDefault();
        }

        public User Get(int userId)
        {
            return _connection.QueryAsync<User>(@"
SELECT *
FROM dbo.[User] u
WHERE u.UserId = @userId ", new { userId }
                ).Result.FirstOrDefault();
        }

        public void Save()
        {
        }
    }
}