using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;

namespace купикота.рф.Data.Repository
{
    public class BreedRepository : IBreed
    {
        private readonly ApplicationDbContext applicationDbContext;
        IEnumerable<LogicBreed> logicBreeds;

        public BreedRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
            logicBreeds = new List<LogicBreed>();

            foreach (var breed in applicationDbContext.Breeds)
                ((List<LogicBreed>)logicBreeds).Add(new LogicBreed { Id = breed.Id, Breed_name = breed.Breed_name});
        }

        public IEnumerable<LogicBreed> AllBreeds => logicBreeds;

        public SelectList Get_breeds()
        {
            SelectList Bs = new SelectList(applicationDbContext.Breeds, nameof(breed.Id), nameof(breed.Breed_name), nameof(breed.Id));
            return Bs;
        }

        public LogicBreed getObjectBreed(int breed_id)
        {
            var br = applicationDbContext.Breeds.FirstOrDefault(p => p.Id == breed_id);
            if (br == null)
            {
                return null;
            }
            return new LogicBreed {Id = br.Id, Breed_name = br.Breed_name };
        }
    }
}
