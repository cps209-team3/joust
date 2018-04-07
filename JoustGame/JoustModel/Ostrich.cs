using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    public class Ostrich : Entity
    {
        public int lives;
        public int score;
        public override int Value { get; set; }

        public Ostrich(Point coords, string imagePath = "Images/Player/player_stand.png")
        {
            Value = 2000;
            lives = 3;
            score = 0;
            speed = 0;
            angle = 0;
            this.imagePath = imagePath;
            this.coords = coords;
            World.Instance.objects.Add(this);
        }

        public override void Die()
        {
            World.Instance.objects.Remove(this);
        }

        public override void Update()
        {

        }

        // Serialization
        public override string Serialize()
        {
            return string.Format("Ostrich, {0}, {1}, {2}, {3}", this.score, lives, stage, this.coords);
        }

        public override void Deserialize(string data)
        {
            // set score
            // set lives
            // set stage
            // set coords
        }
    }

    [TestClass]
    public class TestOstrich
    {
        [TestMethod]
        public void TestDie()
        {
            Ostrich o = new Ostrich(new Point(500, 500);
            o.Die();
            Assert.AreEqual(new List<WorldObject> { }, World.Instance.objects);
        }

        [TestMethod]
        public void TestUpdate()
        {
            // implement when update is implemented
        }
    }
}
