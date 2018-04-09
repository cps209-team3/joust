using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    public class Egg : Enemy
    {
        public override int Value { get; set; }
        
        public Egg()
        {
            Value = 250;
            imagePath = "Images/Platform/egg1.png";
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
            return string.Format("Egg,{0},{1},{2},{3},{4},{5}",speed, angle, coords.x, coords.y, acceleration, accelerationAngle);
        }

        public override void Deserialize(string data)
        {
            string[] properties = data.Split(',');
            speed = Convert.ToDouble(properties[1]); // set speed
            angle = Convert.ToDouble(properties[2]); // set angle
            coords.x = Convert.ToDouble(properties[3]); // set x coord
            coords.y = Convert.ToDouble(properties[4]); // set y coord
            acceleration = Convert.ToDouble(properties[5]); // set acceleration
            accelerationAngle = Convert.ToDouble(properties[6]); // set accelerationAngle
        }
    }

    [TestClass]
    public class TestEgg
    {
        [TestMethod]
        public void TestDie()
        {
            Egg e = new Egg();
            e.coords = new Point(500, 500);
            e.Die();
            Assert.AreEqual(new List<WorldObject> { }, World.Instance.objects);
        }

        [TestMethod]
        public void TestUpdate()
        {
            // implement when update is implemented
        }
    }
}
