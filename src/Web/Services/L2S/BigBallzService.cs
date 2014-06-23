using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using BigBallz.Core;
using BigBallz.Helpers;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class BigBallzService : IBigBallzService, IDisposable
    {
        readonly BigBallzDataContext _db;

        public BigBallzService(BigBallzDataContext context)
        {
            _db = context;

            var options = new DataLoadOptions();
            options.LoadWith<BonusBet>(x => x.Bonus11);
            options.LoadWith<BonusBet>(x => x.Team1);

            options.LoadWith<Bet>(x => x.Match1);
            options.LoadWith<Match>(x => x.Team1);
            options.LoadWith<Match>(x => x.Team2);

            options.LoadWith<User>(x => x.Bets);
            options.LoadWith<User>(x => x.BonusBets);

            _db.LoadOptions = options;
        }

        private int GetTotalUserPoints(User user, DateTime? untilDate = null)
        {
            untilDate = untilDate ?? DateTime.Now.AddHours(-1).BrazilTimeZone();

            var totalPoints = user.Bets.Where(b => b.Match1.StartTime <= untilDate).Sum(bet => BetPoints(bet));
            totalPoints += user.BonusBets.Where(b => b.Bonus11.LastModified <= untilDate).Sum(bet => BonusBetPoints(bet));
            return totalPoints;
        }

        #region Points Rules

        private int BetPoints(Bet bet)
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
            if (bet.Team != bet.Bonus11.Team) return 0;

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
        }
#endregion

        private int GetLastRoundPoints(User user)
        {
            var lastRoundTime =
                _db.Matches.Where(x => x.Score1.HasValue && x.Score2.HasValue)
                    .Select(x => (DateTime?)x.StartTime)
                    .OrderByDescending(x => x)
                    .FirstOrDefault();

            if (lastRoundTime == null) return 0;

            var bets = user.Bets
                            .Where(x =>
                                x.Match1.StartTime.DayOfYear == lastRoundTime.Value.DayOfYear &&
                                x.Match1.StartTime.Year == lastRoundTime.Value.Year);
                
            return bets.Sum(bet => BetPoints(bet));
        }

        public IList<Match> GetUserPendingBets(string userName)
        {
            return _db.Matches.Where(x => x.Bets.All(y => y.User1.UserName != userName) && x.StartTime <= DateTime.Now.BrazilTimeZone().AddHours(24)).ToList();
        }

        public IList<UserPoints> GetStandings()
        {
            var position = 1;
            var users = _db.Users.Where(x => x.Authorized).ToList();

            var lastRoundDate =
                _db.Matches.Where(
                    m =>
                        m.Score1.HasValue &&
                        m.StartTime < _db.Matches.Where(lm => lm.Score1.HasValue).Max(lm => lm.StartTime))
                    .OrderByDescending(m => m.StartTime)
                    .Select(m => (DateTime?)m.StartTime)
                    .FirstOrDefault();

            var lastStandings = !lastRoundDate.HasValue ? new List<UserPoints>() : users.Select(user => new UserPoints
            {
                User = user,
                TotalPoints = GetTotalUserPoints(user, lastRoundDate),
                TotalExactScore = GetTotalUserExactScores(user, lastRoundDate),
                TotalBonusPoints = GetTotalUserBonusPoints(user, lastRoundDate)
            })
            .GroupBy(x => new { x.TotalPoints, x.TotalExactScore, x.TotalBonusPoints })
            .OrderByDescending(x => x.Key.TotalPoints)
            .ThenByDescending(x => x.Key.TotalExactScore)
            .ThenByDescending(x => x.Key.TotalBonusPoints)
            .Select((x, i) =>
            {
                x.ForEach(u => u.Position = position);
                position += x.Count();
                return x;
            })
            .SelectMany((k, u) => k, (k, u) => u)
            .ToList();

            position = 1;

            return users.Select(user => new UserPoints
            {
                User = user,
                TotalPoints = GetTotalUserPoints(user),
                TotalDayPoints = GetLastRoundPoints(user),
                TotalExactScore = GetTotalUserExactScores(user),
                TotalBonusPoints = GetTotalUserBonusPoints(user),
                LastPosition = lastStandings.Where(l => l.User.UserId == user.UserId).Select(l => (int?)l.Position).FirstOrDefault() ?? 1
            })
            .GroupBy(x => new { x.TotalPoints, x.TotalExactScore, x.TotalBonusPoints })
            .OrderByDescending(x => x.Key.TotalPoints)
            .ThenByDescending(x => x.Key.TotalExactScore)
            .ThenByDescending(x => x.Key.TotalBonusPoints)
            .Select((x, i) =>
            {
                x.ForEach(u => u.Position = position);
                position += x.Count();
                return x;
            })
            .SelectMany((k, u) => k, (k, u) => u)
            .ToList();
        }

        private int GetTotalUserBonusPoints(User user, DateTime? untilDate = null)
        {
            untilDate = untilDate ?? DateTime.Now.AddHours(-1).BrazilTimeZone();

            var totalBonusPoints = user.BonusBets.Where(b => b.Bonus11.LastModified <= untilDate).Sum(bet => BonusBetPoints(bet));
            return totalBonusPoints;
        }

        public IList<UserPoints> GetLastRoundStandings()
        {
            var position = 1;
            return GetStandings()
                .GroupBy(x => x.TotalDayPoints)
                .OrderByDescending(x => x.Key)
                .Select((x, i) =>
                {
                    x.ForEach(u => u.Position = position);
                    position += x.Count();
                    return x;
                })
                .SelectMany((k,u) => k, (k,u) => u)
                .OrderBy(x => x.Position)
                .ThenBy(x => x.User.UserName)
                .ToList();
        }

        public IList<BetPoints> GetUserPointsByMatch(User user)
        {
            return user.Bets.Select(x => new BetPoints
                                            {
                                                Bet = x,
                                                Points = BetPoints(x)
                                            }).ToList();
        }

        public IList<BonusPoints> GetUserPointsByBonus(User user)
        {
            return user.BonusBets
                .Select(x => new BonusPoints
                                {
                                    BonusBet = x,
                                    Points = BonusBetPoints(x)
                                }).ToList();
        }

        public IList<UserMatchPoints> GetUserPointsByExpiredMatch(int matchId)
        {
            return (from bet in _db.Bets
                    where bet.Match == matchId && DateTime.Now.BrazilTimeZone() > bet.Match1.StartTime.AddHours(-1)
                    select new UserMatchPoints
                               {
                                   Bet = bet,
                                   Points = BetPoints(bet)
                               }
            )
            .ToList()
            .OrderByDescending(x => x.Points)
            .ThenBy(x => x.Bet.User1.UserName)
            .ToList();
        }

        public Match GetFirstMatch()
        {
            return _db.Matches.OrderBy(x => x.StartTime).FirstOrDefault();
        }

        public decimal GetTotalPrize()
        {
            return _db.Users.Where(x => x.Authorized && x.UserRoles.Any(y => y.Role.Name == BBRoles.Player))
                .Sum(x => x.PagSeguro ? ConfigurationHelper.Price - Math.Round((ConfigurationHelper.Price * (decimal)0.0499 + (decimal)0.4), 2) : ConfigurationHelper.Price);
        }

        private int GetTotalUserExactScores(User user, DateTime? untilDate = null)
        {
            untilDate = untilDate ?? DateTime.Now.AddHours(-1).BrazilTimeZone();

            var exactScores = user.Bets
                                .Where(bet => 
                                            bet.Match1.StartTime <= untilDate
                                            && bet.Score1.HasValue
                                            && bet.Score2.HasValue
                                            && bet.Match1.Score1.HasValue
                                            && bet.Match1.Score2.HasValue)
                                .Count(bet => PlacarExato(bet.Score1.Value, bet.Score2.Value, bet.Match1.Score1.Value, bet.Match1.Score2.Value));

            return exactScores;
        }

        public DateTime GetBonusBetExpireDate()
        {
            if (!_db.Matches.Any()) return DateTime.Now;

            var minDate = _db.Matches.Min(x => x.StartTime);
            minDate = minDate.AddHours(-1);
            return minDate;
        }

        public IEnumerable<MoneyDistribution> GetMoneyDistribution()
        {
            return _db.Users
                .Where(x => x.Authorized && x.UserRoles.Any(y => y.Role.Name == BBRoles.Player))
                .GroupBy(x => new { Holder = x.PagSeguro && x.AuthorizedBy != "PagSeguro" ? x.AuthorizedBy + " (PagSeguro)" : x.AuthorizedBy, x.PagSeguro })
                .Select(x => new MoneyDistribution { 
                            Holder = x.Key.Holder,
                            Amount = x.Sum(key => key.PagSeguro ? ConfigurationHelper.Price - Math.Round(ConfigurationHelper.Price * (decimal) 0.0499 + (decimal)0.4, 2) : ConfigurationHelper.Price),
                            TotalPlayers = x.Count()
                            })
                .OrderBy(x => x.Holder);
        }

        public MatchBetStatistic GetMatchBetStatistics(int matchId)
        {
            var bets = _db.Bets.Where(x => x.Match == matchId).ToList();

            double totalBets = bets.Count();

            if (totalBets <= 0) return null;

            var mostBetScore = bets
                                .GroupBy(x => new {x.Score1, x.Score2})
                                .Select(x => new {x.Key, qtd = x.Count()})
                                .OrderByDescending(x => x.qtd)
                                .Select(x => x.Key)
                                .First();

            return new MatchBetStatistic
                       {
                           Match = bets.First().Match1,
                           AverageScore1 = bets.Average(x => x.Score1).Value,
                           AverageScore2 = bets.Average(x => x.Score2).Value,
                           Score1MostBet = mostBetScore.Score1.Value,
                           Score2MostBet = mostBetScore.Score2.Value,
                           Team1Perc = bets.Count(x => x.Score1 > x.Score2) / totalBets,
                           Team2Perc = bets.Count(x => x.Score1 < x.Score2) / totalBets,
                           TiePerc = bets.Count(x => x.Score1 == x.Score2) / totalBets,
                        };
        }

        public BonusBetStatistic GetBonusBetStatistics(int bonusId)
        {
            var bets = _db.BonusBets.Where(x => x.Bonus == bonusId).ToList();

            double totalBets = bets.Count();

            var mostBetTeam =
                bets.GroupBy(x => new { x.Team1 }).Select(x => new { x.Key, qtd = x.Count() }).OrderByDescending(
                    x => x.qtd).Select(x => x.Key).First();
            
            return new BonusBetStatistic
            {
                Bonus = bets.First().Bonus11,
                Team = mostBetTeam.Team1,
                TeamPerc = bets.Count(x => x.Team1 == mostBetTeam.Team1) / totalBets,
            };
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}