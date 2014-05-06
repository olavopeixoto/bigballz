using System.Collections.Generic;
using BigBallz.Models;

namespace BigBallz.Services
{
    public interface IAccountService
    {
        User FindUserByIdentifier(string identifier);
        User FindUserByUserName(string userName);
        User FindUserByLocalId(int userId);
        void CreateUser(string identifier, string username, string providerName, string emailAddress, bool emailVerified, string photoUrl);
        void AssociateExistingUser(int userId, string identifier, string providerName);
        void AuthorizeUser(string userName);
        int GetTotalAuthorizedUsers();
        int GetTotalPlayers();
        IEnumerable<User> GetAllUsers();
        bool VerifyEmail(string userName);
        IList<User> GetAllPlayers();
        IList<User> GetAllUnAuthorizedUsers();
        void UpdateUserInformation(User user);
    }
}