using System;
using System.Web;
using System.Web.Mvc;
using BigBallz.Core;
using BigBallz.Helpers;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMatchService _matchService;
        private readonly IUserService _userService;
        private readonly IBigBallzService _bigBallzService;

        public HomeController(IMatchService matchService, IUserService userService, IBigBallzService bigBallzService)
        {
            _matchService = matchService;
            _userService = userService;
            _bigBallzService = bigBallzService;
        }

        [Authorize, AllowAnonymous]
        public ActionResult Index()
        {
            ViewData["StartDate"] = _matchService.GetStartDate();
            return View();
        }

        [Authorize, AllowAnonymous]
        public ActionResult Rules()
        {
            ViewData["StartDate"] = _matchService.GetStartDate().FormatDate();
            return View();
        }

        [Authorize, AllowAnonymous]
        public ActionResult Standings(DateTime date, int? user = null)
        {
            ViewData["StartDate"] = _matchService.GetStartDate();
            ViewData["RequestDate"] = date;
            ViewData["User"] = user;

            var standings = _bigBallzService.GetStandings(date);

            return View(standings);
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);

            if (User.Identity.IsAuthenticated)
            {
                var result = filterContext.Result;

                if (result is ViewResultBase
                    && filterContext.HttpContext.User.Identity.IsAuthenticated
                    //&& !ControllerContext.HttpContext.IsDebuggingEnabled
                )
                {
                    var userName = filterContext.HttpContext.User.Identity.Name;
                    var user = _userService.Get(userName);

                    var photoCookie = Request.Cookies["photoUrl"];
                    if (photoCookie == null || user.PhotoUrl != photoCookie.Value)
                    {
                        string photoUrl;
                        if (string.IsNullOrEmpty(user.PhotoUrl))
                        {
                            var gravatar = new GravatarHelper
                            {
                                Email = user.EmailAddress
                            };
                            photoUrl = gravatar.GetGravatarUrl();
                        }
                        else
                        {
                            photoUrl = user.PhotoUrl;
                        }

                        Response.SetCookie(new HttpCookie("photoUrl", photoUrl)
                        {
                            Expires = DateTime.UtcNow.AddDays(30)
                        });
                    }

                    ViewData["UserId"] = user.UserId;
                    ViewData["UserName"] = user.UserName;
                    ViewData["UserEmail"] = user.EmailAddress;
                }
            }
        }
    }
}