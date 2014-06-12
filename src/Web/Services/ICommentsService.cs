﻿using System;
using System.Collections.Generic;
using BigBallz.Models;

namespace BigBallz.Services
{
    public interface ICommentsService : IDisposable
    {
        IList<Comment> GetComments();
        IList<Comment> GetComments(int top);
        IList<Comment> GetComments(string userName);
        void PostComment(string userName, string comment);
    }
}
