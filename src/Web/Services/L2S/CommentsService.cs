using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using BigBallz.Core;
using BigBallz.Infrastructure;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class CommentsService : ICommentsService
    {
        private readonly DataContextProvider _provider;

        public CommentsService(DataContextProvider provider)
        {
            _provider = provider;
        }

        public IList<Comment> GetComments()
        {
            using (var db = _provider.CreateContext())
            {
                var options = new DataLoadOptions();
                options.LoadWith<Comment>(x => x.User1);
                db.LoadOptions = options;
                return db.Comments.OrderByDescending(x => x.CommentedOn).ToList();
            }
        }

        public IList<Comment> GetComments(int top)
        {
            using (var db = _provider.CreateContext())
            {
                var options = new DataLoadOptions();
                options.LoadWith<Comment>(x => x.User1);
                db.LoadOptions = options;
                return db.Comments.OrderByDescending(x => x.CommentedOn).Take(top).ToList();
            }
        }

        public IList<Comment> GetComments(string userName)
        {
            using (var db = _provider.CreateContext())
            {
                var options = new DataLoadOptions();
                options.LoadWith<Comment>(x => x.User1);
                db.LoadOptions = options;
                return db.Comments.Where(x => x.User1.UserName == userName).ToList();
            }
        }

        public void PostComment(string userName, string comment)
        {
            using (var db = _provider.CreateContext())
            {
                var user = db.Users.FirstOrDefault(x => x.UserName == userName);
                var post = new Comment
                {
                    Comments = comment,
                    User = user.UserId,
                    CommentedOn = DateTime.Now.BrazilTimeZone()
                };
                user.Comments.Add(post);
                db.SubmitChanges();
            }
        }

        public void Dispose()
        {}
    }
}