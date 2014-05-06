using System.Linq;
using BigBallz.Services;

namespace BigBallz.Models
{
    public class ForumService : IForumService
    {
        readonly BigBallzDataContext db = new BigBallzDataContext();

        public IQueryable<Forum> GetAll()
        {
            return db.Forums;
        }

        public Forum Get(int id)
        {
            return db.Forums.SingleOrDefault(d => d.ForumId == id);
        }

        public void Add(Forum forum)
        {
            db.Forums.InsertOnSubmit(forum);
        }

        public void Delete(Forum forum)
        {
            db.Forums.DeleteOnSubmit(forum);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}