using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;
using купикота.рф.Models.AdvertViewModels;

namespace купикота.рф.Data.Interfaces
{
    public interface IPhoto
    {
        IEnumerable<LogicPhoto> Cat_Photos { get; }
        LogicPhoto GetObjectPhoto(int photo_id);
        void CreatePhoto(string Photo_name, string Photo);
        cat_photo GetLastPhoto();
    }
}
