using System.Linq;
using BigBallz.Services;

namespace BigBallz.Models
{
    public class UserService : IUserService
    {
        readonly BigBallzDataContext _db = new BigBallzDataContext();

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