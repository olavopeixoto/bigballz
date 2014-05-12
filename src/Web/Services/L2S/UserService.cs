using System.Linq;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class UserService : IUserService
    {
        readonly BigBallzDataContext _db;

        public UserService(BigBallzDataContext context)
        {
            _db = context;
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
            _db.SubmitChanges();
        }
    }
}