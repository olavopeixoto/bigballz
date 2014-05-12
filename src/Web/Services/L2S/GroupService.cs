using System.Linq;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class GroupService : IGroupService
    {
        readonly BigBallzDataContext _db;

        public GroupService(BigBallzDataContext context)
        {
            _db = context;
        }

        public IQueryable<Group> GetAll()
        {
            return _db.Groups;
        }

        public Group Get(int id)
        {
            return _db.Groups.SingleOrDefault(d => d.GroupId == id);
        }

        public void Add(Group group)
        {
            _db.Groups.InsertOnSubmit(group);
        }

        public void Delete(Group group)
        {
            _db.Groups.DeleteOnSubmit(group);
        }

        public void Save()
        {
            _db.SubmitChanges();
        }
    }
}