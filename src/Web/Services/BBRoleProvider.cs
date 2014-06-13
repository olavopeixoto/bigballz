using System.Linq;
using System.Web.Security;
using BigBallz.Core;
using BigBallz.Core.Caching;
using BigBallz.Core.IoC;
using BigBallz.Infrastructure;
using BigBallz.Models;

namespace BigBallz.Services
{
    public sealed class BBRoleProvider : RoleProvider
    {
        private readonly DataContextProvider _provider;

        public BBRoleProvider()
        {
            _provider = ServiceLocator.Resolve<DataContextProvider>();
            ApplicationName = "BigBallz";
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (var db = _provider.CreateContext())
            {
                return db.Users.Any(x => x.UserName == username && x.Roles.Any(r => r.Name == roleName));
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            var cache = ServiceLocator.Resolve<ICache>();

            var key = string.Format("BB-GetRolesForUser-{0}", username);

            if (cache.Contains(key)) return cache.Get<string[]>(key);

            using (var db = _provider.CreateContext())
            {
                var user = db.Users.FirstOrDefault(x => x.UserName == username);
                var result = user == null ? new string[] {} : user.Roles.Select(x => x.Name).ToArray();
                
                cache.Set(key, result);

                return result;
            }
        }

        public override void CreateRole(string roleName)
        {
            using (var db = _provider.CreateContext())
            {
                var role = new Role
                               {
                                   Name = roleName
                               };
                db.Roles.Add(role);
                db.SaveChanges();
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return false;
        }

        public override bool RoleExists(string roleName)
        {
            using (var db = _provider.CreateContext())
            {
                return db.Roles.Any(x => x.Name == roleName);
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            using (var db = _provider.CreateContext())
            {
                foreach (var user in db.Users.Where(u => usernames.Contains(u.UserName)))
                {
                    foreach (var role in db.Roles.Where(r => roleNames.Contains(r.Name)))
                    {
                        user.Roles.Add(role);
                    }
                }

                db.SaveChanges();
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            using (var db = _provider.CreateContext())
            {
                foreach (var user in db.Users.Where(u => usernames.Contains(u.UserName)))
                {
                    foreach (var role in db.Roles.Where(r => roleNames.Contains(r.Name)))
                    {
                        user.Roles.Remove(role);
                    }
                }

                db.SaveChanges();
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            using (var db = _provider.CreateContext())
            {
                return
                    db.Roles.FirstOrDefault(r => r.Name == roleName)
                        .NullSafe(r => r.Users.Select(u => u.UserName).ToArray());
            }
        }

        public override string[] GetAllRoles()
        {
            using (var db = _provider.CreateContext())
            {
                return db.Roles.Select(x => x.Name).ToArray();
            }
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            using (var db = _provider.CreateContext())
            {
                var role = db.Roles.FirstOrDefault(x => x.Name == roleName);
                return role == null ? new string[] {} : role.Users.Select(x => x.UserName).ToArray();
            }
        }

        public override string ApplicationName { get; set; }
    }
}