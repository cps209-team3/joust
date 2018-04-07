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
            game.Load(DateTime.Now.ToString("17-42-49"));
            Assert.IsTrue(game.WorldRef.objects.Count == 4);
            Assert.IsTrue(game.WorldRef.objects[0].coords.x == 5);
            Assert.IsTrue((game.WorldRef.objects[3] as Buzzard).speed == 100);

        }

        [TestMethod]
        public void Save_Default()
        {
            GameController game = new GameController();
            string testLine = game.Save();
            Assert.IsTrue(testLine == "");
        }

        [TestMethod]
        public void Save_OneObjEach()
        {
            GameController game = new GameController();
            new Ostrich() { angle = 1.5, coords = new Point(5, 9), speed = 10};
            new Egg() { angle = 0, coords = new Point(10, 10), speed = 100 };
            new Pterodactyl() { angle = 0, coords = new Point(10, 10), speed = 100 };
            new Buzzard() { angle = 0, coords = new Point(10, 10), speed = 100 };
            // new Platform();  //add later once Jacob polishes the constructors
            // new Respawn();
            // new Base();
            string testLine = game.Save();
            Assert.IsTrue(testLine == "Ostrich,0,3,10,1.5,5,9:Egg,100,0,10,10:Pterodactyl,100,0,10,10:Buzzard,100,0,10,10:");
        }
    }
}


