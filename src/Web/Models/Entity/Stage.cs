using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigBallz
{
    [Table("Stage")]
    public class Stage
    {
        public Stage()
        {
            Matches = new HashSet<Match>();
        }

        public int StageId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Match> Matches { get; set; }
    }
}