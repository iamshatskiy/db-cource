using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.Logic;
using купикота.рф.Data.Models;
using купикота.рф.Models;
using купикота.рф.Models.FeedbackViewModels;

namespace купикота.рф.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly UserLogic _userLogic;
        private readonly FeedbackLogic _feedbacks;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public FeedbackController(UserLogic userLogic, FeedbackLogic feedbacks,
            UserManager<ApplicationUser> userManager, ILogger<DealController> logger)
        {
            _userLogic = userLogic;
            _feedbacks = feedbacks;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize(Roles = "admin,moderator")]
        public IActionResult Index()
        {
            var feeds = _feedbacks.AllFeeds();

            List<FeedbackViewModel> TrueFeeds = new List<FeedbackViewModel>();

            foreach (var f in feeds)
            {
                FeedbackViewModel CurFeed = new FeedbackViewModel
                {
                    BuyerId = f.BuyerId,
                    OwnerId = f.OwnerId,
                    OwnerName = _userLogic.FindUserNameById(f.OwnerId),
                    BuyerName = _userLogic.FindUserNameById(f.BuyerId),
                    FbDate = f.FbDate,
                    Rate = f.Rate,
                    Id = f.Id,
                    Feedback = f.Feedback
                };
                TrueFeeds.Add(CurFeed);
            }

            return View(TrueFeeds);
        }

        [Authorize]
        public IActionResult PersonalIndex()
        {
            var CurUser = _userManager.GetUserAsync(User).Result;
            var feeds = _feedbacks.GetOwnerFeeds(_userLogic.GetUserId(CurUser));

            List<FeedbackViewModel> TrueFeeds = new List<FeedbackViewModel>();

            foreach (var f in feeds)
            {
                if (f.Rate != 0 && f.Feedback != "default")
                {
                    FeedbackViewModel CurFeed = new FeedbackViewModel
                    {
                        BuyerId = f.BuyerId,
                        OwnerName = _userLogic.FindUserNameById(f.OwnerId),
                        BuyerName = _userLogic.FindUserNameById(f.BuyerId),
                        FbDate = f.FbDate,
                        Rate = f.Rate,
                        Id = f.Id,
                        Feedback = f.Feedback
                    };
                    TrueFeeds.Add(CurFeed);
                }
            }
            _logger.LogInformation("Получены данные об отзывах пользователем");
            return View(TrueFeeds);
        }


        [HttpPost]
        public IActionResult OwnerIndex(string OwnerId)
        {
            var feeds = _feedbacks.GetOwnerFeeds(OwnerId);

            List<FeedbackViewModel> TrueFeeds = new List<FeedbackViewModel>();

            foreach (var f in feeds)
            {
                if (f.Feedback != null)
                {
                    FeedbackViewModel CurFeed = new FeedbackViewModel
                    {
                        BuyerId = f.BuyerId,
                        BuyerName = _userLogic.FindUserNameById(f.BuyerId),
                        FbDate = f.FbDate,
                        Rate = f.Rate,
                        Id = f.Id,
                        Feedback = f.Feedback
                    };
                    TrueFeeds.Add(CurFeed);
                }
            }

            _logger.LogInformation("Получены данные об отзывах пользователя с id = %s", OwnerId);

            return View(TrueFeeds);
        }

        [Authorize]
        public IActionResult NeedCommentIndex()
        {
            var CurUser = _userManager.GetUserAsync(User).Result;
            var feeds = _feedbacks.NullFeeds();

            List<FeedbackViewModel> TrueFeeds = new List<FeedbackViewModel>();

            foreach (var f in feeds)
            {
                if (f.BuyerId == _userLogic.GetUserId(CurUser) && f.Feedback == "default")
                {
                    FeedbackViewModel CurFeed = new FeedbackViewModel
                    {
                        BuyerId = f.BuyerId,
                        OwnerId = f.OwnerId,
                        OwnerName = _userLogic.FindUserNameById(f.OwnerId),
                        BuyerName = _userLogic.FindUserNameById(f.BuyerId),
                        FbDate = f.FbDate,
                        Rate = f.Rate,
                        Id = f.Id,
                        Feedback = f.Feedback
                    };
                    TrueFeeds.Add(CurFeed);
                }
            }
            _logger.LogInformation("Получение нулевых отзывов текущем пользователем");
            return View(TrueFeeds);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create(int fid)
        {
            var feed = _feedbacks.GetFeedbackById(fid);
            if (feed != null)
            {
                var model = new FeedbackViewModel
                {
                    Id = fid,
                    OwnerId = feed.OwnerId,
                    BuyerId = feed.BuyerId,
                    OwnerName = _userLogic.FindUserNameById(feed.OwnerId)
                };
                
                return View(model);
            }
            _logger.LogWarning("Не удалось получить объект отзыва с id = %d", fid);
            return RedirectToAction("Index", "Adverts");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(FeedbackViewModel model)
        {
            if (ModelState.IsValid)
            {
                Feedbacks UpFeed = new Feedbacks {
                    Id = model.Id,
                    OwnerId = model.OwnerId,
                    BuyerId = model.BuyerId,
                    Feedback = model.Feedback,
                    Rate = model.Rate
                };
                _feedbacks.UpdateFeed(UpFeed);
                _logger.LogInformation("Успешное изменение отзыва с id = %d", UpFeed.Id);
                return RedirectToAction("NeedCommentIndex", "Feedback");
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles ="admin,moderator")]
        public IActionResult Delete(int fid)
        {
            var feed = _feedbacks.GetFeedbackById(fid);
            if (feed != null)
            {
                _feedbacks.DeleteFeed(fid);
                _logger.LogInformation("Успешное удаление отзыва с id = %d", fid);
            }
            _logger.LogWarning("Проблема с удалением отзыва с id = %d", fid);
            return RedirectToAction("Index", "Feedback");
        }
    }
    
}