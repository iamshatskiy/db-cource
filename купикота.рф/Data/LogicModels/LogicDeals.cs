using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace купикота.рф.Data.LogicModels
{
    public class LogicDeals
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public string OwnerId { get; set; }
        public int IsConfirmedByOWner { get; set; }
        public int IsConfirmedByBuyer { get; set; }
        public int AdvertId { get; set; }
    }
}
