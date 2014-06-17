using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                return db.Comments
                    .OrderByDescending(x => x.CommentedOn)
                    .Include(x => x.User)
                    .ToList();
            }
        }

        public IList<Comment> GetComments(int top)
        {
            using (var db = _provider.CreateContext())
            {
                return db.Comments
                    .OrderByDescending(x => x.CommentedOn)
                    .Include(x => x.User)
                    .Take(top)
                    .ToList();
            }
        }

        public IList<Comment> GetComments(string userName)
        {
            using (var db = _provider.CreateContext())
            {
                return db.Comments
                            .Where(x => x.User.UserName == userName)
                            .Include(x => x.User)
                            .ToList();
            }
        }

        public void PostComment(string userName, string comment)
        {
            using (var db = _provider.CreateContext())
            {
                var user = db.Users.First(x => x.UserName == userName);

                var post = new Comment
                {
                    Comments = comment,
                    UserId = user.UserId,
                    CommentedOn = DateTime.Now.BrazilTimeZone()
                };
                user.Comments.Add(post);
                db.SaveChanges();
            }
        }

        public void Dispose()
        {}
    }
}