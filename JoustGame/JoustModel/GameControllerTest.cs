using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    [TestClass]
    public class SerialTest
    {

        [TestMethod]
        public void Load_Default()
        {
            GameController game = new GameController();
            string[] lineLoaded;
            // populate world with objects
            string line0 = "Player: 1000, 3, 2, (300,600)";
            string line1 = "Platforms: 1, (100,300), false";
            string line2 = "Entities: 1, buzzard, (500,700), 3.6, 4.7";
            lineLoaded = game.Load("Save 2018-4-2-5-32-00.txt");

            Assert.IsTrue(lineLoaded[0] == line0);
            Assert.IsTrue(lineLoaded[1] == line1);
            Assert.IsTrue(lineLoaded[2] == line2);
        }

        [TestMethod]
        public void Save_Default()
        {
            GameController game = new GameController();
            string[] lines2save;
            // the save file needs test data
            string line0 = "Player: ";
            string line1 = "Platforms: ";
            string line2 = "Entities: ";

            lines2save = game.Save();

            Assert.IsTrue(lines2save[0] == line0);
            Assert.IsTrue(lines2save[1] == line1);
            Assert.IsTrue(lines2save[2] == line2);
        }

        [TestMethod]
        public void Save_OneObjEach()
        {
            GameController game = new GameController();
            string[] lines2save;
            // the save file needs test data
            string line0 = "Player: 1000, 3, 2, (300,600)";
            string line1 = "Platforms: 1, platform, (100,300)";
            string line2 = "Entities: 1, buzzard, (500,700), 3.6, 4.7";

            lines2save = game.Save();

            Assert.IsTrue(lines2save[0] == line0);
            Assert.IsTrue(lines2save[1] == line1);
            Assert.IsTrue(lines2save[2] == line2);
        }
    }
}


