using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HighScore
{
    
    public class HighScore_Manager
    {
        public List<Score> allScores { get; set; }
        // must be set with the "getScores" method

        private static HighScore_Manager instance = new HighScore_Manager();
        public static HighScore_Manager Instance
        {
            get { return instance; }
        }

        public void addScore(Score newScore)
        {
            // when the user's score is created, calling 
            // this method will check the score file. the 
            // file will be a simple csv with the username 
            // in the first column and the score in the 
            // second. the method will check the name to see 
            // if it appears less than three times in the file. 
            // if not, it will add the user's score. if the user 
            // appears three times, the score is added only if it 
            // higher than the lowest in which case the lowest is then
            // removed. the score is also added to the "allscores" attribute
            // of the "HighScore_Manager" in the same manner
        }

        public static List<Score> getScores(string scoreFileName)
        {
            // line by line will split the csv file and return a 
            // list of scores. called externally to sync the local copy of 
            // scores to the file's list

            List<Score> toReturn = new List<Score>();
            return toReturn;
        }
    }

    public class Score
    {
        // when the user loses the game, a screen pops 
        // up to ask for a username. using the game's score
        // and the supplied name, a score is created

        public string username;
        public int score;

        public Score(string username, int score)
        {
            this.username = username;
            this.score = score;
        }
    }

}
