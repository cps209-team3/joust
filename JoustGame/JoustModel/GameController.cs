using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    class GameController
    {
        public World world { get; set; }

        public GameController()
        {
            world = new World();
        }

        public void Load(string date)
        {
            // loops through save lines looking for the one that matches the provided date
            // feed line into each object class of the game activating its Deserialize method and loading each object into the game

        }
        public void Save()
        {
            // loop through game objects and build string by activating each's serialize method. 
            // sample complete save line:
            // {saveDate} Player: [score, lives, stage, playerPos] WorldObjects: [platforms: (int)numOfPlats, (point)coords, (bool)respawn], [Entities: (int)numOfEnts, (string)type, (point)coords, (double)speed, (double)angle]
            
        }
    }
}
