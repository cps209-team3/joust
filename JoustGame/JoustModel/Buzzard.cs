using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    public class Buzzard : Enemy
    {
        public event EventHandler BuzzardMoveEvent;
        public event EventHandler BuzzardStateChange;
        public event EventHandler BuzzardDropEgg;
        public event EventHandler BuzzardDestroyed;

        public override int Value { get; set; }

        public static string lock_this = "Lock this please";

        private int updateGraphic;
        private double prevAngle;
        public bool droppedEgg;
        private const double SPEED = 3;
        private const double TERMINAL_VELOCITY = 4;

        public Buzzard(Point coords)
        {
            Value = 500;
            speed = SPEED;
            angle = 0;
            updateGraphic = 0;
            prevAngle = angle;
            droppedEgg = false;
            state = new EnemyStandingState();
            imagePath = "Images/Enemy/mik_red_stand.png";
            this.coords = coords;
            World.Instance.objects.Add(this);
        }

        public override void Die()
        {
            World.Instance.objects.Remove(this);
        }

        public override void Update()
        {
            // Determine the next state
            state = EnemyState.GetNextState(this);
            state.Setup();
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
            else if (state is BuzzardFleeingState) {
                if (coords.x < 3) {
                    if (BuzzardDestroyed != null)
                        BuzzardDestroyed(this, null);
                    Die();
                }
            }

            Trace.WriteLine(state.GetType().ToString());

            prevAngle = angle;

            if (updateGraphic > 3) updateGraphic = 0;
            else updateGraphic++;

            if (BuzzardMoveEvent != null) // Is anyone subscribed?
                BuzzardMoveEvent(this, null); // Raise event

            if (updateGraphic == 0) {
                if (BuzzardStateChange != null)
                    BuzzardStateChange(this, null);
            }

            if (coords.y > 450 && coords.y < 525 && coords.x > 650 && coords.x < 800 && !droppedEgg) {
                if (BuzzardDropEgg != null)
                    BuzzardDropEgg(this, null);
                droppedEgg = true;
            }
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
