using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigBallz.Core;
using BigBallz.Models;
using BigBallz.Services;
using BigBallz.Services.L2S;

namespace BigBallz.Controllers
{
    [Authorize]
    public abstract class SecureBaseController : BaseController
    {
    }
}