using System;
using System.Linq;
using System.Web.Mvc;
using BigBallz.Core.Log;
using BigBallz.Models;
using BigBallz.Services;
using BigBallz.ViewModels;

namespace BigBallz.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BonusController : BaseController
    {
        private readonly ITeamService _teamService;
        private readonly IBonusService _bonusService;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly IBonusBetService _bonusBetService;
        private readonly IMailService _mailService;


        public BonusController(ITeamService teamService, IBonusService bonusService, IUserService userService, IAccountService accountService, IBonusBetService bonusBetService, IMailService mailService, IMatchService matchService, IBigBallzService bigBallzService) : base(userService, matchService, bigBallzService)
        {
            _teamService = teamService;
            _bonusService = bonusService;
            _userService = userService;
            _userService = userService;
            _accountService = accountService;
            _bonusBetService = bonusBetService;
            _mailService = mailService;
        }

        public ActionResult Index()
        {
            var bonusList = _bonusService.GetAll();
            return View(bonusList);
        }

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

                if (user.IsAdmin)
                {
                    var dbBonus = _bonusService.Get(bonus.BonusId);

                    TryUpdateModel(dbBonus, "bonus");

                    _bonusService.Save();

                    this.FlashInfo("Bônus alterado com sucesso");

                    return RedirectToAction("Index");
                }

                throw new Exception("Usuário não tem permissão para editar o bônus.");
            }
            catch(Exception ex)
            {
                Logger.Error(ex);

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
               Logger.Error(ex);

               this.FlashError(ex.Message);
               return RedirectToAction("index");
           }
          
       }
    }
}
