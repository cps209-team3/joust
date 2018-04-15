using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    public class Buzzard : Enemy
    {
        // Event handlers to notify the view
        public event EventHandler BuzzardMoveEvent;
        public event EventHandler BuzzardStateChange;
        public event EventHandler BuzzardDropEgg;
        public event EventHandler BuzzardDestroyed;

        // Public property Value (used to determine points awarded upon destroying)
        public override int Value { get; set; }
        // Public property Color (used to determine the Mik color)
        public string Color { get { return color; } }

        // Public instance variables
        public bool droppedEgg;
        // Private instance variables
        private string color;
        private int updateGraphic;
        private double prevAngle;
        // Constants
        private const double SPEED = 3;
        private const double TERMINAL_VELOCITY = 4;

        public Buzzard()
        {
            // Initialize instance variables
            Value = 500;
            speed = SPEED;
            angle = 0;
            updateGraphic = 0;
            prevAngle = angle;
            droppedEgg = false;
            // Starting state is standing
            state = new EnemyStandingState();
            // Determine the color of the Mik
            Random rand = new Random();
            switch (rand.Next(3)) {
                case 0:
                    color = "red";
                    break;
                case 1:
                    color = "blue";
                    break;
                case 2:
                    color = "silver";
                    break;
                default:
                    color = "red";
                    break;
            }
            imagePath = "Images/Enemy/mik_" + color + "_stand.png";
            coords = new Point(0, 0);
            World.Instance.objects.Add(this);
            World.Instance.enemies.Add(this);
        }

        /// <summary>
        /// Removes the current object from the List of WorldObjects
        /// held in the World Singleton.
        /// </summary>
        public override void Die()
        {
            World.Instance.objects.Remove(this);
            World.Instance.enemies.Remove(this);
            World.Instance.CheckWin();
        }

        /// <summary>
        /// Determines the objects next state and fires the appropriate event
        /// handler to notify the view of the state change.
        /// </summary>
        public override void Update()
        {
            // Determine the next state
            state = EnemyState.GetNextState(this);
            state.Setup();
            
            if (state is EnemyFlappingState) {
                // "Gravity" purposes
                if (speed > TERMINAL_VELOCITY) {
                    if (prevAngle == 225 || prevAngle == 270 || prevAngle == 315) speed = 0.05;
                    else speed = TERMINAL_VELOCITY;
                }
            }
            else if (state is EnemyFallingState) {
                // "Gravity" purposes
                if (speed > TERMINAL_VELOCITY) {
                    if (prevAngle == 45 || prevAngle == 90 || prevAngle == 135) speed = 0.05;
                    else speed = TERMINAL_VELOCITY;
                }
            }
            else if (state is EnemyRunningState) {
                speed = SPEED;
            }
            else if (state is BuzzardFleeingState) {
                // When the Buzzard has been hit, it drops an egg and
                // flies to the left side. Destroy the Buzzard when
                // close enough to the edge of the screen.
                if (coords.x < 3) {
                    if (BuzzardDestroyed != null)
                        BuzzardDestroyed(this, null);
                    Die();
                }
            }

            // "Gravity" purposes
            prevAngle = angle;

            // Slow the rate of updating the graphic
            if (updateGraphic > 3) updateGraphic = 0;
            else updateGraphic++;

            // Always fire the move event
            if (BuzzardMoveEvent != null) // Is anyone subscribed?
                BuzzardMoveEvent(this, null); // Raise event

            // Slow the rate of updating the graphic
            if (updateGraphic == 0) {
                if (BuzzardStateChange != null)
                    BuzzardStateChange(this, null);
            }

            // *** Check if lost in a joust against the player ***
            if (coords.y > 450 && coords.y < 525 && coords.x > 650 && coords.x < 800 && !droppedEgg) {
                if (BuzzardDropEgg != null)
                    BuzzardDropEgg(this, null);
                droppedEgg = true;
            }
        }

        public override string Serialize()
        {
            return string.Format("Buzzard,{0},{1},{2},{3}", speed, angle, coords.x, coords.y);
        }

        public override void Deserialize(string data)
        {
            string[] properties = data.Split(',');
            speed = Convert.ToDouble(properties[1]); // set speed
            angle = Convert.ToDouble(properties[2]); // set angle
            coords.x = Convert.ToDouble(properties[3]); // set x coord
            coords.y = Convert.ToDouble(properties[4]); // set y coord
        }

        public override string ToString()
        {
            return "Buzzard";
        }
    }

    [TestClass]
    public class TestBuzzard
    {
        [TestMethod]
        public void TestDie()
        {
            Buzzard b = new Buzzard();
            b.coords = new Point(500, 500);
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
