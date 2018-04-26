//-----------------------------------------------------------
//  File:   Entity.cs
//  Desc:   Holds the Entity class
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    //-----------------------------------------------------------
    //  Desc:   Parent class for all Entities of the game.
    //          Defines movement variables and the collision test
    //----------------------------------------------------------- 
    public abstract class Entity : WorldObject
    {
        // speed of the entity
        public double speed;
        // angle of the entity
        public double angle;
        // new speed of the entity
        public double nSpeed;
        // new angle of the entity
        public double nAngle;
        // Value of the entity
        public abstract int Value { get; set; }
        // Death method all entities must implement
        public abstract void Die();
        // Update method all entities must implement
        public abstract void Update();


        /// <summary>
        /// Checks for collision, returns the point of collision
        /// </summary>
        /// <returns> point of collision (0) and type of object collided with (2) </returns>
        public WorldObject CheckCollision()
        {
            foreach (WorldObject wo in World.Instance.objects)
            {       // Don't collide with itself! and check for collision
                if (wo.ToString() != this.ToString() && (coords.x < wo.coords.x + wo.width && coords.x + width > wo.coords.x && coords.y < wo.coords.y + wo.height && height + coords.y > wo.coords.y))
                {
                    return wo;
                }
            }
            return null; // no collision
        }

        /// <summary>
        /// calculates the smallest movement vector the object has to made in order to not be colliding for the object that has been collided with
        /// </summary>
        /// <param name="wo"> Object that has been collided with </param>
        /// <returns> smallest movement the the object must make to not be colliding </returns>
        public Point FindMinTV(WorldObject wo)
        {
            // bottom right corner of the object
            Point bRight = new Point(coords.x - (wo.coords.x + wo.width), 0);
            // top right corner of the object
            Point tRight = new Point((coords.x + width) - wo.coords.x, 0);
            // bottom left corner of the object
            Point bLeft = new Point(0, coords.y - (wo.coords.y + wo.height));
            // top left corner of the object
            Point tLeft = new Point(0, (coords.y + height) - wo.coords.y);
            List<Point> cornerVectors = new List<Point> { bRight, tRight, bLeft, tLeft };
            // Smallest vector of movement the object has to make
            Point minTV = tLeft;
            foreach (Point p in cornerVectors) // find smallest vector
            {
                if (Math.Sqrt(Math.Pow(minTV.x, 2) + Math.Pow(minTV.y, 2)) > Math.Sqrt(Math.Pow(p.x, 2) + Math.Pow(p.y, 2)))
                {
                    minTV = p;
                }
            }
            return minTV;
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