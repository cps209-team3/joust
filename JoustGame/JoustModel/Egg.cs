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
            return string.Format("Egg,{0},{1},{2},{3}",speed, angle, coords.x, coords.y);
        }

        public override void Deserialize(string data)
        {
            string[] properties = data.Split(',');
            speed = Convert.ToDouble(properties[0]); // set speed
            angle = Convert.ToDouble(properties[1]); // set angle
            coords.x = Convert.ToDouble(properties[2]); // set x coord
            coords.y = Convert.ToDouble(properties[3]); // set y coord
        }
    }

    [TestClass]
    public class TestEgg
    {
        [TestMethod]
        public void TestDie()
        {
            Egg e = new Egg();
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
