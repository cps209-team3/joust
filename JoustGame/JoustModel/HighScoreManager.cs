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
        // must be set with the "getScores" method
        public List<Score> AllScores { get; set; }
        public string path = "\\JoustGame";

        private HighScoreManager(List<Score> allscores)
        {
            this.AllScores = allscores;
        }

        private static HighScoreManager instance = new HighScoreManager(GetScores("filename"));
        public static HighScoreManager Instance
        {
            get { return instance; }
        }

        public void AddScore(Score newScore)
        {
            // when the user's score is created, calling 
            // this method will check the score file. the 
            // file will be a simple csv with the username 
            // in the first column and the score in the 
            // second.
            // its place in the file is determined by the `score` attribute.
            // the score is also added to the "allscores" attribute
            // of the "HighScore_Manager" in the same manner
        }

        public static List<Score> GetScores(string scoreFileName)
        {
            // line by line will split the csv file and return a 
            // list of the top ten scores. called externally to sync the local copy of 
            // scores to the file's list
            List<Score> toReturn = new List<Score>();

            // some lines to set `toReturn` to a list of the top ten scores in the file
            return toReturn;
        }
    }
}
