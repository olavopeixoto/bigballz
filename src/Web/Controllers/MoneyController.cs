using System.Web.Mvc;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    [Authorize(Roles = "admin")]
    public class MoneyController : BaseController
    {
        private readonly IBigBallzService _bigBallzService;
        //
        // GET: /Money/

        public MoneyController(IBigBallzService bigBallzService, IUserService userService, IMatchService matchService) : base(userService, matchService, bigBallzService)
        {
            _bigBallzService = bigBallzService;
        }

        public ActionResult Index()
        {
            var moneyDistribution = _bigBallzService.GetMoneyDistribution();

            return View(moneyDistribution);
        }

    }
}
