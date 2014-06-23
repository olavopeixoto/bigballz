﻿using System;
using System.Linq;
using System.Web.Mvc;
using BigBallz.Core.Log;
using BigBallz.Filters;
using BigBallz.Services;

namespace BigBallz.Controllers
{
    public class CommentsController : BaseController
    {
        private readonly ICommentsService _commentsService;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;

        public CommentsController(ICommentsService commentsService, IMailService mailService, IBigBallzService bigBallzService, IMatchService matchService, IUserService userService) : base(userService, matchService, bigBallzService)
        {
            _commentsService = commentsService;
            _userService = userService;
            _mailService = mailService;
        }

        [HttpGet, UserNameFilter]
        public ActionResult Index(string userName)
        {
            var comments = _commentsService.GetComments();
            ViewBag.EmailAlert = _userService.Get(userName).EmailAlert;

            return View(comments);
        }

        [HttpPost, UserNameFilter]
        public ActionResult Post(string userName, string comment)
        {
            _commentsService.PostComment(userName, comment);

            try
            {
                _mailService.SendNewCommentPosted(_userService.GetAll().Where(u => u.EmailAddressVerified && u.EmailAlert && u.Authorized).ToArray(), userName, comment);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return RedirectToAction("index");
        }

        [HttpPost, UserNameFilter]
        public void UpdateEmailPreferences(string userName, bool sendEmail)
        {
            var user = _userService.Get(userName);
            user.EmailAlert = sendEmail;
            _userService.Save();
        }
    }
}
