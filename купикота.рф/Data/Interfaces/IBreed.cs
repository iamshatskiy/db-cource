using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;

namespace купикота.рф.Data.Interfaces
{
    public interface IBreed
    {
        IEnumerable<LogicBreed> AllBreeds { get;}
        LogicBreed getObjectBreed(int breed_id);
        SelectList Get_breeds();
    }
}
