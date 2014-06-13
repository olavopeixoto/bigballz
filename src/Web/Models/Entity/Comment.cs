using System;
using System.ComponentModel.DataAnnotations;

namespace BigBallz
{
    public class Comment
    {
        public int Id { get; set; }

        public int User { get; set; }

        [Required]
        public string Comments { get; set; }

        public DateTime CommentedOn { get; set; }

        public virtual User User1 { get; set; }
    }
}