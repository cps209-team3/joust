using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    public abstract class Entity : WorldObject
    {
        public double speed;
        public double angle;
        
        public abstract int Value { get; set; }

        public abstract void Die();

        public abstract void Update();
    }

    [TestClass]
    public class EntityTest
    {
        [TestMethod]
        public void TestMoveUp()
        {
            Entity e = new Ostrich();
            // set initial speed, angle, and position
            e.Update();
            Point testPoint = new Point(450, 500);
            Assert.AreEqual(testPoint, e.coords);
        }

        [TestMethod]
        public void TestMoveLeft()
        {
            Entity e = new Ostrich();
            // set initial speed, angle, and position
            e.Update();
            Point testPoint = new Point(450, 500);
            Assert.AreEqual(testPoint, e.coords);
        }

        [TestMethod]
        public void TestMoveRight()
        {
            Entity e = new Ostrich();
            // set initial speed, angle, and position
            e.Update();
            Point testPoint = new Point(450, 500);
            Assert.AreEqual(testPoint, e.coords);
        }

        [TestMethod]
        public void TestMoveDown()
        {
            Entity e = new Ostrich();
            // set initial speed, angle, and position
            e.Update();
            Point testPoint = new Point(450, 500);
            Assert.AreEqual(testPoint, e.coords);
        }
    }
}