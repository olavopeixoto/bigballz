using System;
using System.Linq;
using System.Web.Mvc;
using BigBallz.Models;
using BigBallz.Services;
using BigBallz.Services.L2S;
using BigBallz.ViewModels;

namespace BigBallz.Controllers
{
    [Authorize(Roles = "admin")]
    public class MatchController : BaseController
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

        public MatchController()
        {
            _teamService = new TeamService();
            _stageService = new StageService();
            _matchService = new MatchService();
            _userService = new UserService();
        }


        // GET: /Team

        public ActionResult Index()
        {
            var matches = _matchService.GetAll().OrderBy(x => x.StageId).ThenBy(x => x.Team1.GroupId).ThenBy(x => x.MatchId);
            return View(matches);
        }

        ////
        //// GET: /Team/Details/5

        //public ActionResult Details(int id)
        //{
        //    var match = _matchService.Get(id);

        //    if (match == null)
        //    {
        //        return View("NotFound");
        //    }
        //    return View(match);
        //}

        ////
        //// GET: /Team/Create

        public ActionResult Create()
        {
            var model = new MatchViewModel
                            {
                                Match = new Match(),
                                Teams = _teamService.GetAll().ToSelectList("TeamId", "Name"),
                                Stages = _stageService.GetAll().ToSelectList("StageId", "Name")
                            };

            model.Match.StartTime = new DateTime(2010, 6, 11, 11, 00, 00);
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
                    _matchService.Save();
                    return RedirectToAction("Index");
                }

                throw new Exception("Usuário não tem permissão para cadastrar jogos.");
            }
            catch(Exception ex)
            {
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

        //private void GetAllTeams(string teamAId, string teamBId)
        //{
        //    var teams = _teamService.GetAll();

        //    if (!string.IsNullOrEmpty(teamAId))
        //        ViewData["TeamsA"] = new SelectList(teams, "Id", "Name", teamAId);
        //    else
        //        ViewData["TeamsA"] = new SelectList(teams, "Id", "Name");

        //    if (!string.IsNullOrEmpty(teamBId))
        //        ViewData["TeamsB"] = new SelectList(teams, "Id", "Name", teamBId);
        //    else
        //        ViewData["TeamsB"] = new SelectList(teams, "Id", "Name");
        //}

        //private void GetAllStages(int? stageId)
        //{
        //    var stages = _stageService.GetAll();

        //    if (stageId != null && stageId > 0)
        //        ViewData["Stages"] = new SelectList(stages, "Id", "Name", stageId);
        //    else
        //        ViewData["Stages"] = new SelectList(stages, "Id", "Name");
        //}

        //
        // GET: /Team/Edit/5

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

        //
        // POST: /Team/Edit/5

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
