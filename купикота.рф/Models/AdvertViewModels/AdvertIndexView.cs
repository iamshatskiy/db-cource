using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;

namespace купикота.рф.Models.AdvertViewModels
{
    public class AdvertIndexView
    {
        public int AdvertId { get; set; }
        [Display(Name = "Возраст")]
        public int Age { get; set; }
        [Display(Name = "Порода")]
        public string Breed { get; set; }
        [Display(Name = "Описание")]
        public string Desription { get; set; }
        [Display(Name = "Стоимость")]
        public int Price { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string Email { get; set; }
        public LogicPhoto Photo { get; set; }
        public DateTime Date { get; set; }
        public string City { get; set; }
        public bool IsVis { get; set; }
        public float Rating { get; set; }
        public int AdvertCount { get; set; }


    }
}
