using System.Linq;
using System.Web.Mvc;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    public class MoneyController : BaseController
    {
        private readonly IBigBallzService _bigBallzService;
        private readonly IUserService _userService;
        //
        // GET: /Money/

        public MoneyController(IBigBallzService bigBallzService, IUserService userService, IMatchService matchService) : base(userService, matchService, bigBallzService)
        {
            _bigBallzService = bigBallzService;
            _userService = userService;
        }

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var moneyDistribution = _bigBallzService.GetMoneyDistribution();

            return View(moneyDistribution);
        }

        public ActionResult Details()
        {
            var users = _userService.GetAll().Where(x => x.Authorized).ToList();

            return View(users);
        }

    }
}
