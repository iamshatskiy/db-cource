using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;
using купикота.рф.Models.AdvertViewModels;

namespace купикота.рф.Data.Repository
{
    public class PhotoRepository : IPhoto
    {
        private readonly ApplicationDbContext applicationDbContext;
        IEnumerable<LogicPhoto> logicPhotos;
        public PhotoRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
            logicPhotos = new List<LogicPhoto> { };

            foreach (var photo in applicationDbContext.Cat_Photos)
                ((List<LogicPhoto>)logicPhotos).Add(new LogicPhoto { Id = photo.Id, Image = photo.Image, Name = photo.Image });

        }

        public IEnumerable<LogicPhoto> Cat_Photos => logicPhotos;

        public void CreatePhoto(string Photo_name, string Photo)
        {
            applicationDbContext.Add(new cat_photo
            {
                Name = Photo_name,
                Image = Photo
                
            });
            
            applicationDbContext.SaveChanges();
            var photo = GetLastPhoto();
            ((List<LogicPhoto>)logicPhotos).Add(new LogicPhoto { Id = photo.Id, Image = photo.Image, Name = photo.Image });
        }

        public cat_photo GetLastPhoto() => applicationDbContext.Cat_Photos.LastOrDefault();
        
        public LogicPhoto GetObjectPhoto(int photo_id)
        {
            var photo = applicationDbContext.Cat_Photos.FirstOrDefault(p => p.Id == photo_id);
            if (photo != null)
            {
                return new LogicPhoto { Id = photo.Id, Image = photo.Image, Name = photo.Name };
            }
            return null;
        }
    }
}
