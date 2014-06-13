using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigBallz
{
    [Table("BonusBet")]
    public class BonusBet
    {
        public int BonusBetId { get; set; }

        public int User { get; set; }

        public int Bonus { get; set; }

        [Required]
        [StringLength(3)]
        public string Team { get; set; }

        public virtual Team Team1 { get; set; }

        public virtual User User1 { get; set; }

        public virtual Bonus Bonus1 { get; set; }
    }
}