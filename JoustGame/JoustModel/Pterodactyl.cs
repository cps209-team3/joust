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
        public event EventHandler PterodactylMoveEvent;
        public event EventHandler PterodactylStateChange;
        public event EventHandler PterodactylDestroyed;

        public bool charging;


        private const double SPEED = 6;
        private const double TERMINAL_VELOCITY = 7;
        private double prevAngle;
        private int updateGraphic;
        private int chargeTime;

        public override int Value { get; set; }

        public Pterodactyl(Point coords)
        {
            Value = 1000;
            speed = 0;
            angle = 315;
            prevAngle = angle;
            updateGraphic = 0;
            charging = false;
            chargeTime = 0;
            state = new EnemyFallingState() { StateEnemy = this, Angle = (int)angle };
            imagePath = "Images/Enemy/pterodactyl_fly1.png";
            this.coords = coords;
            World.Instance.objects.Add(this);
        }

        public override void Die()
        {
            World.Instance.objects.Remove(this);
        }

        public override void Update()
        {
            if (!charging) {
                state = EnemyState.GetNextState(this);
                state.Setup();
            }

            // Handle gravity
            if (state is EnemyFlappingState) {
                if (speed > TERMINAL_VELOCITY) {
                    if (prevAngle == 225 || prevAngle == 270 || prevAngle == 315) speed = 0.05;
                    else speed = TERMINAL_VELOCITY;
                }
            }
            else if (state is EnemyFallingState) {
                if (speed > TERMINAL_VELOCITY) {
                    if (prevAngle == 45 || prevAngle == 90 || prevAngle == 135) speed = 0.05;
                    else speed = TERMINAL_VELOCITY;
                }
            }
            else if (state is EnemyRunningState) {
                speed = SPEED;
            }
            else if (state is PterodactylDestroyedState) {
                if (PterodactylDestroyed != null)
                    PterodactylDestroyed(this, null);
                Die();
            }
            if (state is PterodactylChargeState) {
                charging = true;
                chargeTime++;
                speed = 20;
                if (chargeTime > 40) {
                    charging = false;
                }
            }

            prevAngle = angle;

            if (updateGraphic > 3) updateGraphic = 0;
            else updateGraphic++;

            if (PterodactylMoveEvent != null) // Is anyone subscribed?
                PterodactylMoveEvent(this, null); // Raise event

            if (updateGraphic == 0) {
                if (PterodactylStateChange != null)
                    PterodactylStateChange(this, null);
            }
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
            Pterodactyl p = new Pterodactyl(new Point(500, 500));
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
