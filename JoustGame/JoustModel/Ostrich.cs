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

        public Ostrich()
        {
            Value = 750;
            lives = 3;
            score = 0;
            World.Instance.objects.Add(this);
        }

        public override void Die()
        {
            World.Instance.objects.Remove(this);
        }

        public override void Update()
        {

        }

        //Serialization
        public override string Serialize()
        {
            return string.Format("Ostrich,{0},{1},{2},{3},{4},{5}", score, lives, speed, angle, coords.x, coords.y);
        }

        public override void Deserialize(string data)
        {
            string[] properties = data.Split(',');
            score = Convert.ToInt32(properties[1]); // set score
            lives = Convert.ToInt32(properties[2]); // set lives
            speed = Convert.ToDouble(properties[3]); // set speed
            angle = Convert.ToDouble(properties[4]); // set angle
            coords.x = Convert.ToDouble(properties[5]); // set x coord
            coords.y = Convert.ToDouble(properties[6]); // set y coord
        }
    }

    [TestClass]
    public class TestOstrich
    {
        [TestMethod]
        public void TestDie()
        {
            Ostrich o = new Ostrich();
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
