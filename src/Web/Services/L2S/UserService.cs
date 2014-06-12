using System.Linq;
using BigBallz.Core.Caching;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class UserService : IUserService
    {
        readonly BigBallzDataContext _db;
        private readonly ICache _cache;

        public UserService(BigBallzDataContext context, ICache cache)
        {
            _db = context;
            _cache = cache;
        }

        public IQueryable<User> GetAll()
        {
            return _db.Users.OrderBy(x => x.UserName);
        }

        public User Get(string userName)
        {
            return _db.Users.SingleOrDefault(d => d.UserName == userName);
        }

        public User Get(int userId)
        {
            return _db.Users.SingleOrDefault(d => d.UserId == userId);
        }

        public void Save()
        {
            _cache.Clear();
            _db.SubmitChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}