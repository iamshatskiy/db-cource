using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;

namespace купикота.рф.Data.Repository
{
    public class FeedbackRepository : IFeedbacks
    {
        private readonly ApplicationDbContext applicationDbContext;

        List<LogicFeedbacks> logicFeedbacks;

        public FeedbackRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
            logicFeedbacks = new List<LogicFeedbacks>{ };
            foreach (var feed in applicationDbContext.Feedbacks)
                ((List<LogicFeedbacks>)logicFeedbacks).Add(new LogicFeedbacks
                {
                    Id = feed.Id,
                    BuyerId = feed.BuyerId,
                    FbDate = feed.FbDate,
                    Feedback = feed.Feedback,
                    OwnerId = feed.OwnerId,
                    Rate = feed.Rate
                });
        }

        public void DeleteFeed(int FeedbackId)
        {
            var feed = applicationDbContext.Feedbacks.FirstOrDefault(p => p.Id == FeedbackId);
            if (feed != null)
            {
                applicationDbContext.Feedbacks.Remove(feed);
                applicationDbContext.SaveChanges();
                var logic_feed = logicFeedbacks.Single(p => p.Id == FeedbackId);
                if (logic_feed != null)
                    logicFeedbacks.Remove(logic_feed);
            }
        }

        public IEnumerable<LogicFeedbacks> GetAllFeeds() => logicFeedbacks;
        

        public LogicFeedbacks GetFeed(int fid)
        {
            var feed = applicationDbContext.Feedbacks.Where(p => p.Id == fid).FirstOrDefault();
            if (feed != null)
                return new LogicFeedbacks { Id = feed.Id, BuyerId = feed.BuyerId, FbDate =feed.FbDate, Feedback = feed.Feedback, OwnerId = feed.OwnerId, Rate = feed.Rate };
            return null;
        }

        public IEnumerable<LogicFeedbacks> GetNullFeeds()
        {
            var logicfeeds = logicFeedbacks.Where(p => p.Feedback == "default");
            return logicfeeds;
        }

        public IEnumerable<LogicFeedbacks> GetOwnerFeeds(string OwnerId)
        {
            var feeds = logicFeedbacks.Where(p => p.OwnerId == OwnerId);
            return feeds;
        }

        public LogicFeedbacks UpdateFeed(Feedbacks feedback)
        {
            Feedbacks feed = applicationDbContext.Feedbacks.Where(p => p.Id == feedback.Id ).FirstOrDefault();
            if (feed != null)
            {
                feed.Feedback = feedback.Feedback;
                feed.FbDate = feedback.FbDate;
                feed.Rate = feedback.Rate;
                applicationDbContext.SaveChanges();

                var logicfeed = logicFeedbacks.Where(p => p.Id == feedback.Id).FirstOrDefault();
                logicfeed.Feedback = feedback.Feedback;
                logicfeed.FbDate = feedback.FbDate;
                logicfeed.Rate = feedback.Rate;
                return logicfeed;
            }
            return null;
        }
    }
}
