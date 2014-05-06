using System.Linq;
using BigBallz.Models;

namespace BigBallz.Services
{
    public interface IGroupService
    {
        // Query Methods
        IQueryable<Group> GetAll();
        Group Get(int id);

        // Insert/Delete
        void Add(Group group);
        void Delete(Group group);
 
        // Persistence
        void Save();
    }
}
