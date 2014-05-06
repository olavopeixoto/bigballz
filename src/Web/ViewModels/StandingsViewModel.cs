using System.Web.Mvc;
using BigBallz.Models;

namespace BigBallz.ViewModels
{
    public class StandingsViewModel
    {
        // Properties
        public User User { get; set; }
        public int Points { get; set; }
        public int Scores { get; set; }
    }
}