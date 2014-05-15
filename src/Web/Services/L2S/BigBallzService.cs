using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using BigBallz.Core;
using BigBallz.Helpers;
using BigBallz.Infrastructure;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class BigBallzService : IBigBallzService
    {
        private readonly DataContextProvider _provider;

        public BigBallzService(DataContextProvider provider)
        {
            _provider = provider;
        }

        private int GetTotalUserPoints(string userName)
        {
            using (var db = _provider.CreateContext())
            {
                var dataOptions = new System.Data.Linq.DataLoadOptions();
                dataOptions.LoadWith<Bet>(ca => ca.Match1);
                db.LoadOptions = dataOptions;

                var bets = db.Bets.Where(x => x.User1.UserName == userName);

                dataOptions = new System.Data.Linq.DataLoadOptions();
                dataOptions.LoadWith<BonusBet>(ca => ca.Bonus11);
                db.LoadOptions = dataOptions;

                var bonusBets = db.BonusBets.Where(x => x.User1.UserName == userName);

                var totalPoints = Enumerable.Sum(bets, bet => BetPoints(bet));
                totalPoints += Enumerable.Sum(bonusBets, bet => BonusBetPoints(bet));

                return totalPoints;
            }
        }

        #region Points Rules
        protected int BetPoints(Bet bet)
        {
            if (!bet.Score1.HasValue || !bet.Score2.HasValue || !bet.Match1.Score1.HasValue || !bet.Match1.Score2.HasValue)
            {
                return 0;
            }

            var betScore1 = bet.Score1.Value;
            var betScore2 = bet.Score2.Value;
            var matchScore1 = bet.Match1.Score1.Value;
            var matchScore2 = bet.Match1.Score2.Value;

            if (PlacarExato(betScore1, betScore2, matchScore1, matchScore2))
            {
                return bet.Match1.StageId == 1 ? 7 : 10;
            }
            if (PlacarParcial(betScore1, betScore2, matchScore1, matchScore2)
                && Resultado(betScore1, betScore2, matchScore1, matchScore2))
            {
                return bet.Match1.StageId == 1 ? 3 : 5;
            }
            if (Resultado(betScore1, betScore2, matchScore1, matchScore2))
            {
                return bet.Match1.StageId == 1 ? 2 : 3;
            }
            if (PlacarParcial(betScore1, betScore2, matchScore1, matchScore2))
            {
                return bet.Match1.StageId == 1 ? 1 : 2;
            }

            return 0;
        }
        public bool PlacarExato(int betScore1, int betScore2, int matchScore1, int matchScore2)
        {
            return betScore1 == matchScore1 && betScore2 == matchScore2;
        }
        public bool PlacarParcial(int betScore1, int betScore2, int matchScore1, int matchScore2)
        {
            return betScore1 == matchScore1 || betScore2 == matchScore2;
        }
        public bool Resultado(int betScore1, int betScore2, int matchScore1, int matchScore2)
        {
            return (betScore1 > betScore2 && matchScore1 > matchScore2)
                   || (betScore1 < betScore2 && matchScore1 < matchScore2)
                   || (betScore1 == betScore2 && matchScore1 == matchScore2);
        }
        private int BonusBetPoints(BonusBet bet)
        {
            if (bet.Team == bet.Bonus11.Team)
                switch (bet.Bonus)
                {
                    case 1: //Campeao Copa
                        return 10;
                    case 2: //Vice Copa
                        return 7;
                    case 3: //Terceiro Copa
                    case 4: //Quarto Copa
                        return 5;
                    case 5:
                    case 6:
                    case 7:
                    case 8: 
                    case 9: 
                    case 10: 
                    case 11:
                    case 12: //Campeao Grupo
                        return 5;
                    default:
                        return 0;
                }

            return 0;
        }
#endregion

        public int GetUserStanding(string userName)
        {
            return GetStandings().FirstOrDefault(x => x.User.UserName == userName).NullSafe(x => x.Position, -1);
        }

        public int GetLastRoundPoints(string userName)
        {
            using (var db = _provider.CreateContext())
            {
                var dataOptions = new System.Data.Linq.DataLoadOptions();
                dataOptions.LoadWith<Bet>(ca => ca.Match1);
                db.LoadOptions = dataOptions;

                var bets =
                    db.Bets.Where(
                        x =>
                            x.User1.UserName == userName &&
                            x.Match1.StartTime.DayOfYear == DateTime.Now.BrazilTimeZone().DayOfYear &&
                            x.Match1.StartTime.Year == DateTime.Now.BrazilTimeZone().Year);

                return Enumerable.Sum(bets, bet => BetPoints(bet));
            }
        }

        public IList<Match> GetUserPendingBets(string userName)
        {
            using (var db = _provider.CreateContext())
            {
                return
                    db.Matches.Where(
                        x =>
                            x.Bets.All(y => y.User1.UserName != userName) &&
                            x.StartTime <= DateTime.Now.BrazilTimeZone().AddHours(24)).ToList();
            }
        }

        public IList<Bonus> GetUserPendingBonusBets(string userName)
        {
            using (var db = _provider.CreateContext())
            {
                return db.Bonus.Where(x => x.BonusBets.All(y => y.User1.UserName != userName)).ToList();
            }
        }

        public IList<UserPoints> GetStandings()
        {
            var cache = HttpContext.Current.Cache;
            const string key = "x-GetStandings";
            if (cache.Get(key) != null)
                return (IList<UserPoints>) cache.Get(key);

            using (var db = _provider.CreateContext())
            {
                var users = db.Users.Where(x => x.Authorized).ToList();
                var userPointsList = users.Select(user => new UserPoints
                {
                    User = user,
                    TotalPoints = GetTotalUserPoints(user.UserName),
                    TotalDayPoints = GetLastRoundPoints(user.UserName),
                    TotalExactScore = GetTotalUserExactScores(user.UserName),
                    TotalBonusPoints = GetTotalUserBonusPoints(user.UserName)
                })
                    .OrderByDescending(x => x.TotalPoints)
                    .ThenByDescending(x => x.TotalExactScore)
                    .ThenByDescending(x => x.TotalBonusPoints)
                    .ThenBy(x => x.User.UserName)
                    .ToList();

                var j = 1;
                for (var i = 0; i < userPointsList.Count(); i++)
                {
                    if (i > 0)
                    {
                        var lastUser = userPointsList[i - 1];
                        var currentUser = userPointsList[i];
                        if (lastUser.TotalPoints > currentUser.TotalPoints
                            ||
                            (lastUser.TotalPoints == currentUser.TotalPoints &&
                             lastUser.TotalExactScore > currentUser.TotalExactScore)
                            ||
                            (lastUser.TotalPoints == currentUser.TotalPoints &&
                             lastUser.TotalExactScore == currentUser.TotalExactScore &&
                             lastUser.TotalBonusPoints > currentUser.TotalBonusPoints))
                        {
                            currentUser.Position = lastUser.Position + j;
                            j = 1;
                        }
                        else
                        {
                            currentUser.Position = lastUser.Position;
                            j++;
                        }
                    }
                    else
                    {
                        userPointsList[i].Position = i + 1;
                    }
                }

                cache.Add(key, userPointsList, null, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                return userPointsList;
            }
        }

        private int GetTotalUserBonusPoints(string userName)
        {
            using (var db = _provider.CreateContext())
            {
                var bonusBets = db.BonusBets.Where(x => x.User1.UserName == userName);

                var totalBonusPoints = Enumerable.Sum(bonusBets, bet => BonusBetPoints(bet));
                return totalBonusPoints;
            }
        }

        public IList<UserPoints> GetLastRoundStandings()
        {
            var userPointsList = GetStandings().OrderByDescending(x => x.TotalDayPoints).ThenBy(x => x.User.UserName).ToList();

            for (var i = 0; i < userPointsList.Count(); i++)
            {
                if (i > 0)
                {
                    var lastUser = userPointsList[i - 1];
                    var currentUser = userPointsList[i];
                    if (lastUser.TotalDayPoints > currentUser.TotalDayPoints)
                    {
                        currentUser.Position = lastUser.Position + 1;
                    }
                    else
                    {
                        currentUser.Position = lastUser.Position;
                    }
                }
                else
                {
                    userPointsList[i].Position = i + 1;
                }
            }

            return userPointsList;
        }

        public IList<BetPoints> GetUserPointsByMatch(string userName)
        {
            using (var db = _provider.CreateContext())
            {
                return db.Bets.Where(x => x.User1.UserName == userName).ToList().Select(x => new BetPoints
                {
                    Bet = x,
                    Points = BetPoints(x)
                }).ToList();
            }
        }

        public IList<BonusPoints> GetUserPointsByBonus(string userName)
        {
            using (var db = _provider.CreateContext())
            {
                var dataOptions = new System.Data.Linq.DataLoadOptions();
                dataOptions.LoadWith<BonusBet>(ca => ca.Bonus11);
                db.LoadOptions = dataOptions;

                return db.BonusBets.Where(x => x.User1.UserName == userName).ToList().Select(x => new BonusPoints
                {
                    BonusBet = x,
                    Points = BonusBetPoints(x)
                }).ToList();
            }
        }

        public IList<UserMatchPoints> GetUserPointsByExpiredMatch(int matchId)
        {
            using (var db = _provider.CreateContext())
            {
                var dataOptions = new System.Data.Linq.DataLoadOptions();
                dataOptions.LoadWith<Bet>(ca => ca.Match1);
                dataOptions.LoadWith<Bet>(ca => ca.User1);
                db.LoadOptions = dataOptions;

                return (from bet in db.Bets
                    where bet.Match == matchId && DateTime.Now.BrazilTimeZone() > bet.Match1.StartTime.AddHours(-1)
                    select new UserMatchPoints
                    {
                        Bet = bet,
                        Points = BetPoints(bet)
                    }
                    ).ToList().OrderByDescending(x => x.Points).ThenBy(x => x.Bet.User1.UserName).ToList();
            }
        }

        public Match GetFirstMatch()
        {
            using (var db = _provider.CreateContext())
            {
                return db.Matches.OrderBy(x => x.StartTime).FirstOrDefault();
            }
        }

        public double GetTotalPrize()
        {
            using (var db = _provider.CreateContext())
            {
                var totalPagantes =
                    db.Users.Count(x => x.Authorized && x.UserRoles.All(y => y.Role.Name != BBRoles.Admin));
                return totalPagantes * (double)ConfigurationHelper.Price * (1 - 0.0499) - (0.4 * totalPagantes); //Taxa do PagSeguro = 4,99% + R$0,40
            }
        }

        private int GetTotalUserExactScores(string userName)
        {
            using (var db = _provider.CreateContext())
            {
                var dataOptions = new System.Data.Linq.DataLoadOptions();
                dataOptions.LoadWith<Bet>(ca => ca.Match1);
                db.LoadOptions = dataOptions;

                var bets = db.Bets.Where(x => x.User1.UserName == userName);

                var exactScores =
                    Enumerable.Count(
                        bets.Where(
                            bet =>
                                bet.Score1 != null && bet.Score2 != null && bet.Match1.Score1 != null &&
                                bet.Match1.Score2 != null),
                        bet =>
                            PlacarExato(bet.Score1.Value, bet.Score2.Value, bet.Match1.Score1.Value,
                                bet.Match1.Score2.Value));

                return exactScores;
            }
        }

        public DateTime GetBonusBetExpireDate()
        {
            using (var db = _provider.CreateContext())
            {
                if (!db.Matches.Any()) return DateTime.Now;

                var minDate = db.Matches.Min(x => x.StartTime);
                minDate = minDate.AddDays(-1);
                var endBonus = new DateTime(minDate.Year, minDate.Month, minDate.Day, 23, 59, 59);
                return endBonus;
            }
        }

        public MatchBetStatistic GetMatchBetStatistics(int matchId)
        {
            using (var db = _provider.CreateContext())
            {
                var bets = db.Bets.Where(x => x.Match == matchId).ToList();
                double totalBets = bets.Count();

                if (totalBets <= 0) return null;

                var mostBetScore =
                    bets.GroupBy(x => new {x.Score1, x.Score2})
                        .Select(x => new {x.Key, qtd = x.Count()})
                        .OrderByDescending(
                            x => x.qtd).Select(x => x.Key).FirstOrDefault();
                return new MatchBetStatistic
                {
                    Match = bets.First().Match1,
                    AverageScore1 = bets.Average(x => x.Score1).Value,
                    AverageScore2 = bets.Average(x => x.Score2).Value,
                    Score1MostBet = mostBetScore.Score1.Value,
                    Score2MostBet = mostBetScore.Score2.Value,
                    Team1Perc = bets.Count(x => x.Score1 > x.Score2)/totalBets,
                    Team2Perc = bets.Count(x => x.Score1 < x.Score2)/totalBets,
                    TiePerc = bets.Count(x => x.Score1 == x.Score2)/totalBets,
                };
            }
        }

        public BonusBetStatistic GetBonusBetStatistics(int bonusId)
        {
            using (var db = _provider.CreateContext())
            {
                var bets = db.BonusBets.Where(x => x.Bonus == bonusId).ToList();
                double totalBets = bets.Count();
                var mostBetTeam =
                    bets.GroupBy(x => new {x.Team1}).Select(x => new {x.Key, qtd = x.Count()}).OrderByDescending(
                        x => x.qtd).Select(x => x.Key).First();

                return new BonusBetStatistic
                {
                    Bonus = bets.First().Bonus11,
                    Team = mostBetTeam.Team1,
                    TeamPerc = bets.Count(x => x.Team1 == mostBetTeam.Team1)/totalBets,
                };
            }
        }
    }
}