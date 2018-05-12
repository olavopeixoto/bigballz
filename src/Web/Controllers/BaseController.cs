using System;
using System.Web;
using System.Web.Mvc;
using BigBallz.Helpers;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMatchService _matchService;
        private readonly IBigBallzService _bigBallzService;

        protected BaseController(IUserService userService, IMatchService matchService, IBigBallzService bigBallzService)
        {
            _userService = userService;
            _matchService = matchService;
            _bigBallzService = bigBallzService;
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);

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

                ViewData["NextMatches"] = _matchService.GetNextMatches();
                ViewData["LastMatches"] = _matchService.GetLastPlayedMatches();
                
                ViewData["Prizes"] = _bigBallzService.GetPrizes();

                ViewData["Standings"] = _bigBallzService.GetStandings();
                ViewData["DayStandings"] = _bigBallzService.GetLastRoundStandings();

                ViewData["PendingBets"] = _bigBallzService.GetUserPendingBets(userName);
            }
        }
    }
}