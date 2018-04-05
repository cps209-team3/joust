using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    public class Pterodactyl : Enemy
    {
        public override int Value { get; set; }

        public Pterodactyl()
        {
            Value = 1000;
            World.Instance.objects.Add(this);
        }

        public override void Die()
        {
            World.Instance.objects.Remove(this);
        }

        public override void Update()
        {

        }

        public override string Serialize()
        {
            return string.Format("Pterodactyl, {0}, {1}, {2}", this.coords, this.speed, this.angle);
        }

        public override void Deserialize(string data)
        {
            // set coords
            // set speed
            // set angle
        }
    }

    [TestClass]
    public class TestPterodactyl
    {
        [TestMethod]
        public void TestDie()
        {
            Pterodactyl p = new Pterodactyl();
            p.Die();
            Assert.AreEqual(new List<WorldObject> { }, World.Instance.objects);
        }

        [TestMethod]
        public void TestUpdate()
        {
            // implement when update is implemented
        }
    }
}
