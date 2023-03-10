using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.Models;
using купикота.рф.Models;
using купикота.рф.Models.AdvertViewModels;
using купикота.рф.Data.LogicModels;

namespace купикота.рф.Data.Logic
{
    public class AdvertLogic
    {
        private readonly IAllAdvert _allAdvert;
        private readonly UserLogic _userLogic;

        public AdvertLogic(IAllAdvert allAdvert, UserLogic userLogic)
        {
            _allAdvert = allAdvert;
            _userLogic = userLogic;
        }

        

        public void AddNewAdvert(AdvertViewModel advert)
        {
            _allAdvert.CreateAdvert(advert);
        }

        public List<cat_advert> GetListAllAdverts()
        {
            return _allAdvert.AllAdverts.ToList();
        }

        public List<cat_advert> GetListFilteredAdverts(string breed, int cost_low, int cost_high, int age, string bcity)
        {

            List<cat_advert> FilteredAdverts = new List<cat_advert>();

            var adverts = _allAdvert.AllAdverts.ToList();

            foreach (var a in adverts)
            {
                bool flag1 = false;

                if (age != 0)
                {
                    if (a.Age == age)
                        flag1 = true;
                }
                else
                    flag1 = true;
               


                bool flag2 = false;

                if (breed != null)
                {
                    if (a.BreedId == Int32.Parse(breed))
                        flag2 = true;
                }
                else
                    flag2 = true;


                bool flag3 = false;
                int cost = a.Price;
                if (cost_low < cost_high)
                    if (cost >= cost_low && cost <= cost_high)
                        flag3 = true;
                    else
                        flag3 = false;
                else
                {
                    if (cost >= cost_low)
                        flag3 = true;
                    else
                        flag3 = false;
                }

                bool flag4 = false;

                if (bcity != null)
                {
                    var city = _userLogic.FindUserCityById(a.ApplicationUserId);
                    if (city != null && city == bcity)
                        flag4 = true;
                }
                else
                    flag4 = true;

                if (flag1 && flag2 && flag3 && flag4)
                    FilteredAdverts.Add(a);
            }

            return FilteredAdverts;
        }

        public List<cat_advert> GetListPersonalAdverts(ApplicationUser User)
        {
            return _allAdvert.PersonalAdverts(_userLogic.GetUserId(User)).ToList();
        }

        public cat_advert GetAdvertById(int AdvertId)
        {
            return _allAdvert.getOblectCat_Advert(AdvertId);
        }

        public void EditAdvert(AdvertViewModel advert)
        {
            _allAdvert.EditAdvert(new LogicAdvert {Id = advert.AdvertId, Age = advert.Age, BreedId = Int32.Parse(advert.BreedId), Cat_PhotoId = advert.PhotoId, Description = advert.Descr, Price = advert.Price, ApplicationUserId = advert.UserId});
        }

        public void HideAdvert(int AdvertId)
        {
            _allAdvert.HideAdvert(AdvertId);
        }

        public void PublicAdvert(int AdvertId)
        {
            _allAdvert.PublicAdvert(AdvertId);
        }




    }
}
