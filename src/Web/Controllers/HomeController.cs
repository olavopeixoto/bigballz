using System.Web.Mvc;
using BigBallz.Core;
using BigBallz.Services;
using BigBallz.Services.L2S;

namespace BigBallz.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IMatchService _matchService;

        public HomeController()
        {
            _matchService = new MatchService();
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

        //public ActionResult Poll()
        //{
        //    return View();
        //}
    }
}
