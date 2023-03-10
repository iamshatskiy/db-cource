using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;

namespace купикота.рф.Data.Repository
{
    public class DealHistoryRepository : IDealHistory
    {
        private readonly ApplicationDbContext applicationDbContext;
        IEnumerable<LogicDealHistory> logicDealHistories;

        public DealHistoryRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
            logicDealHistories = new List<LogicDealHistory>();
            foreach (var dealh in applicationDbContext.DealHistory)
                ((List<LogicDealHistory>)logicDealHistories).Add(new LogicDealHistory {Id = dealh.Id,
                    BuyerId = dealh.BuyerId, OwnerId = dealh.OwnerId, IsConfirmed = dealh.IsConfirmed });
        }
        public IEnumerable<DealHistory> GetAllHistory() => applicationDbContext.DealHistory;
       
    }
}
