using System.ComponentModel.DataAnnotations.Schema;

namespace BigBallz
{
    [Table("Bet")]
    public class Bet
    {
        public int BetId { get; set; }

        public int User { get; set; }

        public int Match { get; set; }

        public int Score1 { get; set; }

        public int Score2 { get; set; }

        public virtual Match Match1 { get; set; }

        public virtual User User1 { get; set; }
    }
}