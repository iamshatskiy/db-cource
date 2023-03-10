using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.LogicModels;

namespace купикота.рф.Data.Logic
{
    public class BreedLogic
    {
        private readonly IBreed _breed;

        public BreedLogic(IBreed breed)
        {
            _breed = breed;
        }

        public IEnumerable<LogicBreed> logicBreeds()
        {
            return _breed.AllBreeds;
        }

        public SelectList GetAllBreeds()
        {
            return _breed.Get_breeds();
        }

        public string GetBreedNameById(int BreedId)
        {
            var breed = _breed.getObjectBreed(BreedId);
            return breed.Breed_name;
        }
    }

}
