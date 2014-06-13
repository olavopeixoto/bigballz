using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigBallz
{
    [Table("Group")]
    public class Group
    {
        public Group()
        {
            Bonus = new HashSet<Bonus>();
            Teams = new HashSet<Team>();
        }

        public int GroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Bonus> Bonus { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}