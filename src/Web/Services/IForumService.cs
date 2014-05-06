using System.Linq;
using BigBallz.Models;

namespace BigBallz.Services
{
    public interface IForumService
    {
        // Query Methods
        IQueryable<Forum> GetAll();
        Forum Get(int id);

        // Insert/Delete
        void Add(Forum forum);
        void Delete(Forum forum);
 
        // Persistence
        void Save();
    }
}
