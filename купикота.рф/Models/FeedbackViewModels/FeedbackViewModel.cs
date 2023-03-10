using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace купикота.рф.Models.FeedbackViewModels
{
    public class FeedbackViewModel
    {
        public int Id { get; set; }
        public string OwnerName { get; set; }
        public string BuyerName { get; set; }
        public string OwnerId { get; set; }
        public string BuyerId { get; set; }
        [Required(ErrorMessage = "Отзыв должен быть заполнен! (Максимум 200 символов)")]
        [StringLength(200, ErrorMessage = "Слишком длинный отзыв (Максимум {1} символов)")]
        public string Feedback { get; set; }
        public DateTime FbDate { get; set; }
        [Required(ErrorMessage = "Рейтинг должен быть поставлен!")]
        public int Rate { get; set; }
    }
}
