using System.ComponentModel.DataAnnotations;

namespace BigBallz
{
    public class Bonus
    {
        [Key]
        public int BonusId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(3)]
        public string Team { get; set; }

        public int? Group { get; set; }

        public virtual Group Group1 { get; set; }

        public virtual Team Team1 { get; set; }
    }
}