using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

}
