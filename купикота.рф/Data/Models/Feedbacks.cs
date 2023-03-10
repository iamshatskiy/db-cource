using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Models;

namespace купикота.рф.Data.Models
{
    public class Feedbacks
    {
        public int Id { get; set; }
        public string BuyerId { get; set; }
        [ForeignKey("BuyerId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual ApplicationUser ApplicationUser2 { get; set; }
        public int Rate { get; set; }
        public string Feedback { get; set; }
        public DateTime FbDate { get; set; }

    }
}
