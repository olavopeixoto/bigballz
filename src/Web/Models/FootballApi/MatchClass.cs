namespace BigBallz.Models.FootballApi
{
    public class MatchClass
    {
        public int MatchId { get; set; }
        public string Period { get; set; } /* PreMatch, FullTime */
        public int Minute { get; set; }
        public int ScoreHome { get; set; }
        public int ScoreAway { get; set; }
        public TeamClass TeamHome { get; set; }
        public TeamClass TeamAway { get; set; }
    }
}