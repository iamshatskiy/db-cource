using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using купикота.рф.Data.Models;
using купикота.рф.Models;
using купикота.рф.Models.AccountViewModels;

namespace купикота.рф.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private string ConnectionString { get; set; }
        public ApplicationDbContext(string con) 
        {
            ConnectionString = con;
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<breed> Breeds { get; set; }
        public DbSet<cat_advert> Cat_Adverts { get; set; }
        public DbSet<cat_photo> Cat_Photos { get; set; }
        public DbSet<Deals> Deals { get; set; }
        public DbSet<Feedbacks> Feedbacks { get; set; }
        public DbSet<HideComments> HideComments { get; set; }

        public DbSet<DealHistory> DealHistory { get; set; }


    }
}
