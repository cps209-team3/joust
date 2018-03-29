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

        public void Load(string fileName, string user)
        {
            // search for section in file according to username
            // Get lives count
            // Get score number
            // Get level number

        }
        public void Save(string fileName)
        {
            // write username
            // write score data under "score"
            // write lives count
            // write level number

            // Sample save line:
            // USERNAME, SCORE, LIVES, LEVEL
            // NerdDestroyer9000, 999999, 3, 10

        }
    }
}
