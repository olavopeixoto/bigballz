using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigBallz
{
    [Table("Team")]
    public class Team
    {
        public Team()
        {
            Bonus = new HashSet<Bonus>();
            BonusBets = new HashSet<BonusBet>();
            Matches = new HashSet<Match>();
            Matches1 = new HashSet<Match>();
        }

        [StringLength(3)]
        public string TeamId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int? FifaId { get; set; }

        public int GroupId { get; set; }

        public virtual ICollection<Bonus> Bonus { get; set; }

        public virtual ICollection<BonusBet> BonusBets { get; set; }

        public virtual Group Group1 { get; set; }

        public virtual ICollection<Match> Matches { get; set; }

        public virtual ICollection<Match> Matches1 { get; set; }
    }
}