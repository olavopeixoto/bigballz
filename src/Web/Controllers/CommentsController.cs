using System.Web.Mvc;
using BigBallz.Filters;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    public class CommentsController : BaseController
    {
        private readonly ICommentsService _commentsService;

        public CommentsController(ICommentsService commentsService, IBigBallzService bigBallzService, IMatchService matchService, IUserService userService) : base(userService, matchService, bigBallzService)
        {
            _commentsService = commentsService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var comments = _commentsService.GetComments();
            return View(comments);
        }

        [HttpPost, UserNameFilter]
        public ActionResult Post(string userName, string comment)
        {
            _commentsService.PostComment(userName, comment);

            return RedirectToAction("index");
        }
    }
}
