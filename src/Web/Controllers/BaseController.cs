﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BigBallz.Core;
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
                && filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var userName = filterContext.HttpContext.User.Identity.Name;

                var photoCookie = Request.Cookies["photoUrl"];
                if (photoCookie == null || string.IsNullOrEmpty(photoCookie.Value))
                {
                    var user = _userService.Get(userName);
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
                    Response.Cookies.Add(new HttpCookie("photoUrl", photoUrl)
                    {
                        Expires = DateTime.Now.BrazilTimeZone().AddDays(30)
                    });
                }

                ViewData["NextMatches"] = _matchService.GetNextMatches();
                ViewData["LastMatches"] = _matchService.GetLastPlayedMatches();

                var totalprize = _bigBallzService.GetTotalPrize();
                var prizes = new List<double> {totalprize*0.65, totalprize*0.20, totalprize*0.10};
                ViewData["Prizes"] = prizes;

                ViewData["Standings"] = _bigBallzService.GetStandings();
                ViewData["DayStandings"] = _bigBallzService.GetLastRoundStandings();

                ViewData["PendingBets"] = _bigBallzService.GetUserPendingBets(userName);
            }
        }
    }
}