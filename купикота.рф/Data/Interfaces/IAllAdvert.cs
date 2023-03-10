using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;
using купикота.рф.Models.AdvertViewModels;

namespace купикота.рф.Data.Interfaces
{
    public interface IAllAdvert
    {
        IEnumerable<cat_advert> PersonalAdverts(string uid);
        IEnumerable<cat_advert> AllAdverts { get;}
        cat_advert getOblectCat_Advert(int advertId);
        void CreateAdvert(AdvertViewModel advert);
        void EditAdvert(LogicAdvert advert);
        int getAdvertsCount(int OwnerId);
        void HideAdvert(int AdvertId);
        void PublicAdvert(int AdvertId);
    }
}
