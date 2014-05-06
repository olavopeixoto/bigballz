using System.Web.Mvc;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CronJobsController : SecureBaseController
    {
        public ActionResult Index()
        {
            var tasks = CronJob.GetScheduledTasks();
            return View(tasks);
        }
    }
}