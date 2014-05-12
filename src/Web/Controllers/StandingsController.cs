using System.Web.Mvc;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    public class StandingsController : BaseController
    {
        private readonly IBigBallzService _bigBallzService;

        public StandingsController(IUserService userService, IMatchService matchService, IBigBallzService bigBallzService) : base(userService, matchService, bigBallzService)
        {
            _bigBallzService = bigBallzService;
        }

        public ActionResult Index()
        {
            var standings = _bigBallzService.GetStandings();
            return View(standings);
        }
    }
}