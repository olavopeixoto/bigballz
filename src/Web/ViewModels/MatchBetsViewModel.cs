using System.Collections.Generic;
using BigBallz.Models;

namespace BigBallz.ViewModels
{
    public class MatchBetsViewModel
    {
        // Properties
        public Match Match { get; set; }
        public MatchBetStatistic Statistics { get; set; }
        public IList<UserMatchPoints> UsersMatchPoints { get; set; }
    }
}