using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;

namespace купикота.рф.Data.Interfaces
{
    public interface IHideAdverts
    {
        List<LogicHideComments> allComs();
        LogicHideComments GetCommentObject(int aid);
        void AddComment(int AdvertId, string Comment);
        void DeleteComment(int AdvertId);
    }
}
