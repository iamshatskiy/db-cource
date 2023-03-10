using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.Models;
using купикота.рф.Models.AdvertViewModels;

namespace купикота.рф.Data.Interfaces
{
    public interface IDeal
    {
        //При создании отображение для продавца сначала,
        //Если отклонено, то удаляем запрос,
        //Создать можно только один запрос на одно объявление одному человеку,
        //Если подтверждено, то ожидание покупателя (Если подтвердил, то удаляется запись в об-нии, 
        //фотках и остальные ссылающиеся на запись сделки, 
        //Если отклонено, то удаляем сделку, + в двух последних вариантах делаем рейтинг и отзыв (другая таблица).
       
        void Create(Deals deal);
        int Delete(int DealId);
        void ConfirmedOwner(int DealId, int Conf);
        void ConfirmedBuyer(int DealId, int Conf);
        IEnumerable<Deals> GetDeals(int AdvertId);
        IEnumerable<Deals> GetAllDealsIncoming(string OwnerId);
        IEnumerable<Deals> GetAllDealsOutgoing(string BuyerId);

    }
}
