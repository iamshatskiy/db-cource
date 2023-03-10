using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.Logic;
using купикота.рф.Data.Models;
using купикота.рф.Models;
using купикота.рф.Models.AdvertViewModels;

namespace купикота.рф.Controllers
{
    public class AdvertsController : Controller
    {

        private readonly AdvertLogic _advertLogic;
        private readonly PhotoLogic _photoLogic;
        private readonly UserLogic _userLogic;
        private readonly BreedLogic _breedLogic;
        private readonly HideLogic _hideLogic;
        private readonly FeedbackLogic _feedbackLogic;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;


        public AdvertsController(AdvertLogic advertLogic, PhotoLogic photoLogic, UserLogic userLogic, BreedLogic breedLogic,
            HideLogic hideLogic, FeedbackLogic feedbackLogic, UserManager<ApplicationUser> userManager, ILogger<AdvertsController> logger)
        {
            _photoLogic = photoLogic;
            _advertLogic = advertLogic;
            _userLogic = userLogic;
            _breedLogic = breedLogic;
            _hideLogic = hideLogic;
            _feedbackLogic = feedbackLogic;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Create()
        {   
            ViewBag.breed = _breedLogic.GetAllBreeds();
            var model = new AdvertViewModel{};
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(AdvertViewModel advert, IFormFile pic)
        {

            if (ModelState.IsValid && pic != null)
            {
                int success_photo = _photoLogic.PhotoCreate(pic);
                if (success_photo == -1)
                {
                    _logger.LogError("Попытка загрузки файла завершилась ошибкой. Повторая отправка в Create.");
                    ViewBag.breed = _breedLogic.GetAllBreeds();
                    return View(advert);
                }
                else if (success_photo == 1)
                {
                    advert.PhotoId = _photoLogic.GetLastPhotoId();
                    advert.Photo_name = _photoLogic.GetLastPhotoName();
                    _logger.LogInformation("Успешная загрузка файла. Id = %d.", advert.PhotoId);
                }


                var user = _userManager.GetUserAsync(User).Result;

                advert.UserId = _userLogic.GetUserId(user);

                _advertLogic.AddNewAdvert(advert);
                _logger.LogInformation("Успешное создание объявление пользователем с id %s. Id загруженного файла = %d.", 
                    advert.UserId,advert.PhotoId);
                return RedirectToAction("Index", "Adverts");
            }


            var model = new AdvertViewModel
            {
                Age = advert.Age,
                BreedId = advert.BreedId,
                Descr = advert.Descr,
                Price = advert.Price,
                Photo = advert.Photo,
                Photo_name = advert.Photo_name

            };
            ViewBag.breed = _breedLogic.GetAllBreeds();
            return View(model);
        }

        IEnumerable<AdvertIndexView> GetTrueAdverts(List<cat_advert> adverts, bool is_person)
        {
            List<AdvertIndexView> ViewAdverts = new List<AdvertIndexView>();
            AdvertIndexView CurAdv;
            foreach (var m in adverts)
            {
                if (is_person == true || m.Is_vis == true)
                {
                    CurAdv = new AdvertIndexView
                    {
                        Breed = _breedLogic.GetBreedNameById(m.BreedId),
                        Desription = m.Description,
                        Age = m.Age,
                        AdvertId = m.Id,
                        Rating = _feedbackLogic.GetRating(m.ApplicationUserId),
                        OwnerId = m.ApplicationUserId,
                        Price = m.Price,
                        OwnerName = _userLogic.FindUserNameById(m.ApplicationUserId),
                        Photo = _photoLogic.GetObjectPhotoById(m.Cat_PhotoId),
                        Date = m.AdvertDate,
                        IsVis = m.Is_vis,
                        City = _userLogic.FindUserCityById(m.ApplicationUserId)
                    };
                    ViewAdverts.Add(CurAdv);
                }
            }
            return ViewAdverts;
        }
        [Authorize]
        public IActionResult PersonalIndex()
        {
            var user = _userManager.GetUserAsync(User).Result;
            var adverts = _advertLogic.GetListPersonalAdverts(user);
            var TrueAdverts = GetTrueAdverts(adverts, true);

            return View(TrueAdverts);
        }

        public IActionResult Index()
        {
            var adverts = _advertLogic.GetListAllAdverts();

            var TrueAdverts = GetTrueAdverts(adverts, false);

            ViewBag.breed = _breedLogic.GetAllBreeds();

            return View(TrueAdverts);
        }

        [HttpPost]
        public IActionResult Index(string breed, int cost_low, int cost_high, int age, string bcity)
        {
            var adverts = _advertLogic.GetListFilteredAdverts(breed, cost_low, cost_high, age, bcity);

            var TrueAdverts = GetTrueAdverts(adverts, false);
            ViewBag.breed = _breedLogic.GetAllBreeds();
            ViewBag.age = age;
            return View(TrueAdverts);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int aid)
        {
            if (aid <= 0)
            {
                _logger.LogError("Ошибка изменения объявления некорректный ID=%d.", aid);
                return RedirectToAction("PersonalIndex", "Adverts");
            }

            cat_advert m = _advertLogic.GetAdvertById(aid);

            if (m != null)
            {


                var user = _userManager.GetUserAsync(User).Result;

                if (_userLogic.CompareUserWithCur(m.ApplicationUserId, user) == false)
                    return RedirectToAction("PersonalIndex", "Adverts");

                AdvertViewModel advert = new AdvertViewModel
                {
                    BreedId = m.BreedId.ToString(),
                    Descr = m.Description,
                    Age = m.Age,
                    AdvertId = m.Id,
                    UserId = m.ApplicationUserId,
                    Price = m.Price,
                    PhotoId = m.Cat_PhotoId,

                };
                ViewBag.breed = _breedLogic.GetAllBreeds();
                return View(advert);
            }
            _logger.LogError("Ошибка отображения объявления. Объявление с id=%d не найдено.", aid);
            return RedirectToAction("PersonalIndex", "Adverts");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(AdvertViewModel advert, IFormFile pic)
        {
            if (ModelState.IsValid)
            {
                AdvertViewModel model = new AdvertViewModel
                {
                    AdvertId = advert.AdvertId,
                    Age = advert.Age,
                    Descr = advert.Descr,
                    BreedId = advert.BreedId,
                    UserId = advert.UserId,
                    Price = advert.Price

                };
                if (pic != null)
                {
                    int success_photo = _photoLogic.PhotoCreate(pic);
                    if (success_photo == -1)
                    {
                        _logger.LogError("Попытка загрузки файла для изменения объявления завершилась ошибкой. " +
                            "Повторая отправка в Edit.");
                        ViewBag.breed = _breedLogic.GetAllBreeds();
                        return View(model);
                    }
                    else if (success_photo == 1)
                    {
                        model.PhotoId = _photoLogic.GetLastPhotoId();
                        model.Photo_name = _photoLogic.GetLastPhotoName();
                        _logger.LogInformation("Успешная загрузка файла для измения объявления с id=%d. Id изоражения = %d.", model.AdvertId,model.PhotoId);
                    }
                }
                _advertLogic.EditAdvert(model);
                _advertLogic.PublicAdvert(model.AdvertId);
                return RedirectToAction("PersonalIndex", "Adverts");
            }
            ViewBag.breed = _breedLogic.GetAllBreeds();
            return View(advert);
        }

        public IActionResult More(int aid)
        {
            cat_advert m = _advertLogic.GetAdvertById(aid);

            AdvertIndexView model = new AdvertIndexView
            {
                Breed = _breedLogic.GetBreedNameById(m.BreedId),
                Desription = m.Description,
                Age = m.Age,
                AdvertCount = _feedbackLogic.GetFeedCount(m.ApplicationUserId),
                Rating = _feedbackLogic.GetRating(m.ApplicationUserId),
                AdvertId = aid,
                OwnerId = m.ApplicationUserId,
                Price = m.Price,
                IsVis = m.Is_vis,
                OwnerName = _userLogic.FindUserNameById(m.ApplicationUserId),
                Photo = _photoLogic.GetObjectPhotoById(m.Cat_PhotoId),
                Email = _userLogic.FindUserEmailById(m.ApplicationUserId),
                Date = m.AdvertDate,
                City = _userLogic.FindUserCityById(m.ApplicationUserId)
            };
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "admin,moderator")]
        public IActionResult Hide(int aid)
        {
            var model = new HideCommentViewModel
            {
                AdvertId = aid,
                Comment = "Замечание"
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Hide(HideCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comment = new HideCommentViewModel
                {
                    AdvertId = model.AdvertId,
                    Comment = model.Comment
                };
                _hideLogic.AddCommentInDB(comment.AdvertId, comment.Comment);
                _advertLogic.HideAdvert(comment.AdvertId);
                _logger.LogInformation("Скрыта информация об объявлении с id = %d", comment.AdvertId);
                return RedirectToAction("Index", "Adverts");
            }
           
            return View(model);
        }

        public IActionResult HideReason(int aid)
        {
            var model = new HideCommentViewModel { AdvertId=aid, Comment = _hideLogic.GetCommentByAdvertId(aid)};
            return View(model);

        }

    }
}