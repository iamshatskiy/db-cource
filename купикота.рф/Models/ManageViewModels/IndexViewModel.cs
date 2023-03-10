using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace купикота.рф.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string Firstname { get; set; }
        [Required]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Город")]
        public string city { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Phone]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }



        public string StatusMessage { get; set; }
    }
}
