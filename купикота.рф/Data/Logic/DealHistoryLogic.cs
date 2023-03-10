using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;

namespace купикота.рф.Data.Logic
{
    public class DealHistoryLogic
    {
        private readonly IDealHistory _dealHistory;

        public DealHistoryLogic(IDealHistory dealHistory)
        {
            _dealHistory = dealHistory;
        }

        public IEnumerable<DealHistory> GetAllHistory()
        {
            return _dealHistory.GetAllHistory();
        }
    }
}
