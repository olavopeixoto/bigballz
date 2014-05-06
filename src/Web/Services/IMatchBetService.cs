using System.Linq;
using BigBallz.Models;
using System.Collections.Generic;

namespace BigBallz.Services
{
    public interface IMatchBetService
    {
        // Query Methods
        IQueryable<Bet> GetAll();
        IQueryable<Bet> GetAll(string userName);
        IQueryable<Bet> GetAll(int userId);
        IQueryable<Bet> GetAllExpired(int userId);
        Bet Get(int betId);

        // Insert/Delete
        void Add(Bet bet);
        void Add(IList<Bet> bets);
        void Delete(Bet bet);
 
        // Persistence
        void Save();
    }
}
