using System.Web.Mvc;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    public class StandingsController : BaseController
    {
        private readonly IBigBallzService _bigBallzService = new BigBallzService();

        public ActionResult Index()
        {
            var standings = _bigBallzService.GetStandings();
            return View(standings);
        }
    }
}