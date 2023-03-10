using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.Interfaces;
using купикота.рф.Data.LogicModels;

namespace купикота.рф.Data.Logic
{
    public class HideLogic
    {
        private readonly IHideAdverts _hideAdverts;

        public HideLogic(IHideAdverts hideAdverts)
        {
            _hideAdverts = hideAdverts;
        }

        public List<LogicHideComments> GetHideComments()
        {
            return _hideAdverts.allComs();
        }

        public string GetCommentByAdvertId(int aid)
        {
            var advert = _hideAdverts.GetCommentObject(aid);
            return advert == null ? null : advert.Comment;
        }

        public LogicHideComments GetComObject(int Id)
        {
            return _hideAdverts.GetCommentObject(Id);
        }

        public void DeleteCommentByAdvertId(int aid)
        {
            _hideAdverts.DeleteComment(aid);
        }

        public void AddCommentInDB(int Id, string com)
        {
            _hideAdverts.AddComment(Id, com);
        }
    }
}
