using System;
using System.Linq;
using BigBallz.Models;

namespace BigBallz.Services
{
    public interface IBonusService : IDisposable
    {
        // Query Methods
        IQueryable<Bonus> GetAll();
        Bonus Get(int id);

        // Insert/Delete
        void Add(Bonus bonus);
        void Delete(Bonus bonus);
 
        // Persistence
        void Save();
    }
}
