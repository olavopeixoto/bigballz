using System;
using System.Collections.Generic;
using BigBallz.Models;

namespace BigBallz.Services
{
    public interface IMailService
    {
        void SendRegistration(User user, string paymentUrl, string activationUrl);
        void SendPaymentConfirmation(User user);
        void SendEndBetAlert(User user, IList<Bet> bets);
        void SendEndBonusAlert(User user, IList<BonusBet> bonusBets);
        void SendBonusExpirationWarning(User user, DateTime startTime);
        void SendNewCommentPosted(User[] toArray, string userName, string comment);
        void SendBetWarning(User player, IList<Match> matches);
    }
}