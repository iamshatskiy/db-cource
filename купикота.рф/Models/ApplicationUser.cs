using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace купикота.рф.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string city { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
    }
}
