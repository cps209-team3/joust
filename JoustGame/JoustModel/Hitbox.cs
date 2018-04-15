using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    public class Hitbox
    {

        public double width;
        public double height;
        public double xPos;
        public double yPos;

        public Hitbox(double width, double height, double x, double y)
        {
            this.width = width;
            this.height = height;
            xPos = x;
            yPos = y;
        }

        public WorldObject CheckCollisions(WorldObject wo)
        {
            if (xPos < wo.coords.x + wo.hitbox.width && xPos + width > wo.coords.x && yPos < wo.coords.y + wo.hitbox.height && height + yPos > wo.coords.y)
            {   
                // collision detected with "wo"
                return wo;
            }
            // return null if no collisions
            return null;
        }
    }

    [TestClass]
    public class HitboxTest
    {
        /*
        [TestMethod]
        public void TestNoCollision()
        {
            Ostrich a = new Ostrich();
            a.coords = new Point(500, 500);
            Buzzard b = new Buzzard();
            b.coords = new Point(600, 600);
            a.hitbox = new Hitbox(50, 50);
            b.hitbox = new Hitbox(50, 50);
            World.Instance.objects.Add(a);
            World.Instance.objects.Add(b);
            Assert.IsTrue(a.hitbox.CheckCollisions() == null);
        }

        [TestMethod]
        public void TestCollisions()
        {
            Ostrich a = new Ostrich();
            a.coords = new Point(500, 500);
            Buzzard b = new Buzzard();
            b.coords = new Point(510, 510);
            a.hitbox = new Hitbox(50, 50);
            b.hitbox = new Hitbox(50, 50);
            World.Instance.objects.Add(a);
            World.Instance.objects.Add(b);
            Assert.AreEqual(b.hitbox, a.hitbox.CheckCollisions());
        }
        */
    }
}