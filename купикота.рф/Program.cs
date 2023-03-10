using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CustomIdentityApp;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using купикота.рф.Models;

namespace купикота.рф
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                Task t;
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<Models.ApplicationUser>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    t = RoleInitializer.InitializeAsync(userManager, rolesManager);
                    t.Wait();

                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<Logger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

            public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
      
    }
}
