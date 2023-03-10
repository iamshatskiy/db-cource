using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using купикота.рф.Data.Logic;
using купикота.рф.Models.DealViewModels;

namespace купикота.рф.Controllers
{
    [Authorize(Roles = "admin")]
    public class DealHistoryController : Controller
    {
        private readonly DealHistoryLogic _dealHistoryLogic;

        public DealHistoryController (DealHistoryLogic dealHistoryLogic)
        {
            _dealHistoryLogic = dealHistoryLogic;
        }

        public IActionResult Index()
        {
            var history = _dealHistoryLogic.GetAllHistory();

            List<DealHistoryViewModel> model = new List<DealHistoryViewModel>();

            foreach (var h in history)
            {
                var CurHistory = new DealHistoryViewModel {
                    Id = h.Id,
                    BuyerId = h.BuyerId,
                    OwnerId = h.OwnerId,
                    IsConfirmed = h.IsConfirmed
                };
                model.Add(CurHistory);
            }

            return View(model);
        }
    }
}