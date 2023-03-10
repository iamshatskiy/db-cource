using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;

namespace купикота.рф.Data.Repository
{
    public class HideRepository : IHideAdverts
    {
        private readonly ApplicationDbContext applicationDbContext;

        List<LogicHideComments> logicHideComments;

        public HideRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
            logicHideComments = new List<LogicHideComments> { };
            foreach (var com in applicationDbContext.HideComments)
                ((List<LogicHideComments>)logicHideComments).Add(new LogicHideComments {Id = com.Id, AdvertId = com.AdvertId, Comment = com.Comment});
        }

        public List<LogicHideComments> allComs() => logicHideComments;

        public void AddComment(int AdvertId, string Comment)
        {
            applicationDbContext.HideComments.Add(new HideComments
            {
                AdvertId = AdvertId,
                Comment = Comment
            });
            
            applicationDbContext.SaveChanges();
            var last_com = applicationDbContext.HideComments.LastOrDefault();
            ((List<LogicHideComments>)logicHideComments).Add(new LogicHideComments { Id = last_com.Id, AdvertId = last_com.AdvertId, Comment = last_com.Comment });
        }

        public void DeleteComment(int AdvertId)
        {
            var row = applicationDbContext.HideComments.FirstOrDefault(p => p.AdvertId == AdvertId);
            if (row != null)
                applicationDbContext.HideComments.Remove(row);
            applicationDbContext.SaveChanges();
            var listItem = logicHideComments.Single(r => r.AdvertId == AdvertId);
            if (listItem != null)
                logicHideComments.Remove(listItem);
        }

        public LogicHideComments GetCommentObject(int aid)
        {
            var row = applicationDbContext.HideComments.FirstOrDefault(p => p.AdvertId == aid);
            if (row == null)
                return null;
            return new LogicHideComments { Id = row.Id, AdvertId = row.AdvertId, Comment = row.Comment };
        }
    }
}
