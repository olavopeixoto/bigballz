using System;
using System.Linq;
using BigBallz.Models;

namespace BigBallz.Services
{
    public interface ITeamService : IDisposable
    {
        // Query Methods
        IQueryable<Team> GetAll();
        IQueryable<Team> GetAll(int groupId);
        Team Get(string id);

        // Insert/Delete
        void Add(Team team);
        void Delete(Team team);
 
        // Persistence
        void Save();
    }
}
