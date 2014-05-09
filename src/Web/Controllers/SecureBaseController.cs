using System.Web.Mvc;

namespace BigBallz.Controllers
{
    [Authorize]
    public abstract class SecureBaseController : BaseController
    {
    }
}