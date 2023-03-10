using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.Models;

namespace купикота.рф.Data.Logic
{
    public class DealLogic
    {
        private readonly IDeal _deal;

        public DealLogic(IDeal deal)
        {
            _deal = deal;
        }

        public int CreateDeal(Deals deal)
        {
            var CurDeals = _deal.GetDeals(deal.AdvertId);
            foreach (var d in CurDeals)
            {
                if (d.BuyerId == deal.BuyerId)
                    return -1;
            }

            _deal.Create(deal);
            return 0;
        }

        public IEnumerable<Deals> GetIncomingDeals(string ownerId)
        {
            var deals = _deal.GetAllDealsIncoming(ownerId);
            return deals;
        }

        public IEnumerable<Deals> GetOutgoingDeals(string buyerId)
        {
            var deals = _deal.GetAllDealsOutgoing(buyerId);
            return deals;
        }

        public int DeleteDeal(int DealId)
        {
            int success = _deal.Delete(DealId);
            return success;
        }

        public void OwnerConformition(int DealId, sbyte Conf)
        {
            _deal.ConfirmedOwner(DealId, Conf);
        }

        public void BuyerConformition(int DealId, sbyte Conf)
        {
            _deal.ConfirmedBuyer(DealId, Conf);
        }
    }
}
