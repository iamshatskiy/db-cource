using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;
using купикота.рф.Models.FeedbackViewModels;

namespace купикота.рф.Data.Interfaces
{
    public interface IFeedbacks
    {
        IEnumerable<LogicFeedbacks> GetAllFeeds();
        IEnumerable<LogicFeedbacks> GetNullFeeds();
        IEnumerable<LogicFeedbacks> GetOwnerFeeds(string OwnerId);
        LogicFeedbacks UpdateFeed(Feedbacks feedback);
        void DeleteFeed(int FeedbackId);
        LogicFeedbacks GetFeed(int fid);
    }
}
