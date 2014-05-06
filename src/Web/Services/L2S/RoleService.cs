using System;
using System.Linq;
using BigBallz.Services;

namespace BigBallz.Models
{
    public class RoleService : IRoleService
    {
        readonly BigBallzDataContext db = new BigBallzDataContext();

        public IQueryable<Role> GetAll()
        {
            return db.Roles;
        }

        public Role Get(string name)
        {
            return db.Roles.SingleOrDefault(d => d.Name == name);
        }

        public Role Get(int id)
        {
            return db.Roles.SingleOrDefault(d => d.RoleId == id);
        }

        public void Add(Role role)
        {
            db.Roles.InsertOnSubmit(role);
        }

        public void Delete(Role role)
        {
            db.Roles.DeleteOnSubmit(role);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}