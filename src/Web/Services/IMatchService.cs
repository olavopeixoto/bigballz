using System;
using System.Collections.Generic;
using System.Linq;
using BigBallz.Models;

namespace BigBallz.Services
{
    public interface IMatchService
    {
        // Query Methods
        IQueryable<Match> GetAll();
        Match Get(int id);

        IEnumerable<Match> GetNextMatches();

        IEnumerable<Match> GetLastPlayedMatches();

        // Insert/Delete
        void Add(Match match);
        void Delete(Match match);

        // Persistence
        void Save();
        DateTime GetStartDate();
    }
}
