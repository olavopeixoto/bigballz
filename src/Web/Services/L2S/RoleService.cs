using System.Linq;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class RoleService : IRoleService
    {
        readonly BigBallzDataContext _db;

        public RoleService(BigBallzDataContext context)
        {
            _db = context;
        }

        public IQueryable<Role> GetAll()
        {
            return _db.Roles;
        }

        public Role Get(string name)
        {
            return _db.Roles.SingleOrDefault(d => d.Name == name);
        }

        public Role Get(int id)
        {
            return _db.Roles.SingleOrDefault(d => d.RoleId == id);
        }

        public void Add(Role role)
        {
            _db.Roles.InsertOnSubmit(role);
        }

        public void Delete(Role role)
        {
            _db.Roles.DeleteOnSubmit(role);
        }

        public void Save()
        {
            _db.SubmitChanges();
        }
    }
}