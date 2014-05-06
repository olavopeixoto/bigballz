using System.Web.Mvc;
using BigBallz.Models;

namespace BigBallz.ViewModels
{
    public class TeamViewModel
    {
        // Properties
        public Team Team { get; set; }
        public SelectList Groups { get; set; }
    }
}