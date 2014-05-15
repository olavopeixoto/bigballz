using System.Linq;
using BigBallz.Models;
using System.Collections.Generic;

namespace BigBallz.Services
{
    public interface IMatchBetService
    {
        // Query Methods
        IEnumerable<Bet> GetAll();
        IEnumerable<Bet> GetAll(string userName);
        IEnumerable<Bet> GetAll(int userId);
        IEnumerable<Bet> GetAllExpired(int userId);
        Bet Get(int betId);

        // Insert/Delete
        void Add(Bet bet);
        void Add(IList<Bet> bets);
        void Delete(Bet bet);
 
        // Persistence
        void Save();
    }
}
