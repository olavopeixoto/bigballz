﻿using System;
using System.Collections.Generic;
using BigBallz.Models;

namespace BigBallz.Services
{
    public interface IBigBallzService : IDisposable
    {
        IList<Match> GetUserPendingBets(string userName);
        IList<UserPoints> GetStandings();
        IList<UserPoints> GetLastRoundStandings();
        IList<BetPoints> GetUserPointsByMatch(string userName);
        IList<BonusPoints> GetUserPointsByBonus(string userName);
        IList<UserMatchPoints> GetUserPointsByExpiredMatch(int matchId);
        MatchBetStatistic GetMatchBetStatistics(int matchId);
        BonusBetStatistic GetBonusBetStatistics(int bonusId);
        Match GetFirstMatch();
        decimal GetTotalPrize();
        DateTime GetBonusBetExpireDate();
        IEnumerable<MoneyDistribution> GetMoneyDistribution();
    }
}