using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    public class Egg : Enemy
    {
        public event EventHandler EggMoveEvent;
        public event EventHandler EggStateChange;
        public event EventHandler EggHatched;
        public event EventHandler EggDestroyed;

        public override int Value { get; set; }
        public int seconds;
        public int milliseconds;
        public bool mounted;

        private int updateGraphic;
        private bool alreadyHatched;

        public Egg(Point coords)
        {
            Value = 250;
            updateGraphic = 0;
            seconds = 0;
            milliseconds = 3;
            alreadyHatched = false;
            mounted = false;
            imagePath = "Images/Enemy/egg1.png";
            this.coords = coords;
            World.Instance.objects.Add(this);
        }

        public override void Die()
        {
            World.Instance.objects.Remove(this);
        }

        public override void Update()
        {
            state = EnemyState.GetNextState(this);
            state.Setup();

            if (state is EggHatchedState) {
                if (milliseconds > 30 && !alreadyHatched) {
                    if (EggHatched != null) {
                        EggHatched(this, null);
                        alreadyHatched = true;
                    }
                }
            }

            if (mounted) {
                if (EggDestroyed != null)
                    EggDestroyed(this, null);
                Die();
            }

            if (updateGraphic > 3) updateGraphic = 0;
            else updateGraphic++;

            if (EggMoveEvent != null) // Is anyone subscribed?
                EggMoveEvent(this, null); // Raise event

            if (updateGraphic == 0) {
                if (EggStateChange != null)
                    EggStateChange(this, null);
            }
        }

        //Serialization
        public override string Serialize()
        {
            return string.Format("Egg, {0}, {1}, {2}", this.coords, this.speed, this.angle);
        }

        public override void Deserialize(string data)
        {
            // set coords
            // set speed
            // set angle
        }
    }

    [TestClass]
    public class TestEgg
    {
        [TestMethod]
        public void TestDie()
        {
            Egg e = new Egg(new Point(500, 500));
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
