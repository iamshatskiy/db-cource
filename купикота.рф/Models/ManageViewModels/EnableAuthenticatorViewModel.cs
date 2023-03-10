using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace купикота.рф.Models.ManageViewModels
{
    public class EnableAuthenticatorViewModel
    {
        [Required]
        [StringLength(7, ErrorMessage = "{0} должен быть как минимум {2} и максимум {1} символов в длину.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Код верификации")]
        public string Code { get; set; }

        [BindNever]
        public string SharedKey { get; set; }

        [BindNever]
        public string AuthenticatorUri { get; set; }
    }
}
