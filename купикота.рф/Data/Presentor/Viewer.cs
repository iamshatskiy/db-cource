using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using купикота.рф.Data.LogicModels;

namespace купикота.рф.Data.Presentor
{
    public class Viewer
    {
        public Viewer()
        {

        }

        public int ConsoleInput(string inp)
        {
            Console.WriteLine(inp);
            int id = Convert.ToInt32(Console.ReadLine());
            return id;
        }

        public void ConsoleOutput(string str)
        {
            Console.WriteLine(str);
        }

        public string ConvertBreedToString(LogicBreed logicBreed)
        {
            return Convert.ToString(logicBreed.Id) + " " + Convert.ToString(logicBreed.Breed_name);
        }

        public string ConvertComToString(LogicHideComments logicCom)
        {
            return Convert.ToString(logicCom.Id) + " " + Convert.ToString(logicCom.AdvertId) + " " + Convert.ToString(logicCom.Comment);
        }

        public string ConvertFeedToString(LogicFeedbacks logicCom)
        {
            return Convert.ToString(logicCom.Id) + " " + Convert.ToString(logicCom.BuyerId) + " " + Convert.ToString(logicCom.OwnerId) +
                " " + Convert.ToString(logicCom.Feedback) + " " + Convert.ToString(logicCom.Rate) + " " + Convert.ToString(logicCom.FbDate);
        }
    }
}
