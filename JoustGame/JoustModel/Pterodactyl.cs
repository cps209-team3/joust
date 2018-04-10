using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    public class Pterodactyl : Enemy
    {
        // Event handlers to notify the view
        public event EventHandler PterodactylMoveEvent;
        public event EventHandler PterodactylStateChange;
        public event EventHandler PterodactylDestroyed;

        // Public property Value (used to determine points awarded upon destroying)
        public override int Value { get; set; }
        
        // Private instance variables
        private bool charging;
        private const double SPEED = 6;
        private const double TERMINAL_VELOCITY = 7;
        private double prevAngle;
        private int updateGraphic;
        private int updateGraphicRate;
        private int chargeTime;
        private int dieAnimateTime;

        // Class Constructor
        public Pterodactyl()
        {
            // Initialize instance variables
            Value = 1000;
            speed = 0;
            angle = 315;
            prevAngle = angle;
            updateGraphic = 0;
            updateGraphicRate = 3;
            charging = false;
            chargeTime = 0;
            dieAnimateTime = 0;
            // Start out in falling state
            state = new EnemyFallingState() { StateEnemy = this, Angle = (int)angle };
            imagePath = "Images/Enemy/pterodactyl_fly1.png";
            coords = new Point(0, 0);
            World.Instance.objects.Add(this);
        }

        /// <summary>
        /// Removes the current object from the List of WorldObjects
        /// held in the World Singleton.
        /// </summary>
        public override void Die()
        {
            World.Instance.objects.Remove(this);
        }

        /// <summary>
        /// Determines the objects next state and fires the appropriate event
        /// handler to notify the view of the state change.
        /// </summary>
        public override void Update()
        {
            if (!charging) {
                // Determine the next state
                state = EnemyState.GetNextState(this);
                state.Setup();
            }

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
            else if (state is PterodactylDestroyedState) {
                // Slow the last 2 frames of the destroyed pterodactyl
                dieAnimateTime++;
                updateGraphicRate = 10;
                if (dieAnimateTime > 18) {
                    if (PterodactylDestroyed != null)
                        PterodactylDestroyed(this, null);
                    Die();
                }
            }
            else if (state is PterodactylChargeState) {
                // Don't allow the state to change when charging
                charging = true;
                chargeTime++;
                speed = 10;
                if (chargeTime > 40) {
                    charging = false;
                    chargeTime = 0;
                }
            }

            // "Gravity" purposes
            prevAngle = angle;

            // Slow the rate of updating the graphic
            if (updateGraphic > updateGraphicRate) updateGraphic = 0;
            else updateGraphic++;

            // Always fire the move event
            if (PterodactylMoveEvent != null)
                PterodactylMoveEvent(this, null);

            // Slow the rate of updating the graphic
            if (updateGraphic == 0) {
                if (PterodactylStateChange != null)
                    PterodactylStateChange(this, null);
            }
        }

        public override string Serialize()
        {
            return string.Format("Pterodactyl,{0},{1},{2},{3}", speed, angle, coords.x, coords.y);
        }

        public override void Deserialize(string data)
        {
            string[] properties = data.Split(',');
            speed = Convert.ToDouble(properties[1]); // set speed
            angle = Convert.ToDouble(properties[2]); // set angle
            coords.x = Convert.ToDouble(properties[3]); // set x coord
            coords.y = Convert.ToDouble(properties[4]); // set y coord
        }
    }

    [TestClass]
    public class TestPterodactyl
    {
        [TestMethod]
        public void TestDie()
        {
            Pterodactyl p = new Pterodactyl();
            p.coords = new Point(500, 500);
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
