namespace BigBallz.Models
{
    public class MatchBetStatistic
    {
        public Match Match { get; set; }
        public double Team1Perc { get; set; }
        public double Team2Perc { get; set; }
        public double TiePerc { get; set; }
        public int Score1MostBet { get; set; }
        public int Score2MostBet { get; set; }
        public double AverageScore1 { get; set; }
        public double AverageScore2 { get; set; }
    }
}