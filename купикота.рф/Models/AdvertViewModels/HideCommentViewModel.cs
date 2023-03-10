using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace купикота.рф.Models.AdvertViewModels
{
    public class HideCommentViewModel
    {
        public int AdvertId { get; set; }
        [Required(ErrorMessage = "Комментарий должен быть заполнен! (Максимум 250 символов)")]
        public string Comment { get; set; }
    }
}
