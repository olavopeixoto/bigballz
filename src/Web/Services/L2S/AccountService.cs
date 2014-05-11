using System;
using System.Collections.Generic;
using System.Linq;
using BigBallz.Core;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class AccountService : IAccountService
    {
        readonly BigBallzDataContext _db = new BigBallzDataContext();

        public User FindUserByIdentifier(string identifier)
        {
            return (from map in _db.UserMappings
                   where map.Identifier == identifier
                   select map.User).FirstOrDefault();
        }

        public User FindUserByUserName(string userName)
        {
            return _db.Users.FirstOrDefault(x => x.UserName == userName);
        }

        public User FindUserByLocalId(int userId)
        {
            return _db.Users.FirstOrDefault(x => x.UserId == userId);
        }

        public void CreateUser(string identifier, string username, string providerName, string emailAddress, bool emailVerified, string photoUrl)
        {
            var user = new User
                           {
                               UserName = username,
                               PhotoUrl = photoUrl,
                               EmailAddress = emailAddress,
                               EmailAddressVerified = emailVerified,
                               CreatedOn = DateTime.Now.BrazilTimeZone()
                           };
            var map = new UserMapping
                          {
                              CreatedOn = DateTime.Now.BrazilTimeZone(),
                              Identifier = identifier,
                              ProviderName = providerName,
                              User = user
                          };

            user.UserMappings.Add(map);

            _db.Users.InsertOnSubmit(user);

            _db.SubmitChanges();
        }

        public void AssociateExistingUser(int userId, string identifier, string providerName)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserId == userId);
            
            if (user == null) return;

            var map = new UserMapping
                          {
                              CreatedOn = DateTime.Now.BrazilTimeZone(),
                              Identifier = identifier,
                              ProviderName = providerName,
                              User = user
                          };
            user.UserMappings.Add(map);

            _db.SubmitChanges();
        }

        public void AuthorizeUser(string userName)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserName == userName);
            user.Authorized = true;
            user.UserRoles.Add(new UserRole
                                   {
                                       Role = _db.Roles.FirstOrDefault(x => x.Name == BBRoles.Player),
                                       User = user
                                   });
            _db.SubmitChanges();
        }

        public bool VerifyEmail(string userName)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserName == userName);

            if (user == null) return false;

            if (!user.EmailAddressVerified)
            {
                user.EmailAddressVerified = true;
                _db.SubmitChanges();
            }

            return true;
        }

        public int GetTotalAuthorizedUsers()
        {
            return (from user in _db.Users
                    where user.Authorized && user.UserRoles.All(x => x.Role.Name != BBRoles.Admin)
                    select user).Count();
        }

        public int GetTotalPlayers()
        {
            return (from user in _db.Users
                    where user.Authorized && user.UserRoles.Any(x => x.Role.Name == BBRoles.Player)
                    select user).Count();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _db.Users.ToList();
        }

        public IList<User> GetAllPlayers()
        {
            return _db.Users.Where(x => x.UserRoles.Any(y => y.Role.Name == BBRoles.Player)).ToList();
        }

        public IList<User> GetAllUnAuthorizedUsers()
        {
            return _db.Users.Where(x => !x.UserRoles.Any(y => y.Role.Name == BBRoles.Player)).ToList();
        }

        public void UpdateUserInformation(User user)
        {
            var dbUser = _db.Users.FirstOrDefault(x => x.UserId == user.UserId);

            if (dbUser == null) return;

            dbUser.PhotoUrl = user.PhotoUrl;
            _db.SubmitChanges();
        }
    }
}