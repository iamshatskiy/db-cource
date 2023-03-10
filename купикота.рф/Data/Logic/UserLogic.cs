using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using купикота.рф.Models;

namespace купикота.рф.Data.Logic
{
    public class UserLogic
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserLogic(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public ApplicationUser FindUserById(string Id)
        {
            return _userManager.FindByIdAsync(Id).Result;
        }

        public string GetUserId(ApplicationUser _User)
        {
            return _User.Id;
        }

        public bool CompareUserWithCur(string UserId, ApplicationUser CurUser)
        {
            var UserP = FindUserById(UserId);
            string UserPId = GetUserId(UserP);
            string CurUserId = GetUserId(CurUser);

            if (CurUserId == null || UserPId == null)
                return false;

            return UserPId == CurUserId;
        }

        public string FindUserCityById(string UserId)
        {
            var user = _userManager.FindByIdAsync(UserId).Result;
            return user.city;
        }

        public string FindUserEmailById(string UserId)
        {
            var user = _userManager.FindByIdAsync(UserId).Result;
            return user.Email;
        }

        public string FindUserNameById(string UserId)
        {
            var user = _userManager.FindByIdAsync(UserId).Result;
            return user.surname + " " + user.name;
        }

        public string FindUserNumById(ApplicationUser UserId)
        {
            var num = _userManager.GetPhoneNumberAsync(UserId).Result;
            return num;
        }
    }
}
