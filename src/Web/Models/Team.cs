using System.ComponentModel.DataAnnotations;

namespace BigBallz.Models
{
    [MetadataType(typeof(TeamMD))]
    public partial class Team
    {
        public class TeamMD
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string TeamId { get; set; }

            [Required]
            public int FifaId { get; set; }

            [Required]
            public Group Group { get; set; }
        }
    }
}