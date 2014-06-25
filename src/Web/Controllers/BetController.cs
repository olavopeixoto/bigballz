using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BigBallz.Core;
using BigBallz.Core.Log;
using BigBallz.Filters;
using BigBallz.Helpers;
using BigBallz.Models;
using BigBallz.Services;
using BigBallz.ViewModels;

namespace BigBallz.Controllers
{
    public class BetController : BaseController
    {
        private readonly IBonusBetService _bonusBetService;
        private readonly IBonusService _bonusService;
        private readonly IMatchBetService _matchBetService;
        private readonly IMatchService _matchService;
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;
        private readonly IBigBallzService _bigBallzService;

        public BetController(ITeamService teamService, IBonusService bonusService, IBonusBetService bonusBetService,
                             IUserService userService, IMatchService matchService, IMatchBetService matchBetService, IBigBallzService bigBallzService) : base(userService, matchService, bigBallzService)
        {
            _teamService = teamService;
            _bonusService = bonusService;
            _bonusBetService = bonusBetService;
            _userService = userService;
            _matchService = matchService;
            _matchBetService = matchBetService;
            _bigBallzService = bigBallzService;
        }

        private void ShowRemindMessage()
        {
            var startDate = _matchService.GetStartDate();

            var now = DateTime.Now.BrazilTimeZone();
            if (User.Identity.IsAuthenticated && now <= startDate && Convert.ToBoolean(Request.Cookies["betremindershown"].NullSafe(x => x.Value, Boolean.FalseString)))
            {
                this.FlashWarning("Falta(m) " + ((int) (startDate - now).TotalDays) +
                                  " dia(s) para começar o campeonato. Não se esqueça de apostar no BONUS e nos primeiros jogos!!!");
                
                var reminderCookie = new HttpCookie("betremindershown", Boolean.TrueString)
                {
                    Expires = DateTime.Now.BrazilTimeZone().AddDays(5)
                };
                Response.Cookies.Add(reminderCookie);
            }
        }

        [UserNameFilter]
        public ActionResult Index(string userName)
        {
            ShowRemindMessage();

            var matches = _matchService
                                .GetAll()
                                .OrderBy(x => x.StartTime)
                                .ToList();

            var endBonus = _bigBallzService.GetBonusBetExpireDate();

            var bonusEnded = DateTime.Now.BrazilTimeZone() > endBonus;

            var user = _userService.Get(userName);

            var bonusBetViewModel = new BetViewModel
                                        {
                                            ShowHelp = !user.HelpShown,
                                            BonusEnabled = !bonusEnded,
                                            BonusList = (from bonus in _bonusService.GetAll().ToList()
                                                         let cupStartDate = _bigBallzService.GetFirstMatch().StartTime
                                                         let teams = _teamService.GetAll().ToList()
                                                         let bonusBets = _bonusBetService.GetAll(user.UserId).ToList()
                                                         let pointsEarned = _bigBallzService.GetUserPointsByBonus(user)
                                                         let bonusBetStatistic = bonusEnded ? _bigBallzService.GetBonusBetStatistics(bonus.BonusId) : new BonusBetStatistic()
                                                         select new BetViewModel.BonusTeams
                                                                    {
                                                                        BonusBetId = bonusBets.Where(x => x.Bonus == bonus.BonusId).Select(x => x.BonusBetId).FirstOrDefault(),
                                                                        BonusBet = bonusBets.FirstOrDefault(x => x.Bonus == bonus.BonusId),
                                                                        Bonus = bonus,
                                                                        CupStartDate = cupStartDate,
                                                                        PointsEarned = pointsEarned.Where(x => x.BonusBet.Bonus == bonus.BonusId).Sum(x => x.Points),
                                                                        Enabled = cupStartDate.Subtract(new TimeSpan(0, 1, 0, 0)).Subtract(DateTime.Now.BrazilTimeZone()).TotalMilliseconds > 0,
                                                                        Teams = bonus.Group.HasValue
                                                                                ? new SelectList(
                                                                                      teams.Where(x => x.GroupId == bonus.Group),
                                                                                      "TeamId", "Name",
                                                                                      bonusBets.Where(x => x.Bonus == bonus.BonusId)
                                                                                                      .Select(x => x.Team)
                                                                                                      .FirstOrDefault())
                                                                                : new SelectList(teams, "TeamId", "Name",
                                                                                                 bonusBets.Where(x => x.Bonus == bonus.BonusId)
                                                                                                    .Select(x => x.Team)
                                                                                                    .FirstOrDefault()),
                                                                        BonusBetStatistic = bonusBetStatistic
                                                                    }).ToList(),

                                            BetList = (from match in matches
                                                       let matchBets = user.Bets
                                                       let pointsEarned = _bigBallzService.GetUserPointsByMatch(user)
                                                       select new BetViewModel.BetMatches
                                                                    {
                                                                        PointsEarned = pointsEarned.Where(x => x.Bet.Match == match.MatchId).Sum(x => x.Points),
                                                                        Enabled = match.StartTime.Subtract(new TimeSpan(0,1,0,0)).Subtract(DateTime.Now.BrazilTimeZone()).TotalMilliseconds > 0,
                                                                        Bet = matchBets.FirstOrDefault(x => x.Match == match.MatchId),
                                                                        Match = match
                                                                    }).ToList()
                                        };

            //Verifica se usuario está autorizado
            if (!user.Authorized)
            {
                this.FlashWarning("Favor efetuar o pagamento e aguardar a autorização para realizar as apostas.");
            }

            if (!user.HelpShown)
            {
                user.HelpShown = true;
                _userService.Save();
            }

            return View(bonusBetViewModel);
        }

        public ActionResult Expired(int id)
        {
            var user = _userService.Get(id);
            var matches = _matchService.GetAll().Where(x => x.StartTime.AddHours(-1) < DateTime.Now.BrazilTimeZone()).OrderByDescending(x => x.StartTime).ToList();
            var bonusEnded = DateTime.Now.BrazilTimeZone() > _bigBallzService.GetBonusBetExpireDate();
            var bonusBetViewModel = new BetViewModel
                                        {
                                            UserName = user.UserName,

                                            BonusList = (from bonus in _bonusService.GetAll().ToList()
                                                         let bonusBets = bonusEnded ? user.BonusBets.ToList() : new List<BonusBet>()
                                                         let pointsEarned = _bigBallzService.GetUserPointsByBonus(user)
                                                         let bonusBetStatistic = bonusEnded ? _bigBallzService.GetBonusBetStatistics(bonus.BonusId) : new BonusBetStatistic()
                                                         select new BetViewModel.BonusTeams
                                                         {
                                                             BonusBet = bonusBets.FirstOrDefault(x => x.Bonus == bonus.BonusId),
                                                             Bonus = bonus,
                                                             PointsEarned = pointsEarned.FirstOrDefault(x => x.BonusBet.Bonus == bonus.BonusId && !string.IsNullOrEmpty(x.BonusBet.Bonus11.Team)).NullSafe(x => x.Points),
                                                             BonusBetStatistic = bonusBetStatistic
                                                         }).ToList(),

                                            BetList = (from match in matches
                                                       let matchBets = _matchBetService.GetAllExpired(id).ToList()
                                                       let pointsEarned = _bigBallzService.GetUserPointsByMatch(user)
                                                       select new BetViewModel.BetMatches
                                                       {
                                                           PointsEarned = pointsEarned.FirstOrDefault(x => x.Bet.Match == match.MatchId).NullSafe(x => x.Points),
                                                           Bet = matchBets.FirstOrDefault(x => x.Match == match.MatchId),
                                                           Match = match
                                                       }).ToList()
                                        };

            return View(bonusBetViewModel);
        }

        public ActionResult MatchBets(int id)
        {
            var matchStatistics = _bigBallzService.GetMatchBetStatistics(id);

            if (matchStatistics == null || matchStatistics.Match.StartTime.AddHours(-1) > DateTime.Now.BrazilTimeZone())
            {
                this.FlashWarning("Partida ainda não disponível para acompanhamento de resultados.");
                return RedirectToAction("index");
            }

            var usersMatchPoints = _bigBallzService.GetUserPointsByExpiredMatch(id);

            var model = new MatchBetsViewModel
                            {
                                Match = matchStatistics.Match,
                                Statistics = matchStatistics,
                                UsersMatchPoints = usersMatchPoints
                            };

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post), UserNameFilter]
        public ActionResult SaveBonus(string userName, IList<BonusBet> bonusBet)
        {
            try
            {
                var user = _userService.Get(userName);

                //Verifica se usuario está autorizado
                if (!user.Authorized)
                {
                    this.FlashWarning("Favor efetuar o pagamento e aguardar a autorização para realizar as apostas.");
                    return RedirectToAction("Index");
                }

                var firstMatch = _bigBallzService.GetFirstMatch();
                if (firstMatch != null && firstMatch.StartTime.AddHours(-1) <= DateTime.Now.BrazilTimeZone())
                {
                    this.FlashWarning("Apostas Bonus Encerradas!");
                    return RedirectToAction("Index");
                }

                foreach (var bet in user.BonusBets)
                {
                    _bonusBetService.Delete(bet);
                }

                user.BonusBets.Clear();

                foreach (var bet in bonusBet.Where(x => !string.IsNullOrEmpty(x.Team)))
                {
                    bet.User = user.UserId;
                    bet.User1 = user;
                    user.BonusBets.Add(bet);
                    _bonusBetService.Add(bet);
                }

                _bonusBetService.Save();

                this.FlashInfo("Apostas Cadastradas Com Sucesso!");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.FlashError(ex.Message);
                return RedirectToAction("Index");
            }
        }

        [AcceptVerbs(HttpVerbs.Post), UserNameFilter]
        public ActionResult SaveBet(string userName, IList<Bet> bets)
        {
            try
            {
                //Verifica se usuario está autorizado
                if (!this.IsAuthorized())
                {
                    this.FlashWarning("Favor efetuar o pagamento e aguardar a autorização para realizar as apostas.");
                    return RedirectToAction("Index");
                }

                var erroStringBuilder = new StringBuilder();
                foreach (var bet in bets.Where(x => x.Score1.HasValue && x.Score2.HasValue))
                {
                    var userBet = bet;
                    if (bet.BetId > 0)
                    {
                        userBet = _matchBetService.Get(bet.BetId);
                        if (userBet.Match1.StartTime.AddHours(-1) <= DateTime.Now.BrazilTimeZone())
                        {
                            erroStringBuilder.AppendLine(
                                string.Format("Aposta para a partida {0} X {1} já está encerrada",
                                    userBet.Match1.Team1.Name, userBet.Match1.Team2.Name));
                            continue;
                        }
                        userBet.Score1 = bet.Score1;
                        userBet.Score2 = bet.Score2;
                    }
                    else
                    {
                        var userId = _userService.Get(userName).UserId;
                        userBet.User = userId;
                        try
                        {
                            _matchBetService.Add(userBet);
                        }
                        catch (ValidationException ex)
                        {
                            erroStringBuilder.AppendLine(ex.Message);
                        }
                    }
                }
                _matchBetService.Save();

                if (string.IsNullOrEmpty(erroStringBuilder.ToString()))
                {
                    this.FlashInfo("Apostas Cadastradas Com Sucesso!");
                }
                else
                {
                    this.FlashWarning(erroStringBuilder.ToString());
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.FlashError(ex.Message);
                return RedirectToAction("Index");
            }
        }
    }
}