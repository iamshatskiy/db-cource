using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace купикота.рф.Models.AdvertViewModels
{
    public class AdvertViewModel
    {
        public int AdvertId { get; set; }
        [Required]
        [Display(Name = "Возраст")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Порода должна быть выбрана :-)")]
        [Display(Name = "Порода")]
        public string BreedId { set; get; }
        [Required(ErrorMessage = "Описание должно быть заполнено! (Максимум 500 символов)")]
        [Display(Name = "Описание")]
        [StringLength(500, ErrorMessage = "Слишком длинное описание (Максимум {1} символов)")]
        public string Descr { get; set; }
        [Required]
        [Display(Name = "Цена")]
        public int Price { get; set; }

        [Display(Name = "Фото")]
        
        public string Photo { get; set; }
        public string Photo_name { get; set; }
        public string UserId { get; set; }
        public int PhotoId { get; set; }

        
    }
}
