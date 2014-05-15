using System.Linq;
using System.Web;
using System.Web.Security;
using BigBallz.Core.IoC;
using BigBallz.Infrastructure;
using BigBallz.Models;
using StackExchange.Profiling;

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
            using (MiniProfiler.Current.Step("IsUserInRole"))
            {
                var key = string.Format("x-role-{0}-{1}", username, roleName);
                if (HttpContext.Current.Items[key] as bool? != null) return (bool) HttpContext.Current.Items[key];
                using (var db = _provider.CreateContext())
                {
                    return
                        (bool)
                            (HttpContext.Current.Items[key] =
                                db.Users.Any(
                                    x => x.UserName == username && x.UserRoles.Any(r => r.Role.Name == roleName)));
                }
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            using (MiniProfiler.Current.Step("GetRolesForUser"))
            {
                var key = string.Format("x-role-{0}", username);
                if (HttpContext.Current.Items[key] as string[] != null) return (string[])HttpContext.Current.Items[key];
                using (var db = _provider.CreateContext())
                {
                    var userRoles = db.UserRoles.Where(x => x.User.UserName == username).Select(x => x.Role.Name).ToArray();
                    return (string[])(HttpContext.Current.Items[key] = userRoles);
                }
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