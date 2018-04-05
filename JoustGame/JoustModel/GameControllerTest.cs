using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
