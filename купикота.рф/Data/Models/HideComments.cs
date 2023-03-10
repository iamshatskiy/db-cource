using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace купикота.рф.Data.Models
{
    public class HideComments
    {
        public int Id { get; set; }
        public int AdvertId { get; set; }
        [ForeignKey("AdvertId")]
        public virtual cat_advert Cat_Advert { get; set; }
        public string Comment { get; set; }
    }
}
