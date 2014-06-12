using System;
using System.Linq;
using BigBallz.Models;

namespace BigBallz.Services
{
    public interface IStageService : IDisposable
    {
        // Query Methods
        IQueryable<Stage> GetAll();
        Stage Get(int id);

        // Insert/Delete
        void Add(Stage stage);
        void Delete(Stage stage);
 
        // Persistence
        void Save();
    }
}
