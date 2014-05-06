using System.Linq;
using BigBallz.Services;

namespace BigBallz.Models
{
    public class GroupService : IGroupService
    {
        readonly BigBallzDataContext db = new BigBallzDataContext();

        public IQueryable<Group> GetAll()
        {
            return db.Groups;
        }

        public Group Get(int id)
        {
            return db.Groups.SingleOrDefault(d => d.GroupId == id);
        }

        public void Add(Group group)
        {
            db.Groups.InsertOnSubmit(group);
        }

        public void Delete(Group group)
        {
            db.Groups.DeleteOnSubmit(group);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}