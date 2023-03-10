using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using купикота.рф.Data.Logic;
using купикота.рф.Data.Models;
using купикота.рф.Models;
using купикота.рф.Models.AdvertViewModels;

namespace купикота.рф.Controllers
{
    public class DealController : Controller
    {
        private readonly UserLogic _userLogic;
        private readonly DealLogic _dealLogic;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public DealController(UserLogic userLogic, DealLogic dealLogic, 
            UserManager<ApplicationUser> userManager, ILogger<DealController> logger)
        {
            _userLogic = userLogic;
            _dealLogic = dealLogic;
            _userManager = userManager;
            _logger = logger;
        }
        [Authorize]
        public async Task<IActionResult> IncomingIndex()
        {
            var CurUser = await _userManager.GetUserAsync(User);

            var deals = _dealLogic.GetIncomingDeals(CurUser.Id);

            List<DealViewModel> model = new List<DealViewModel>();

            foreach (var d in deals)
            {
                if (d.IsConfirmedByOWner == 0 && d.IsConfirmedByBuyer == 0)
                {
                    var CurDeal = new DealViewModel
                    {
                        Id = d.Id,
                        AdvertId = d.AdvertId,
                        OwnerId = d.OwnerId,
                        BuyerId = d.BuyerId,
                        BuyerEmail = _userLogic.FindUserEmailById(d.BuyerId),
                        BuyerName = _userLogic.FindUserNameById(d.BuyerId),
                        BuyerNum = _userLogic.FindUserNumById(CurUser),
                        ConfByBuyer = d.IsConfirmedByBuyer,
                        ConfByOWner = d.IsConfirmedByOWner
                    };
                    model.Add(CurDeal);
                }
            }
            _logger.LogInformation("Вызов входящих сделок пользователем с id = %s", CurUser.Id);

            return View(model);
        }

        public async Task<IActionResult> OutgoingIndex()
        {
            var CurUser = await _userManager.GetUserAsync(User);

            var deals = _dealLogic.GetOutgoingDeals(CurUser.Id);

            List<DealViewModel> model = new List<DealViewModel>();

            foreach (var d in deals)
            {
                if (d.IsConfirmedByOWner == 1 && d.IsConfirmedByBuyer == 0)
                {
                    var CurDeal = new DealViewModel
                    {
                        Id = d.Id,
                        AdvertId = d.AdvertId,
                        OwnerId = d.OwnerId,
                        BuyerId = d.BuyerId,
                        OwnerEmail = _userLogic.FindUserEmailById(d.OwnerId),
                        OwnerName = _userLogic.FindUserNameById(d.OwnerId),
                        OwnerNum = _userLogic.FindUserNumById(CurUser),
                        ConfByBuyer = d.IsConfirmedByBuyer,
                        ConfByOWner = d.IsConfirmedByOWner
                    };
                    model.Add(CurDeal);
                }
            }

            _logger.LogInformation("Вызов исходящих сделок пользователем с id = %s", CurUser.Id);

            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(int aid, string OwnerId)
        {
            if (aid > 0 || OwnerId != null)
            {
                var CurUser = await _userManager.GetUserAsync(User);
                var model = new Deals {
                    AdvertId = aid,
                    OwnerId = OwnerId,
                    BuyerId = _userLogic.GetUserId(CurUser)
                };
                _dealLogic.CreateDeal(model);
                _logger.LogInformation("Создана новая сделка пользователем с id = %s", CurUser.Id);
            }

            return RedirectToAction("Index", "Adverts");
        }

        [Authorize]
        [HttpPost]
        public IActionResult OwnerConformitionTrue(int DealId)
        {
            _dealLogic.OwnerConformition(DealId, 1);
            _logger.LogInformation("Сделка id = %d была принята продавцом.", DealId);
            return RedirectToAction("IncomingIndex", "Deal");
        }

        [Authorize]
        [HttpPost]
        public IActionResult OwnerConformitionFalse(int DealId)
        {
            _dealLogic.OwnerConformition(DealId, -1);

            _logger.LogWarning("Сделка id = %d была отклонена продавцом.", DealId);

            return RedirectToAction("IncomingIndex", "Deal");
        }

        [Authorize]
        [HttpPost]
        public IActionResult BuyerConformition(int DealId, sbyte state)
        {
            _dealLogic.BuyerConformition(DealId, state);
            _logger.LogInformation("Заключена новая сделка.");
            return RedirectToAction("OutgoingIndex", "Deal");
        }
    }
}