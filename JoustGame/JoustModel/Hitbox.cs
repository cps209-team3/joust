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

        public Hitbox(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public Hitbox CheckCollisions()
        {
            foreach (WorldObject wo in World.Instance.objects)
            {
                // if hitboxes intersect return this one
                if (false)
                {
                    return wo.hitbox;
                }
            }
            // return null if no collisions
            return null;
        }
    }

    [TestClass]
    public class HitboxTest
    {
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
    }
}