using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace купикота.рф.Models.DealViewModels
{
    public class DealHistoryViewModel
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string BuyerId { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
