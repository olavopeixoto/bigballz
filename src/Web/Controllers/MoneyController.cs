using System.Web.Mvc;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    [Authorize(Roles = "admin")]
    public class MoneyController : Controller
    {
        private readonly IBigBallzService _bigBallzService;
        //
        // GET: /Money/

        public MoneyController(IBigBallzService bigBallzService)
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
