using System;
using System.Collections.Generic;
using System.Linq;
using BigBallz.Core;
using BigBallz.Core.Caching;
using BigBallz.Models;
using Uol.PagSeguro.Domain;

namespace BigBallz.Services.L2S
{
    public class AccountService : IAccountService
    {
        private readonly BigBallzDataContext _db;
        private readonly ICache _cache;

        public AccountService(BigBallzDataContext context, ICache cache)
        {
            _db = context;
            _cache = cache;
        }

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

            _db.Users.Add(user);

            _db.SaveChanges();

            _cache.Clear();
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

            _db.SaveChanges();
        }

        public bool AuthorizeUser(string userName, string adminName, bool pagSeguro = false)
        {
            var user = _db.Users.First(x => x.UserName == userName);
            if (user.Authorized) return false;

            user.Authorized = true;
            user.PagSeguro = pagSeguro;
            user.AuthorizedBy = adminName;
            if (user.Roles.All(x => x.Name.ToLowerInvariant() != BBRoles.Player))
            {
                user.Roles.Add(_db.Roles.FirstOrDefault(x => x.Name == BBRoles.Player));
            }
            
            _db.SaveChanges();

            _cache.Clear();

            return true;
        }

        public bool VerifyEmail(string userName)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserName == userName);

            if (user == null) return false;

            if (!user.EmailAddressVerified)
            {
                user.EmailAddressVerified = true;
                _db.SaveChanges();
                _cache.Clear();
            }

            return true;
        }

        public int GetTotalAuthorizedUsers()
        {
            return (from user in _db.Users
                    where user.Authorized && user.Roles.All(x => x.Name != BBRoles.Admin)
                    select user).Count();
        }

        public int GetTotalPlayers()
        {
            return (from user in _db.Users
                    where user.Authorized && user.Roles.Any(x => x.Name == BBRoles.Player)
                    select user).Count();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _db.Users.ToList();
        }

        public IList<User> GetAllPlayers()
        {
            return _db.Users.Where(x => x.Roles.Any(y => y.Name == BBRoles.Player)).ToList();
        }

        public IList<User> GetAllUnAuthorizedUsers()
        {
            return _db.Users.Where(x => x.Roles.All(y => y.Name != BBRoles.Player)).ToList();
        }

        public void UpdateUserInformation(User user)
        {
            var dbUser = _db.Users.FirstOrDefault(x => x.UserId == user.UserId);

            if (dbUser == null) return;

            dbUser.PhotoUrl = user.PhotoUrl;
            _db.SaveChanges();
            _cache.Clear();
        }

        public void UpdateTransactionStatus(User user, Transaction transaction)
        {
            var paymentStatus = new PaymentStatus
            {
                Date = transaction.Date,
                LastEventDate = transaction.LastEventDate,
                SenderName = transaction.Sender.Name,
                SenderEmail = transaction.Sender.Email,
                Transaction = transaction.Code,
                User1 = user,
                User = user.UserId,
                PaymentMethod = GetPaymentMethodDescription(transaction.PaymentMethod.PaymentMethodCode),
                Status = GetStatusDescription(transaction.TransactionStatus)
            };

            _db.PaymentStatus.Add(paymentStatus);
            _db.SaveChanges();
        }

        private string GetPaymentMethodDescription(int paymentMethodCode)
        {
            var status = (PagSeguroPaymentMethod)paymentMethodCode;
            return status.ToString();
        }

        private string GetStatusDescription(int transactionStatus)
        {
            var status = (PagSeguroTransactionStatus)transactionStatus;
            return status.ToString();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}