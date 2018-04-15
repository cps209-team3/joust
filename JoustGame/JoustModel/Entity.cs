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
        public double nSpeed;
        public double nAngle;

        public abstract int Value { get; set; }

        public abstract void Die();

        public abstract void Update();

        public void CheckCollision()
        {
            foreach (WorldObject wo in World.Instance.objects)
            {
                if (wo.ToString() != this.ToString()) // Don't collide with itself!
                {
                    WorldObject objHit = hitbox.CheckCollisions(wo);
                    if (objHit != null)
                    {
                        // Collision detected!
                        Console.WriteLine("Collision detected between " + this.ToString() + " and " + objHit.ToString());
                    }
                }

            }
        }
    }    
        

    [TestClass]
    public class EntityTest
    {
        [TestMethod]
        public void TestMoveUp()
        {
            Entity e = new Ostrich();
            e.coords = new Point(500, 500);
            // set initial speed, angle, and position
            e.Update();
            Point testPoint = new Point(450, 500);
            Assert.AreEqual(testPoint, e.coords);
        }

        [TestMethod]
        public void TestMoveLeft()
        {
            Entity e = new Ostrich();
            e.coords = new Point(500, 500);
            // set initial speed, angle, and position
            e.Update();
            Point testPoint = new Point(450, 500);
            Assert.AreEqual(testPoint, e.coords);
        }

        [TestMethod]
        public void TestMoveRight()
        {
            Entity e = new Ostrich();
            e.coords = new Point(500, 500);
            // set initial speed, angle, and position
            e.Update();
            Point testPoint = new Point(450, 500);
            Assert.AreEqual(testPoint, e.coords);
        }

        [TestMethod]
        public void TestMoveDown()
        {
            Entity e = new Ostrich();
            e.coords = new Point(500, 500);
            // set initial speed, angle, and position
            e.Update();
            Point testPoint = new Point(450, 500);
            Assert.AreEqual(testPoint, e.coords);
        }
    }
}