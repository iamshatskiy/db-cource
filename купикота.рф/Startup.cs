using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using купикота.рф.Data;
using купикота.рф.Models;
using купикота.рф.Services;
using купикота.рф.Data.Repository;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.Logic;

namespace купикота.рф
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));


            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IAllAdvert, cat_advertRepository>();
            services.AddTransient<IBreed, BreedRepository>();
            services.AddTransient<IPhoto, PhotoRepository>();
            services.AddTransient<IHideAdverts, HideRepository>();
            services.AddTransient<IDeal, DealRepository>();
            services.AddTransient<IDealHistory, DealHistoryRepository>();
            services.AddTransient<IFeedbacks, FeedbackRepository>();

            services.AddScoped<AdvertLogic>();
            services.AddScoped<BreedLogic>();
            services.AddScoped<PhotoLogic>();
            services.AddScoped<UserLogic>();
            services.AddScoped<HideLogic>();
            services.AddScoped<DealLogic>();
            services.AddScoped<DealHistoryLogic>();
            services.AddScoped<FeedbackLogic>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
