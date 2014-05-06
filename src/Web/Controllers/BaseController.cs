using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BigBallz.Core;
using BigBallz.Helpers;
using BigBallz.Models;
using BigBallz.Services;
using BigBallz.Services.L2S;

namespace BigBallz.Controllers
{
    public abstract class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //&& filterContext.HttpContext.User.IsInRole(BBRoles.Player)

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var userName = filterContext.HttpContext.User.Identity.Name;

                var photoCookie = Request.Cookies["photoUrl"];
                if (photoCookie == null || string.IsNullOrEmpty(photoCookie.Value))
                {
                    IUserService userService = new UserService();
                    var user = userService.Get(userName);
                    Response.Cookies.Remove("photoUrl");
                    string photoUrl;
                    if (string.IsNullOrEmpty(user.PhotoUrl))
                    {
                        var gravatar = new GravatarHelper
                                           {
                                               email = user.EmailAddress
                                           };
                        photoUrl = gravatar.GetGravatarUrl();
                    }
                    else
                    {
                        photoUrl = user.PhotoUrl;
                    }
                    Response.Cookies.Add(new HttpCookie("photoUrl", photoUrl) { Expires = DateTime.Now.BrazilTimeZone().AddDays(30) });
                }

                IMatchService matchService = new MatchService();
                ViewData["NextMatches"] = matchService.GetNextMatches();

                ViewData["LastMatches"] = matchService.GetLastPlayedMatches();

                var bigballzService = new BigBallzService();
                var totalprize = bigballzService.GetTotalPrize();
                var prizes = new List<double> { totalprize * 0.65, totalprize * 0.20, totalprize * 0.10 };
                ViewData["Prizes"] = prizes;
                
                ViewData["Standings"] = bigballzService.GetStandings();
                ViewData["DayStandings"] = bigballzService.GetLastRoundStandings();

                ViewData["PendingBets"] = bigballzService.GetUserPendingBets(userName);
            }
        }
    }
}