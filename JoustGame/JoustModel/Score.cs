using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoustModel
{
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

        public void addScore()
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
            // removed
        }

        public static List<Score> getScores(string scoreFileName)
        {
            // line by line will split the csv file and return a 
            // list of scores, turning the number scores into ints
            // and keeping the names strings

            List<Score> toReturn = new List<Score>();
            return toReturn;
        }
    }
}
