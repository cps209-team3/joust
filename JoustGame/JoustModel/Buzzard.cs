using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    public class Buzzard : Enemy
    {
        public event EventHandler BuzzardMoveEvent;
        private Random rand;

        public override int Value { get; set; }

        public Buzzard(Point coords)
        {
            rand = new Random();
            Value = 500;
            speed = 0;
            angle = 0;
            imagePath = "Images/Enemy/buzzard_stand.png";
            this.coords = coords;
            World.Instance.objects.Add(this);
        }

        public override void Die()
        {
            World.Instance.objects.Remove(this);
        }

        public override void Update()
        {
            speed = 10;
            angle = rand.Next(360);
            if (BuzzardMoveEvent != null) // Is anyone subscribed?
                BuzzardMoveEvent(this, null); // Raise event
        }

        public override string Serialize()
        {
            return string.Format("Buzzard, {0}, {1}, {2}", this.coords, this.speed, this.angle);
        }

        public override void Deserialize(string data)
        {
            //set coords
            //set speed 
            //set angle 
        }
    }

    [TestClass]
    public class TestBuzzard
    {
        [TestMethod]
        public void TestDie()
        {
            Buzzard b = new Buzzard(new Point(500, 500));
            b.Die();
            Assert.AreEqual(new List<WorldObject> { }, World.Instance.objects);
        }

        [TestMethod]
        public void TestUpdate()
        {
            // implement when update is implemented
        }
    }
}
