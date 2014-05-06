using System.Web.Mvc;
using BigBallz.Core;
using BigBallz.Services;
using BigBallz.Services.L2S;

namespace BigBallz.Controllers
{
    [HandleError]
    public class HomeController : BaseController
    {
        private readonly IMatchService _matchService;

        public HomeController()
        {
            _matchService = new MatchService();
        }

        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";
            
            return this.AjaxView();
        }
        
        public ActionResult Rules()
        {
            ViewData["StartDate"] = _matchService.GetStartDate().FormatDate();
            return View();
        }

        public ActionResult Poll()
        {
            return View();
        }
    }
}
