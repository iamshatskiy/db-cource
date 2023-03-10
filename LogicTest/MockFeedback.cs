using System;
using System.Collections.Generic;
using System.Text;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;

namespace LogicTest
{
    class MockFeedback : IFeedbacks
    {
        public MockFeedback()
        {
        }

        public void DeleteFeed(int FeedbackId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LogicFeedbacks> GetAllFeeds()
        {
            throw new NotImplementedException();
        }

        public LogicFeedbacks GetFeed(int fid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LogicFeedbacks> GetNullFeeds()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LogicFeedbacks> GetOwnerFeeds(string OwnerId)
        {
            List<LogicFeedbacks> logicFeedbacks = new List<LogicFeedbacks>();
            logicFeedbacks.Add(new LogicFeedbacks { Rate = 5 });
            logicFeedbacks.Add(new LogicFeedbacks { Rate = 3 });
            return logicFeedbacks;
        }

        public LogicFeedbacks UpdateFeed(Feedbacks feedback)
        {
            throw new NotImplementedException();
        }
    }
}
