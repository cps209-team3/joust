using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JoustModel
{

    public class Score
    {
        // when the user loses the game, a screen pops 
        // up to ask for a username. using the game's score
        // and the supplied name, a score is created

        public int points;
        public string username;

        public Score(int points, string username)
        {
            this.points = points;
            this.username = username;

            this.username = this.username.TrimEnd('\r', '\n');
        }

        public string Serialize()
        {
            return this.points.ToString() + " || " + this.username.ToString();
        }

        public override string ToString()
        {
            // for ease of checking debugging variable values

            return this.points.ToString() + " || " + this.username.ToString();
        }

        public static bool Equals_Mine(Score expected, Score actual)
        {

            if ((expected.points == actual.points) && (expected.username == actual.username))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
