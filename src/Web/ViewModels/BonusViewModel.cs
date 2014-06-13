using System.Web.Mvc;

namespace BigBallz.ViewModels
{
    public class BonusViewModel
    {
        // Properties
        public Bonus Bonus { get; set; }
        public SelectList Teams { get; set; }
    }
}