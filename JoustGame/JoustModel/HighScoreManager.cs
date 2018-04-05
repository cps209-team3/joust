using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    // handles saving and loading high scores
    public class HighScoreManager
    {
        public List<Score> AllScores { get; set; }
        public int lowest;

        private HighScoreManager(List<Score> allscores)
        {
            this.AllScores = allscores;
        }

        private static HighScoreManager instance = new HighScoreManager();
        public static HighScoreManager Instance
        {
            get { return instance; }
        }

        public void AddScore(Score newScore)
        {
            // when a score is created, calling AddScore first 
            // checks to see if the score is higher than the lowest
            // score on file. if it is, the Score will replace the lowest 
            // score and a new lowest score must be set
        }

        private static List<Score> GetScores(string scoreFileName)
        {
            // line by line will split the csv file and return a 
            // list of the top ten scores. called by the constructor

            List<Score> toReturn = new List<Score>();

            // some lines to set `toReturn` to a list of the top ten scores in the file
            return toReturn;
        }
    }
}
