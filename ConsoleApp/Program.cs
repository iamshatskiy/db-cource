using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using купикота.рф.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using купикота.рф.Data.Logic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using купикота.рф.Models;
using CustomIdentityApp;
using System.Threading.Tasks;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.Repository;
using купикота.рф.Data.Presentor;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var _config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
            var db = new ApplicationDbContext(_config.GetConnectionString("DefaultConnection"));

            var builder = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<Start>();
                services.AddSingleton<AdvertLogic>();
                services.AddSingleton<UserLogic>();
                services.AddSingleton<BreedLogic>();
                services.AddSingleton<PhotoLogic>();
                services.AddSingleton<DealHistoryLogic>();
                services.AddSingleton<DealLogic>();
                services.AddSingleton<FeedbackLogic>();
                services.AddSingleton<HideLogic>();

                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(_config.GetConnectionString("DefaultConnection")));

            });

            var host = builder.Build();
            using (var serviceScope = host.Services.CreateScope())
            {
                Task t;
                var services = serviceScope.ServiceProvider;
                try
                {
                    //var prog = new Start(db);
                    var prog = services.GetRequiredService<Start>();
                    prog.Run();
                    Console.WriteLine("Successfully opened");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Occured");
                }
            }

            
        }
        class Start
        {
            private ApplicationDbContext _db;
            private  BreedLogic _breedLogic;
            private  HideLogic _hideLogic;
            private  FeedbackLogic _feedbackLogic;
            private  DealHistoryLogic _dealHistoryLogic;
            private  DealLogic _dealLogic;
            private Viewer _viewer;
            private Presentor _presentor;

            public Start(ApplicationDbContext db)
            {
                _db = db;
                IBreed br = new BreedRepository(db);
                IHideAdverts hide = new HideRepository(db);
                IFeedbacks feeds = new FeedbackRepository(db);
                IDeal deals = new DealRepository(db);
                IDealHistory history = new DealHistoryRepository(db);
                _breedLogic = new BreedLogic(br);
                _hideLogic = new HideLogic(hide);
                _feedbackLogic = new FeedbackLogic(feeds);
                _dealHistoryLogic = new DealHistoryLogic(history);
                _dealLogic = new DealLogic(deals);
                _viewer = new Viewer();
                _presentor = new Presentor(_breedLogic, _hideLogic, _feedbackLogic, _dealHistoryLogic, _dealLogic, _viewer);

                Run();
            }

            public void Run()
            {
                int need = 0;
                while (need != 6)
                {
                    PrintMenuMain();
                    need = Convert.ToInt32(Console.ReadLine());
                    switch (need)
                    {
                        case 1:
                            ActionBreed();
                            break;
                        case 2:
                            ActionHides();
                            break;
                        case 3:
                            ActionFeeds();
                            break;
                        case 4:
                            break;
                        case 5:
                            break;
                        case 6:
                            break;
                        default:
                            Console.WriteLine("Неверный номер");
                            break;
                    }
                }
                
            }
            void PrintMenuMain()
            {
                Console.WriteLine("\n Выберите категорию: \n1 - породы\n2 - скрытые комментарии\n3 - отзывы\n4 - сделки\n5 - история сделок \n6 - ВЫХОД");
            }

            public void ActionFeeds()
            {
                int need = 0;
                while (need != 4)
                {
                    Console.WriteLine("\n Выберите действие: \n1 - вывести все отзывы\n2 - получить по Id\n3 - получить все нулевые отзывы\n4 - ВЫХОД");
                    need = Convert.ToInt32(Console.ReadLine());
                    switch (need)
                    {
                        case 1:
                            _presentor.GetAllFeeds();
                            break;
                        case 2:
                            _presentor.GetFeedById();
                            break;
                        case 3:
                            _presentor.GetNullFeeds();
                            break;
                        case 4:
                            break;
                        default:
                            Console.WriteLine("Неверный номер");
                            break;
                    }
                }
            }

            public void ActionBreed()
            {
                
                int need = 0;
                while (need != 3)
                {
                    Console.WriteLine("\n Выберите действие: \n1 - вывести все породы\n2 - получить по Id имя породы\n3 - ВЫХОД");
                    need = Convert.ToInt32(Console.ReadLine());
                    switch (need)
                    {
                        case 1:
                            _presentor.AllBreeds();
                            break;
                        case 2:
                            _presentor.GetBreedById();
                            break;
                        case 3:
                            break;
                        default:
                            Console.WriteLine("Неверный номер");
                            break;
                    }
                }
            }

            public void ActionHides()
            {

                int need = 0;
                while (need != 4)
                {
                    Console.WriteLine("\n Выберите действие: \n1 - вывести все комментарии\n2 - получить по Id объявления\n3 - получить по Id\n4 - ВЫХОД");
                    need = Convert.ToInt32(Console.ReadLine());
                    switch (need)
                    {
                        case 1:
                            _presentor.AllComs();
                            break;
                        case 2:
                            _presentor.GetComByAdvertId();
                            break;
                        case 3:
                            _presentor.GetComById();
                            break;
                        case 4:
                            break;
                        default:
                            Console.WriteLine("Неверный номер");
                            break;
                    }
                }
            }
        }
        
    }
}
