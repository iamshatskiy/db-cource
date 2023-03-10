using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.Models;

namespace купикота.рф.Models.AdvertViewModels
{
    public class DealViewModel
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public string BuyerNum { get; set; }
        public string BuyerEmail { get; set; }
        public string BuyerName { get; set; }
        public string OwnerId { get; set; }
        public string OwnerNum { get; set; }
        public string OwnerEmail { get; set; }
        public string OwnerName { get; set; }
        public int ConfByOWner { get; set; }
        public int ConfByBuyer { get; set; }
        public int AdvertId { get; set; }
    }
}
