using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using купикота.рф.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using купикота.рф.Data.Models;
using купикота.рф.Data.LogicModels;

namespace купикота.рф.Data.Logic
{
    public class PhotoLogic
    {
        

        private readonly IPhoto _photo;
        private readonly IHostingEnvironment hostingEnvironment;

        public PhotoLogic(IPhoto photo, IHostingEnvironment hostingEnvironment)
        {
            _photo = photo;
            this.hostingEnvironment = hostingEnvironment;
        }

        public int PhotoCreate(IFormFile pic)
        {
            IFormFile f = pic;
            string uniqueFileName = null;
            if (f != null && f.Length > 0)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + pic.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                pic.CopyTo(new FileStream(filePath, FileMode.Create));

                if (filePath == null)
                    return -1;

                _photo.CreatePhoto(uniqueFileName, filePath);
                return 1;
            }
            return 0;
        }
        
        public int GetLastPhotoId()
        {
            return _photo.GetLastPhoto().Id;
        }

        public string GetLastPhotoName()
        {
            return _photo.GetLastPhoto().Name;
        }

        public LogicPhoto GetObjectPhotoById(int PhotoId)
        {
            return _photo.GetObjectPhoto(PhotoId);
        }

    }
}
