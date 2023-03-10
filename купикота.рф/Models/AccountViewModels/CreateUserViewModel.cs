using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace купикота.рф.Models.AccountViewModels
{
    public class CreateUserViewModel
    {
        public string Email { get; set; }
        
        public string Password { get; set; }
        
        public string FirstName { get; set; }
       
        public string SecondName { get; set; }
        
        public string city { get; set; }
    }
}
