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


        /// <summary>
        /// Checks for collision, returns the point of collision
        /// </summary>
        /// <returns> point of collision </returns>
        public Point CheckCollision()
        {
            foreach (WorldObject wo in World.Instance.objects)
            {       // Don't collide with itself! and check for collision

                if (wo.ToString() != this.ToString() && (coords.x < wo.coords.x + wo.width && coords.x + width > wo.coords.x && coords.y < wo.coords.y + wo.height && height + coords.y > wo.coords.y))
                {
                    if (wo.ToString() == "Buzzard") // these statements need to be reorganized later. (Implement die method for enemies class, and more enemy checks to ostrich code)
                    {
                        (wo as Buzzard).stateMachine.Change("flee");
                        return null;
                    }
                    else if (wo.ToString() == "Pterodactyl")
                    {
                        (wo as Pterodactyl).Die();
                        return null;
                    }
                    else
                    {
                        Point leftV = new Point(coords.x - (wo.coords.x + wo.width), 0);
                        Point rightV = new Point((coords.x + width) - wo.coords.x, 0);
                        Point topV = new Point(0, coords.y - (wo.coords.y + wo.height));
                        Point bottomV = new Point(0, (coords.y + height) - wo.coords.y);
                        List<Point> cornerVectors = new List<Point> { leftV, rightV, topV, bottomV };
                        Point minTV = leftV;
                        foreach (Point p in cornerVectors)
                        {
                            if (Math.Sqrt(Math.Pow(minTV.x, 2) + Math.Pow(minTV.y, 2)) > Math.Sqrt(Math.Pow(p.x, 2) + Math.Pow(p.y, 2)))
                            {
                                minTV = p;
                            }
                        }
                        return minTV;
                    }

                }
            }
            return null; // no collision
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