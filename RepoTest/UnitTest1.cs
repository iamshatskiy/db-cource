using Microsoft.Extensions.Logging;
using System;
using Xunit;
using купикота.рф.Data;
using купикота.рф.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using купикота.рф.Data.LogicModels;

namespace RepoTest
{
    public class BreedRepoTest
    {

        public static IConfiguration _config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
        private readonly ILogger<BreedRepository> _logger;
        private readonly ILogger<cat_advertRepository> _loggera;

        [Fact]
        public void BreedTest()
        {
            
            BreedRepository rep = new BreedRepository(new ApplicationDbContext(_config["DefaultConnection"]), _logger);

            var res = rep.getObjectBreed(1);
            Assert.Equal("Бирманская", res.Breed_name);

            res = rep.getObjectBreed(0);
            Assert.Equal("", res.Breed_name);
        }

        [Fact]
        public void PhotoTest()
        {

            PhotoRepository rep = new PhotoRepository(new ApplicationDbContext(_config["DefaultConnection"]));

            rep.CreatePhoto("cat", "~/images/cat.jpg");
            var photo = rep.GetLastPhoto();
            Assert.Equal("~/images/cat.jpg", photo.Name);

            var res = rep.GetObjectPhoto(5);
            Assert.NotNull(res);
        }

        [Fact]
        public void HideTest()
        {

            HideRepository rep = new HideRepository(new ApplicationDbContext(_config["DefaultConnection"]));

            rep.AddComment(117, "have fun");
            var res = rep.GetCommentObject(117);

            Assert.NotNull(res);

            rep.DeleteComment(117);
            res = rep.GetCommentObject(117);
            Assert.Null(res);
        }

        [Fact]
        public void FeedbackTest()
        {

            FeedbackRepository rep = new FeedbackRepository(new ApplicationDbContext(_config["DefaultConnection"]));

            var res = rep.GetFeed(5);

            Assert.Equal(4, res.Rate);

            rep.DeleteFeed(5);
            res = rep.GetFeed(5);
            Assert.Null(res);
        }

        [Fact]
        public void DealTest()
        {
            DealRepository rep = new DealRepository(new ApplicationDbContext(_config["DefaultConnection"]));

            var res = rep.GetAllDealsOutgoing("somebody");

            Assert.Null(res);

            var result = rep.Delete(4);

            Assert.Equal(-1, result);
        }

        [Fact]
        public void AdvertsTest()
        {
            cat_advertRepository rep = new cat_advertRepository(new ApplicationDbContext(_config["DefaultConnection"]), _loggera);

            rep.HideAdvert(6);

            var res = rep.getOblectCat_Advert(6);

            Assert.NotNull(res);

            Assert.False(res.Is_vis);

            rep.PublicAdvert(6);
            res = rep.getOblectCat_Advert(6);
            Assert.True(res.Is_vis);

            int count = rep.getAdvertsCount(5);
            Assert.Equal(1, count);

            LogicAdvert advert = new LogicAdvert { Id = 7, Age = 10, Description = "описание", Price = 10000 };

            rep.EditAdvert(advert);

            res = rep.getOblectCat_Advert(7);

            Assert.Equal(10000, res.Price);
            Assert.Equal(10, res.Age);
            Assert.Equal("описание", res.Description);
        }
    }
    
}
