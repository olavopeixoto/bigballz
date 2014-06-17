using System.Web.Mvc;
using BigBallz.Models;

namespace BigBallz.ViewModels
{
    public class BonusViewModel
    {
        // Properties
        public Bonus Bonus { get; set; }
        public SelectList Teams { get; set; }
    }
}