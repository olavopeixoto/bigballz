using System;
using System.Linq;
using BigBallz.Models;
using System.Collections.Generic;

namespace BigBallz.Services
{
    public interface IBonusBetService : IDisposable
    {
        // Query Methods
        IEnumerable<BonusBet> GetAll();
        IEnumerable<BonusBet> GetAll(string userName);
        IEnumerable<BonusBet> GetAll(int userId);
        BonusBet Get(int bonusBetID);

        // Insert/Delete
        void Add(BonusBet bonusBet);
        void Add(IList<BonusBet> bonusBetList); 
        void Delete(BonusBet bonusBetBet);
 
        // Persistence
        void Save();
    }
}
