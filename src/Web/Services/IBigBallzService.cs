using System;
using System.Collections.Generic;
using BigBallz.Models;

namespace BigBallz.Services
{
    public interface IBigBallzService : IDisposable
    {
        IList<Match> GetUserPendingBets(string userName);
        IList<UserPoints> GetStandings(DateTime? lastRoundDate = null);
        IList<UserPoints> GetLastRoundStandings();
        IList<BetPoints> GetUserPointsByMatch(User user);
        IList<BonusPoints> GetUserPointsByBonus(User user);
        IList<UserMatchPoints> GetUserPointsByExpiredMatch(int matchId);
        MatchBetStatistic GetMatchBetStatistics(int matchId);
        BonusBetStatistic GetBonusBetStatistics(int bonusId);
        Match GetFirstMatch();
        Prizes GetPrizes();
        DateTime GetBonusBetExpireDate();
    }
}