using System.Web.Mvc;
using BigBallz.Core;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IMatchService _matchService;

        public HomeController(IMatchService matchService, IUserService userService, IBigBallzService bigBallzService) : base(userService, matchService, bigBallzService)
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

        //public ActionResult Poll()
        //{
        //    return View();
        //}
    }
}
