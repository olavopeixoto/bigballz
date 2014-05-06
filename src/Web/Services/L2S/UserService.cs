using System;
using System.Linq;
using BigBallz.Services;

namespace BigBallz.Models
{
    public class UserService : IUserService
    {
        readonly BigBallzDataContext db = new BigBallzDataContext();

        public IQueryable<User> GetAll()
        {
            return db.Users.OrderBy(x => x.UserName);
        }

        public User Get(string userName)
        {
            return db.Users.SingleOrDefault(d => d.UserName == userName);
        }

        public User Get(int userId)
        {
            return db.Users.SingleOrDefault(d => d.UserId == userId);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}