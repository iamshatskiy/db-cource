using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Models;

namespace купикота.рф.Data.Models
{
    public class cat_advert
    {
        public int Id { set; get; }
        public int BreedId { set; get; }
        public virtual breed Breed { set; get; }
        public int Age { get; set;}
        public string Description { get; set; }
        public int Price { get; set; }
        public bool Is_vis { get; set; }
        public DateTime AdvertDate { get; set; }
        public int Cat_PhotoId { get; set; }
        public virtual cat_photo Cat_Photo { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
