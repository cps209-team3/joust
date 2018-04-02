using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    class GameController
    {
        public World WorldObj { get; set; }

        public GameController()
        {
            WorldObj = World.Instance;
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
    
    [TestClass]
    public class SerialTest
    {

        [TestMethod]
        public void Save_Default()
        {
            GameController game = new GameController();
            string line;
            // populate world with objects
            string result = "Player: [1000, 3, 2, (300,600)] WorldObjects: [platforms: 1, (100,300), false], [Entities: 1, buzzard, (500,700), 3.6, 4.7]";
            line = game.Load("Save 2018-4-2-5-32-00");
            Assert.IsTrue(result == line);
        }

        [TestMethod]
        public void Load_Default()
        {
            GameController game = new GameController();
            string line2save;
            // the save file needs test data
            string result = "Player: [1000, 3, 2, (300,600)] WorldObjects: [platforms: 1, (100,300), false], [Entities: 1, buzzard, (500,700), 3.6, 4.7]";
            line2save = game.Save();
            Assert.IsTrue(result == line2save);
        }
    }
}
