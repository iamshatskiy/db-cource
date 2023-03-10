using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace купикота.рф.Data.LogicModels
{
    public class LogicDealHistory
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string BuyerId { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
