namespace BigBallz.Models
{
    public class UserPoints
    {
        public int Position { get; set; }
        public User User { get; set; }
        public int TotalPoints { get; set; }
        public int TotalExactScore { get; set; }
        public int TotalDayPoints { get; set; }
        public int TotalBonusPoints { get; set; }
    }
}