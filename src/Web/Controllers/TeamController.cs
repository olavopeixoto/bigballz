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
    public class TeamController : BaseController
    {
        private readonly ITeamService _teamService;
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;

        public TeamController(ITeamService teamService, IGroupService groupService, IUserService userService, IMatchService matchService, IBigBallzService bigBallzService) : base(userService, matchService, bigBallzService)
        {
            _teamService = teamService;
            _groupService = groupService;
            _userService = userService;
        }

        public ActionResult Index()
        {
            var teams = _teamService.GetAll().ToList();
            return View(teams);
        }

        public ActionResult List()
        {
            var teams = _teamService.GetAll();
            return Json(teams);
        }

        public ActionResult Create()
        {
            var model = new TeamViewModel
                            {
                                Groups = _groupService.GetAll().ToSelectList("GroupId", "Name")
                            };

            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Team team)
        {
            try
            {
                var user = _userService.Get(User.Identity.Name);

                //Verifica se usuario tem papel de admin
                if (user.UserRoles.Count > 1)
                {
                    _teamService.Add(team);
                    _teamService.Save();
                    return RedirectToAction("Index");
                }

                throw new Exception("Usuário não tem permissão para cadastrar times.");
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
                this.FlashError(ex.Message);
                //ModelState.AddModelErrors(game.GetRuleViolations());
                var model = new TeamViewModel
                {
                    Groups = _groupService.GetAll().ToSelectList("GroupId", "Name")
                };

                return View(model);
            }

        }

        public ActionResult Edit(string id)
        {
            ViewData["Groups"] = _groupService.GetAll().ToSelectList("GroupId", "Name");

            return View(_teamService.Get(id));
        }

        [HttpPost]
        public ActionResult Edit(Team team)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["Groups"] = _groupService.GetAll().ToSelectList("GroupId", "Name");
                    return View(_teamService.Get(team.TeamId));
                }

                var dbmatch = _teamService.Get(team.TeamId);
                TryUpdateModel(dbmatch, "team");
                _teamService.Save();
                
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
                this.FlashError(ex.Message);

                ViewData["Groups"] = _groupService.GetAll().ToSelectList("GroupId", "Name");

                return View(_teamService.Get(team.TeamId));
            }
        }
    }
}
