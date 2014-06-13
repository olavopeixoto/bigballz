using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigBallz
{
    [Table("Match")]
    public class Match
    {
        public Match()
        {
            Bets = new HashSet<Bet>();
        }

        public int MatchId { get; set; }

        public int StageId { get; set; }

        public DateTime StartTime { get; set; }

        [StringLength(3)]
        public string Team1Id { get; set; }

        [StringLength(3)]
        public string Team2Id { get; set; }

        public int? Score1 { get; set; }

        public int? Score2 { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }

        public virtual Stage Stage1 { get; set; }

        public virtual Team Team1 { get; set; }

        public virtual Team Team2 { get; set; }
    }
}