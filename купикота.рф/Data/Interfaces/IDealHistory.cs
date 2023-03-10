using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;

namespace купикота.рф.Data.Interfaces
{
    public interface IDealHistory
    {
        IEnumerable<DealHistory> GetAllHistory();
    }
}
