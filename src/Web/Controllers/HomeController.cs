using System.Web.Mvc;
using BigBallz.Core;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMatchService _matchService;

        public HomeController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [Authorize, AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize, AllowAnonymous]
        public ActionResult Rules()
        {
            ViewData["StartDate"] = _matchService.GetStartDate().FormatDate();
            return View();
        }
    }
}