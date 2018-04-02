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

        public void Die()
        {
            World.Instance.objects.Remove(this);
            // Play death animation
        }

        public void Update()
        {
            // Move based on input/AI
            // Check for collisions
        }
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
            Point testPoint = new Point();
            testPoint.x = 450;
            testPoint.y = 500;
            Assert.AreEqual(testPoint, e.coords);
        }

        [TestMethod]
        public void TestMoveLeft()
        {
            Entity e = new Ostrich();
            // set initial speed, angle, and position
            e.Update();
            Point testPoint = new Point();
            testPoint.x = 450;
            testPoint.y = 500;
            Assert.AreEqual(testPoint, e.coords);
        }

        [TestMethod]
        public void TestMoveRight()
        {
            Entity e = new Ostrich();
            // set initial speed, angle, and position
            e.Update();
            Point testPoint = new Point();
            testPoint.x = 450;
            testPoint.y = 500;
            Assert.AreEqual(testPoint, e.coords);
        }

        [TestMethod]
        public void TestMoveDown()
        {
            Entity e = new Ostrich();
            // set initial speed, angle, and position
            e.Update();
            Point testPoint = new Point();
            testPoint.x = 450;
            testPoint.y = 500;
            Assert.AreEqual(testPoint, e.coords);
        }
    }
}