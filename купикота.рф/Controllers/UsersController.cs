using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using купикота.рф.Models.AccountViewModels;
using купикота.рф.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace купикота.рф.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public UsersController(UserManager<ApplicationUser> userManager, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { Email = model.Email, UserName = model.Email, surname = model.SecondName, city = model.city };
                if (model.FirstName == null)
                    user.name = model.Email;
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Новый пользователь успешно создан АДМИНИСТРАТОРОМ.");
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError("Ошибка создания нового пользователя АДМИНИСТРАТОРОМ, не удалось добавить данные в БД.");
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, FirstName = user.name, SecondName = user.surname, city = user.city };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.name = model.FirstName;
                    user.surname = model.SecondName;
                    user.city = model.city;


                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Данные пользователя с id=%s успешно изменены АДМИНИСТРАТОРОМ.", user.Id);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _logger.LogError("Ошибка изменения данных пользователя с id=%s, ошибка БД.", user.Id);
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                _logger.LogError("Пользователь с id=%s успешно удален АДМИНИСТРАТОРОМ.", user.Id);
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            _logger.LogError("Ошибка удаления пользователя с id=%s. Пользователь не найден.", user.Id);
            return RedirectToAction("Index");
        }
    }
}