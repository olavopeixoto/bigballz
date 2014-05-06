using System.Linq;
using BigBallz.Models;

namespace BigBallz.Services
{
    public interface IUserService
    {
        // Query Methods
        IQueryable<User> GetAll();
        User Get(string userName);
        User Get(int userId);

        //// Insert/Delete
        //void Add(Group group);
        //void Delete(Group group);
 
        //// Persistence
        void Save();
    }
}
