using System;
using System.Collections.Generic;
using BigBallz.Models;
using Uol.PagSeguro.Domain;

namespace BigBallz.Services
{
    public interface IAccountService : IDisposable
    {
        User FindUserByIdentifier(string identifier);
        User FindUserByUserName(string userName);
        User FindUserByLocalId(int userId);
        void CreateUser(string identifier, string username, string providerName, string emailAddress, bool emailVerified, string photoUrl);
        void AssociateExistingUser(int userId, string identifier, string providerName);
        bool AuthorizeUser(string userName, string adminName, bool pagSeguro = false);
        int GetTotalAuthorizedUsers();
        int GetTotalPlayers();
        IEnumerable<User> GetAllUsers();
        bool VerifyEmail(string userName);
        IList<User> GetAllPlayers();
        IList<User> GetAllUnAuthorizedUsers();
        void UpdateUserInformation(User user);
        void UpdateTransactionStatus(User user, Transaction transaction);
    }
}