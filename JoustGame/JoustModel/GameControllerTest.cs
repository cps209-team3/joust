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
            new Ostrich();
            new Egg();
            new Pterodactyl();
            new Buzzard();
            new Platform();  
            new Respawn();
            new Base();
            string testLine = game.Save();
            Assert.IsTrue(testLine == "Ostrich,0,3,0,0,0,0:Egg,0,0,0,0:Pterodactyl,0,315,0,0:Buzzard,3,0,0,0:Platform,0,0:Respawn,0,0:Base,0,0:");
        }
    }
}


