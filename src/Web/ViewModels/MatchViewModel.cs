using System.Web.Mvc;
using BigBallz.Models;

namespace BigBallz.ViewModels
{
    public class MatchViewModel
    {
        // Properties
        public Match Match { get; set; }
        public SelectList Teams { get; set; }
        public SelectList Stages { get; set; }
        public bool Enabled { get; set; }
    }
}