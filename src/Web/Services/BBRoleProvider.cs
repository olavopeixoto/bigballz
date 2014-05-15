using System.Linq;
using System.Web.Security;
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
                return db.Users.Any(x => x.UserName == username && x.UserRoles.Any(r => r.Role.Name == roleName));
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
                var result = user == null ? new string[] {} : user.UserRoles.Select(x => x.Role.Name).ToArray();
                
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
                db.Roles.InsertOnSubmit(role);
                db.SubmitChanges();
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
                foreach (var roleName in roleNames)
                {
                    var role = db.Roles.FirstOrDefault(x => x.Name == roleName);
                    if (role != null)
                    {
                        var users = db.Users.Where(x => usernames.Contains(x.UserName)).Select(x => new UserRole
                                                                                                         {
                                                                                                             Role = role,
                                                                                                             User = x
                                                                                                         }).ToList();
                        role.UserRoles.AddRange(users);
                    }
                }

                db.SubmitChanges();
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            using (var db = _provider.CreateContext())
            {
                var userRoles =
                    db.UserRoles.Where(x => usernames.Contains(x.User.UserName) && roleNames.Contains(x.Role.Name));
                db.UserRoles.DeleteAllOnSubmit(userRoles);
                db.SubmitChanges();
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            using (var db = _provider.CreateContext())
            {
                return db.UserRoles.Where(x => x.Role.Name == roleName).Select(x => x.User.UserName).ToArray();
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
                return role == null ? new string[] {} : role.UserRoles.Select(x => x.User.UserName).ToArray();
            }
        }

        public override string ApplicationName { get; set; }
    }
}