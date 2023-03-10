using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Models;

namespace купикота.рф.Data.Models
{
    public class Deals
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        [ForeignKey("BuyerId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual ApplicationUser ApplicationUser2 { get; set; }
        public int IsConfirmedByOWner { get; set; }
        public int IsConfirmedByBuyer { get; set; }
        public int AdvertId { get; set; }
        [ForeignKey("AdvertId")]
        public virtual cat_advert Cat_Advert { get; set; }
    }
}
