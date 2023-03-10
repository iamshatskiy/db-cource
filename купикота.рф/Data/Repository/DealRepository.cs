using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;
using купикота.рф.Models.AdvertViewModels;

namespace купикота.рф.Data.Repository
{
    public class DealRepository : IDeal
    {
        private readonly ApplicationDbContext applicationDbContext;
        List<LogicDeals> logicDeals;
        public DealRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;

            logicDeals = new List<LogicDeals>();

            foreach (var deal in applicationDbContext.Deals)
                ((List<LogicDeals>)logicDeals).Add(new LogicDeals
                {
                    Id = deal.Id,
                    AdvertId = deal.AdvertId,
                    OwnerId = deal.OwnerId,
                    BuyerId = deal.BuyerId,
                    IsConfirmedByBuyer = deal.IsConfirmedByBuyer,
                    IsConfirmedByOWner = deal.IsConfirmedByOWner
                });
        }

        public void ConfirmedBuyer(int DealId, int Conf)
        {
            var dl = applicationDbContext.Deals.FirstOrDefault(p => p.Id == DealId);
            if (dl != null)
            {
                dl.IsConfirmedByBuyer = Conf;
                applicationDbContext.SaveChanges();
            }
        }

        public void ConfirmedOwner(int DealId, int Conf)
        {
            var dl = applicationDbContext.Deals.FirstOrDefault(p => p.Id == DealId);
            if (dl != null)
            {
                dl.IsConfirmedByOWner = Conf;
                applicationDbContext.SaveChanges();
            }
        }

        public IEnumerable<Deals> GetDeals(int AdvertId)
        {
            var deals = applicationDbContext.Deals.Where(p => p.AdvertId == AdvertId);
            return deals;
        }

        public IEnumerable<Deals> GetAllDealsIncoming(string OwnerId)
        {
            var deals = applicationDbContext.Deals.Where(p => p.OwnerId == OwnerId);
            return deals;
        }

        public IEnumerable<Deals> GetAllDealsOutgoing(string BuyerId)
        {
            var deals = applicationDbContext.Deals.Where(p => p.BuyerId == BuyerId);
            return deals;
        }

        public void Create(Deals deal)
        {
            applicationDbContext.Add(new Deals
            {
                AdvertId = deal.AdvertId,
                BuyerId = deal.BuyerId,
                OwnerId = deal.OwnerId,
                IsConfirmedByBuyer = 0,
                IsConfirmedByOWner = 0
            });
            applicationDbContext.SaveChanges();
        }

        public int Delete(int DealId)
        {
            var row = applicationDbContext.Deals.FirstOrDefault(p => p.Id == DealId);
            if (row != null)
            {
                applicationDbContext.Deals.Remove(row);
                applicationDbContext.SaveChanges();
                return 0;
            }
            return -1;
        }
    }
}
