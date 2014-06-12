using System;
using System.Linq;
using BigBallz.Models;

namespace BigBallz.Services
{
    public interface IRoleService : IDisposable
    {
        // Query Methods
        IQueryable<Role> GetAll();
        Role Get(string name);
        Role Get(int userId);

        //// Insert/Delete
        void Add(Role role);
        void Delete(Role role);
 
        //// Persistence
        void Save();
    }
}
