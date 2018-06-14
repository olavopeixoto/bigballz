using System;

namespace BigBallz.Models.FootballApi
{
    public class MatchDays
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public DateTime KickoffFirst { get; set; }
        public DateTime KickoffLast { get; set; }
        public DateTime[] Kickoffs { get; set; }
        public bool IsCurrentMatchday { get; set; }
        public bool HasMatchdayFeed { get; set; }
    }
}