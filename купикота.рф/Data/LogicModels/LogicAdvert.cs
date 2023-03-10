using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace купикота.рф.Data.LogicModels
{
    public class LogicAdvert
    {
        public int Id { set; get; }
        public int BreedId { set; get; }
        public virtual LogicBreed Breed { set; get; }
        public int Age { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool Is_vis { get; set; }
        public int Cat_PhotoId { get; set; }
        public virtual LogicPhoto Cat_Photo { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
