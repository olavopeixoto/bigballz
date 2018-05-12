using System.Linq;
using System.Web.Mvc;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    public class MoneyController : BaseController
    {
        private readonly IUserService _userService;
        //
        // GET: /Money/

        public MoneyController(IBigBallzService bigBallzService, IUserService userService, IMatchService matchService) : base(userService, matchService, bigBallzService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            var users = _userService.GetAll()
                                    .Where(x => x.Authorized)
                                    .ToList();

            return View(users);
        }

    }
}
