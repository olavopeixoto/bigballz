using System.Web.Mvc;
using BigBallz.Filters;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    public class CommentsController : SecureBaseController
    {
        private readonly ICommentsService _commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

         public CommentsController()
        {
            _commentsService = new CommentsService();
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
