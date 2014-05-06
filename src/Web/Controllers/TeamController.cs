using System;
using System.Web.Mvc;
using BigBallz.Models;
using BigBallz.Services;
using BigBallz.ViewModels;

namespace BigBallz.Controllers
{
    [Authorize(Roles = "admin")]
    public class TeamController : SecureBaseController
    {
        private readonly ITeamService _teamService;
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;

        public TeamController(ITeamService teamService, IGroupService groupService, IUserService userService)
        {
            _teamService = teamService;
            _groupService = groupService;
            _userService = userService;
        }

        public TeamController()
        {
            _teamService = new TeamService();
            _groupService = new GroupService();
            _userService = new UserService();
        }

        // GET: /Team
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            var teams = _teamService.GetAll();
            return Json(teams);
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
            var model = new TeamViewModel
                            {
                                Groups = _groupService.GetAll().ToSelectList("GroupId", "Name")
                            };

            return View(model);
        }

        //
        // POST: /Team/Create

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
                this.FlashError(ex.Message);
                //ModelState.AddModelErrors(game.GetRuleViolations());
                var model = new TeamViewModel
                {
                    Groups = _groupService.GetAll().ToSelectList("GroupId", "Name")
                };

                return View(model);
            }

        }

        //
        // GET: /Team/Edit/5

        public ActionResult Edit(string id)
        {
            ViewData["Groups"] = _groupService.GetAll().ToSelectList("GroupId", "Name");

            return View(_teamService.Get(id));
        }

        //
        // POST: /Team/Edit/5

        [HttpPost]
        public ActionResult Edit(Team team)
        {
            try
            {
                var user = _userService.Get(User.Identity.Name);

                //Verifica se usuario tem papel de admin
                if (user.UserRoles.Count == 0)
                {
                    throw new Exception("Usuário não tem permissão para editar jogos.");
                }

                var dbmatch = _teamService.Get(team.TeamId);
                TryUpdateModel(dbmatch, "team");
                _teamService.Save();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                this.FlashError(ex.Message);

                ViewData["Groups"] = _groupService.GetAll().ToSelectList("GroupId", "Name");

                return View(_teamService.Get(team.TeamId));
            }
        }
    }
}
