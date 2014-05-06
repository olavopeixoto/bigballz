using System.Linq;
using BigBallz.Models;

namespace BigBallz.Services
{
    public interface IBonusService
    {
        // Query Methods
        IQueryable<Bonus> GetAll();
        Bonus Get(int id);

        // Insert/Delete
        void Add(Bonus Bonus);
        void Delete(Bonus Bonus);
 
        // Persistence
        void Save();
    }
}
