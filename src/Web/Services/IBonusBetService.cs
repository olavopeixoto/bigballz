﻿using System.Linq;
using BigBallz.Models;
using System.Collections.Generic;

namespace BigBallz.Services
{
    public interface IBonusBetService
    {
        // Query Methods
        IQueryable<BonusBet> GetAll();
        IQueryable<BonusBet> GetAll(string userName);
        IQueryable<BonusBet> GetAll(int userId);
        BonusBet Get(int bonusBetID);

        // Insert/Delete
        void Add(BonusBet bonusBet);
        void Add(IList<BonusBet> bonusBetList); 
        void Delete(BonusBet BonusBetBet);
 
        // Persistence
        void Save();
    }
}
