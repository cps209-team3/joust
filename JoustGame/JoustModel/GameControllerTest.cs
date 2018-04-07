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
            game.Load("Save2018-4-2-5-32-00");

        }

        [TestMethod]
        public void Save_Default()
        {
            GameController game = new GameController();
            game.Save();
        }

        [TestMethod]
        public void Save_OneObjEach()
        {
            GameController game = new GameController();
            new Ostrich() { angle = 1.5, coords = new Point(5, 9), lives = 3, score = 1000, speed = 10};
            new Egg();
            new Pterodactyl();
            new Buzzard();
            game.Save();
        }
    }
}


