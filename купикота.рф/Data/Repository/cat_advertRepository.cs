using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;
using купикота.рф.Models.AdvertViewModels;

namespace купикота.рф.Data.Repository
{
    public class cat_advertRepository : IAllAdvert
    {
        private readonly ApplicationDbContext applicationDbContext;

        public cat_advertRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public IEnumerable<cat_advert> PersonalAdverts(string uid) => applicationDbContext.Cat_Adverts.Where(p => p.ApplicationUserId == uid);
        public IEnumerable<cat_advert> AllAdverts => applicationDbContext.Cat_Adverts.Where(p => p.Is_vis);

        public void CreateAdvert(AdvertViewModel advert)
        {
            applicationDbContext.Add(new cat_advert
            {
                AdvertDate = DateTime.Now,
                Age = advert.Age,
                BreedId = Int32.Parse(advert.BreedId),
                Description = advert.Descr,
                Price = advert.Price,
                Is_vis = true,
                Cat_PhotoId = advert.PhotoId,
                ApplicationUserId = advert.UserId
            });
            applicationDbContext.SaveChanges();
            
        }

        public void EditAdvert(LogicAdvert advert)
        {
            cat_advert Dr = applicationDbContext.Cat_Adverts.Where(p => p.Id == advert.Id).FirstOrDefault();
            if (Dr != null)
            {
                Dr.Age = advert.Age;
                Dr.BreedId = advert.BreedId;
                Dr.Description = advert.Description;
                Dr.Price = advert.Price;
                if (advert.Cat_PhotoId > 0)
                    Dr.Cat_PhotoId = advert.Cat_PhotoId;
                applicationDbContext.SaveChanges();
               
            }
        }

        public cat_advert getOblectCat_Advert(int advertId) => applicationDbContext.Cat_Adverts.FirstOrDefault(p => p.Id == advertId);
        public int getAdvertsCount(int OwnerId)
        {
            return 1;
        }

        public void HideAdvert(int AdvertId)
        {
            cat_advert Dr = applicationDbContext.Cat_Adverts.Where(p => p.Id == AdvertId).FirstOrDefault();
            if (Dr != null)
            {
                Dr.Is_vis = false;
                applicationDbContext.SaveChanges();
            }
        }

        public void PublicAdvert(int AdvertId)
        {
            cat_advert Dr = applicationDbContext.Cat_Adverts.Where(p => p.Id == AdvertId).FirstOrDefault();
            if (Dr != null)
            {
                Dr.Is_vis = true;
                applicationDbContext.SaveChanges();
            }
        }

        
    }
}
