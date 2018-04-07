using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class GameController
    {
        public World WorldObj { get; set; }

        public GameController()
        {
            WorldObj = World.Instance;
        }

        public void CalculateNumEnemies(int stage, ref int numBuzzards, ref int numPterodactyls)
        {
            numBuzzards = stage + 3;
            if (stage >= 5)
            {
                numPterodactyls = (stage - 4) + (stage / 2);
            }
        }

        public void SpawnEnemies(int numBuzzards, int numPterodactyls)
        {
            for (int i = 0; i < numBuzzards; i++)
            {
                Buzzard b = new Buzzard(new Point(500, 500));
                WorldObj.objects.Add(b);
            }
        }

        public string Load(string filename)
        {
            string line = "";
            // loops through save lines looking for the one that matches the provided date
            // feed line into each object class of the game activating its Deserialize method and loading each object into the game
            return line;
        }
        public string Save()
        {
            string line2save = "";
            // loop through game objects and build string by activating each's serialize method. 
            // sample complete save line:
            // {saveDate} Player: [score, lives, stage, playerPos] WorldObjects: [platforms: (int)numOfPlats, (point)coords, (bool)respawn], [Entities: (int)numOfEnts, (string)type, (point)coords, (double)speed, (double)angle]
            return line2save;
        }
    }
}
