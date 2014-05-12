﻿using System.Web.Mvc;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CronJobsController : BaseController
    {
        public CronJobsController(IUserService userService, IMatchService matchService, IBigBallzService bigBallzService) : base(userService, matchService, bigBallzService)
        {}

        public ActionResult Index()
        {
            var tasks = CronJob.GetScheduledTasks();
            return View(tasks);
        }
    }
}