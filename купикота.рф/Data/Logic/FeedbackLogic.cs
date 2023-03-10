using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;

namespace купикота.рф.Data.Logic
{
    public class FeedbackLogic
    {
        private readonly IFeedbacks _feed;

        public FeedbackLogic(IFeedbacks feed)
        {
            _feed = feed;
        }

        public IEnumerable<LogicFeedbacks> AllFeeds()
        {
            return _feed.GetAllFeeds();
        }

        public IEnumerable<LogicFeedbacks> NullFeeds()
        {
            return _feed.GetNullFeeds();
        }

        public IEnumerable<LogicFeedbacks> GetOwnerFeeds(string OwnerId)
        {
            return _feed.GetOwnerFeeds(OwnerId);
        }

        public LogicFeedbacks UpdateFeed(Feedbacks feedback)
        {
            feedback.FbDate = DateTime.Now;
            return _feed.UpdateFeed(feedback);
        }

        public LogicFeedbacks GetFeedbackById (int fid)
        {
            return _feed.GetFeed(fid);
        }

        public void DeleteFeed(int FeedId)
        {
            _feed.DeleteFeed(FeedId);
        }

        public float GetRating(string OwnerId)
        {
            var feeds = _feed.GetOwnerFeeds(OwnerId).ToList();
            int count = feeds.Count;
            if (count == 0)
                return 5;
            float sum = 0;
            foreach (var f in feeds)
                if (f.Feedback != "default")
                    sum += f.Rate;
                else
                    count -= 1;
            return sum / count;
                
        }

        public int GetFeedCount(string OwnerId)
        {
            var count = _feed.GetOwnerFeeds(OwnerId).ToList().Count;
            return count;

        }
    }
}
