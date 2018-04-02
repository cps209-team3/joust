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

        public string Load(string date)
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
            return string line2save;
        }
    }
    
    [TestClass]
    public class SerialTest
    {

        [TestMethod]
        public void Save_Default()
        {
            GameController game = new GameController();
            // populate world with objects
            string result = "{saveDate} Player: [score, lives, stage, playerPos] WorldObjects: [platforms: (int)numOfPlats, (point)coords, (bool)respawn], [Entities: (int)numOfEnts, (string)type, (point)coords, (double)speed, (double)angle]"
            line = game.Load();
            Assert.IsTrue(result == line);
        }

        [TestMethod]
        public void Load_Default()
        {
            GameController game = new GameController();
            // the save file needs test data
            string result = string result = "{saveDate} Player: [score, lives, stage, playerPos] WorldObjects: [platforms: (int)numOfPlats, (point)coords, (bool)respawn], [Entities: (int)numOfEnts, (string)type, (point)coords, (double)speed, (double)angle]";
            line2save = game.Save();
            Assert.IsTrue(result == line2save);
        }
    }
}
