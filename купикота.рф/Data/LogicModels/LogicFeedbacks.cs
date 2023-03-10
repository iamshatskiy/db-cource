using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace купикота.рф.Data.LogicModels
{
    public class LogicFeedbacks
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        public string OwnerId { get; set; }
        public int Rate { get; set; }
        public string Feedback { get; set; }
        public DateTime FbDate { get; set; }
    }
}
