using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigBallz
{
    [Table("UserMapping")]
    public class UserMapping
    {
        public int UserMappingId { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(200)]
        public string Identifier { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string ProviderName { get; set; }

        public virtual User User { get; set; }
    }
}