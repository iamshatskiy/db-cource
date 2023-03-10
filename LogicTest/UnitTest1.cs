using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;
using купикота.рф.Data;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.Logic;
using купикота.рф.Data.Repository;
using купикота.рф.Models;

namespace LogicTest
{
    public class LogicTests
    {
        [Fact]
        public void FeedTest()
        {
            FeedbackLogic rep = new FeedbackLogic(new MockFeedback());

            int count = rep.GetFeedCount("somebody");
            Assert.Equal(2, count);

            float sum = rep.GetRating("somebody");
            Assert.Equal(4, sum);
        }
    }
}
