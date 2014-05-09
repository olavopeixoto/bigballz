using System;
using System.Linq;
using System.Web.Mvc;
using BigBallz.Models;
using BigBallz.Services;
using BigBallz.Services.L2S;
using BigBallz.ViewModels;

namespace BigBallz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BonusController : SecureBaseController
    {
        private readonly ITeamService _teamService;
        private readonly IBonusService _bonusService;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly IMailService _mailService;
        private readonly IBonusBetService _bonusBetService;


        public BonusController(ITeamService teamService, IBonusService bonusService, IUserService userService, IMailService mailService, IAccountService accountService, IBonusBetService bonusBetService)
        {
            _teamService = teamService;
            _bonusService = bonusService;
            _userService = userService;
            _userService = userService;
            _accountService = accountService;
            _bonusBetService = bonusBetService;
        }

        public BonusController()
        {
            _teamService = new TeamService();
            _bonusService = new BonusService();
            _userService = new UserService();
            _userService = new UserService();
            _mailService = new MailService();
            _accountService = new AccountService();
            _bonusBetService = new BonusBetService();
        }


        // GET: /Team

        public ActionResult Index()
        {
            var bonusList = _bonusService.GetAll();
            return View(bonusList);
        }

        ////
        //// GET: /Team/Details/5

        //public ActionResult Details(int id)
        //{
        //    var Bonus = _BonusService.Get(id);

        //    if (Bonus == null)
        //    {
        //        return View("NotFound");
        //    }
        //    return View(Bonus);
        //}

        ////
        //// GET: /Team/Create

        public ActionResult Create()
        {
            var model = new BonusViewModel
                            {
                                Bonus = new Bonus(),
                                Teams = _teamService.GetAll().ToSelectList("TeamId", "Name"),                                
                            };
           
            return View(model);
        }

        //
        // POST: /Team/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Bonus bonus)
        {
            try
            {
                var user = _userService.Get(User.Identity.Name);

                //Verifica se usuario tem papel de admin
                if (user.UserRoles.Count == 0)
                {
                    throw new Exception("Usuário não tem permissão para cadastrar jogos.");
                }

                _bonusService.Add(bonus);
                _bonusService.Save();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                this.FlashError(ex.Message);
              
                var model = new BonusViewModel
                {
                    Bonus = bonus,
                    Teams = _teamService.GetAll().ToSelectList("TeamId", "Name"),
                   
                };

                return View(model);
            }

        }

      

        //
        // GET: /Team/Edit/5

        public ActionResult Edit(int id)
       { 
            var model = new BonusViewModel
            {
                Bonus = _bonusService.Get(id)                
            };

            if (model.Bonus.Group != null)
            {
                var groupId = (int)model.Bonus.Group;
                model.Teams = _teamService.GetAll(groupId).ToSelectList("TeamId", "Name");
            }
            else
            {
                model.Teams = _teamService.GetAll().ToSelectList("TeamId", "Name");
            }


            return View(model);
        }

        //
        // POST: /Team/Edit/5

       [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Bonus bonus)
        {
            try
            {
                var user = _userService.Get(User.Identity.Name);

                //Verifica se usuario tem papel de admin
                if (user.UserRoles.Count > 1)
                {
                    var dbBonus = _bonusService.Get(bonus.BonusId);
                    TryUpdateModel(dbBonus, "bonus");


                    _bonusService.Save();
                    this.FlashInfo("Usuário alterado com sucesso");
                    return RedirectToAction("Index");
                }

                throw new Exception("Usuário não tem permissão para editar jogos.");

               
            }
            catch(Exception ex)
            {
                this.FlashError(ex.Message);
                var model = new BonusViewModel
                {
                    Bonus = bonus,
                    Teams = _teamService.GetAll().ToSelectList("TeamId", "Name"),                   
                };

                return View(model);
            }
        }

       public ActionResult SendBonusEmail()
       {
           try
           {
               foreach (var user in _accountService.GetAllPlayers())
               {
                   _mailService.SendEndBonusAlert(user, _bonusBetService.GetAll().ToList());
               }
               
               this.FlashInfo("Email enviado com sucesso.");

               return RedirectToAction("index");
           }
           catch (Exception ex)
           {
               this.FlashError(ex.Message);
               return RedirectToAction("index");
           }
          
       }
    }
}
