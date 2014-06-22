using System;
using System.Linq;
using System.Web.Mvc;
using BigBallz.Core.Log;
using BigBallz.Models;
using BigBallz.Services;
using BigBallz.ViewModels;

namespace BigBallz.Controllers
{
    [Authorize(Roles = "admin")]
    public class MatchController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly IStageService _stageService;
        private readonly IMatchService _matchService;
        private readonly IUserService _userService;

        public MatchController(ITeamService teamService, IStageService stageService, IMatchService matchService, IUserService userService)
        {
            _teamService = teamService;
            _stageService = stageService;
            _matchService = matchService;
            _userService = userService;
        }

        public ActionResult Index()
        {
            var matches = _matchService.GetAll().OrderBy(x => x.StageId).ThenBy(x => x.Team1.GroupId).ThenBy(x => x.MatchId);
            return View(matches);
        }

        public ActionResult Create()
        {
            var model = new MatchViewModel
                            {
                                Match = new Match(),
                                Teams = _teamService.GetAll().ToSelectList("TeamId", "Name"),
                                Stages = _stageService.GetAll().ToSelectList("StageId", "Name")
                            };

            model.Match.StartTime = DateTime.Today;
            return View(model);
        }

        //
        // POST: /Team/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Match match)
        {
            try
            {
                var user = _userService.Get(User.Identity.Name);

                //Verifica se usuario tem papel de admin
                if (user.UserRoles.Count > 1)
                {
                    _matchService.Add(match);
                    return RedirectToAction("Index");
                }

                throw new Exception("Usuário não tem permissão para cadastrar jogos.");
            }
            catch(Exception ex)
            {
                Logger.Error(ex);

                this.FlashError(ex.Message);
                //ModelState.AddModelErrors(game.GetRuleViolations());
                var model = new MatchViewModel
                {
                    Match = match,
                    Teams = _teamService.GetAll().ToSelectList("TeamId", "Name"),
                    Stages = _stageService.GetAll().ToSelectList("StageId", "Name")
                };

                return View(model);
            }

        }

        public ActionResult Edit(int id)
        {
            var model = new MatchViewModel
            {
                Match = _matchService.Get(id),
                Teams = _teamService.GetAll().ToSelectList("TeamId", "Name"),
                Stages = _stageService.GetAll().ToSelectList("StageId", "Name")
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Match match)
        {
            try
            {
                var user = _userService.Get(User.Identity.Name);

                //Verifica se usuario tem papel de admin
                if (user.UserRoles.Count == 0)
                {
                    throw new Exception("Usuário não tem permissão para editar jogos.");
                }

                var dbmatch = _matchService.Get(match.MatchId);
                TryUpdateModel(dbmatch, "match");
                _matchService.Save();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
                this.FlashError(ex.Message);
                var model = new MatchViewModel
                {
                    Match = match,
                    Teams = _teamService.GetAll().ToSelectList("TeamId", "Name"),
                    Stages = _stageService.GetAll().ToSelectList("StageId", "Name")
                };

                return View(model);
            }
        }
    }
}
