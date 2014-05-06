using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BigBallz.Models;

namespace BigBallz.ViewModels
{
    public class BetViewModel
    {
        public string UserName { get; set; }
        public bool BonusEnabled { get; set; }
        public IList<BonusTeams> BonusList { get; set; }
        public IList<BetMatches> BetList { get; set; }

        public class BetMatches
        {
            public bool Enabled { get; set; }
            public Match Match { get; set; }
            public Bet Bet { get; set; }
            public int PointsEarned { get; set; }
        }

        public class BonusTeams
        {
            public int? BonusBetId { get; set; }
            public Bonus Bonus { get; set; }
            public BonusBet BonusBet { get; set; }
            public SelectList Teams { get; set; }
            public bool Enabled { get; set; }
            public int PointsEarned { get; set; }
            public DateTime CupStartDate { get; set; }
            public BonusBetStatistic BonusBetStatistic { get; set; }
            

        }

       
    }
}