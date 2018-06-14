using System;

namespace BigBallz.Models.FootballApi
{
    public class KickoffClass
    {
        public DateTime Kickoff { get; set; }
        public GroupClass[] Groups { get; set; }
    }
}