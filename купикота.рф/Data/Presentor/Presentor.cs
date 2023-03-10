using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.Logic;
using купикота.рф.Data.LogicModels;
using купикота.рф.Data.Models;
using купикота.рф.Models;

namespace купикота.рф.Data.Presentor
{
    public class Presentor
    {
        private readonly BreedLogic _breedLogic;
        private readonly HideLogic _hideLogic;
        private readonly FeedbackLogic _feedbackLogic;
        private readonly DealHistoryLogic _dealHistoryLogic;
        private readonly DealLogic _dealLogic;
        private readonly Viewer _viewer;
        

        public Presentor(BreedLogic breedLogic, 
            HideLogic hideLogic, FeedbackLogic feedbackLogic, DealHistoryLogic dealHistoryLogic, DealLogic dealLogic, Viewer viewer)
        {
            _dealLogic = dealLogic;
            _breedLogic = breedLogic;
            _hideLogic = hideLogic;
            _feedbackLogic = feedbackLogic;
            _dealHistoryLogic = dealHistoryLogic;
            _viewer = viewer;
        }

        public string AllBreeds()
        {
            var breeds = _breedLogic.logicBreeds();
            foreach (var breed in breeds)
            {
                var str = _viewer.ConvertBreedToString(breed);
                _viewer.ConsoleOutput(str);
            }
            return "NICE";
        }

        public void GetBreedById()
        {
            int id = _viewer.ConsoleInput("Введите ID:");
            var breed = _breedLogic.GetBreedNameById(id);
            if (breed == null)
                _viewer.ConsoleOutput("Не найдена порода с заданным id!");
            else
                _viewer.ConsoleOutput(breed);
        }

        public string AllComs()
        {
            var coms = _hideLogic.GetHideComments();
            foreach (var com in coms)
            {
                var str = _viewer.ConvertComToString(com);
                _viewer.ConsoleOutput(str);
            }
            return "NICE";
        }

        public void GetComByAdvertId()
        {
            int id = _viewer.ConsoleInput("Введите ID объявления:");
            var hide = _hideLogic.GetCommentByAdvertId(id);
            if (hide == null)
                _viewer.ConsoleOutput("Не найден комментарий с заданным id объявления!");
            else
                _viewer.ConsoleOutput(hide);
        }

        public void GetComById()
        {
            int id = _viewer.ConsoleInput("Введите ID:");
            var hide = _hideLogic.GetComObject(id);
            if (hide == null)
                _viewer.ConsoleOutput("Не найден комментарий с заданным id!");
            else
                _viewer.ConsoleOutput(_viewer.ConvertComToString(hide));
        }

        public void GetAllFeeds()
        {
            var coms = _feedbackLogic.AllFeeds();
            foreach (var com in coms)
            {
                var str = _viewer.ConvertFeedToString(com);
                _viewer.ConsoleOutput(str);
            }
        }

        public void GetFeedById()
        {
            int id = _viewer.ConsoleInput("Введите ID:");
            var hide = _feedbackLogic.GetFeedbackById(id);
            if (hide == null)
                _viewer.ConsoleOutput("Не найден отзыв с заданным id!");
            else
                _viewer.ConsoleOutput(_viewer.ConvertFeedToString(hide));
        }

        public void GetNullFeeds()
        {
            var coms = _feedbackLogic.NullFeeds();
            foreach (var com in coms)
            {
                var str = _viewer.ConvertFeedToString(com);
                _viewer.ConsoleOutput(str);
            }
        }



    }
}
