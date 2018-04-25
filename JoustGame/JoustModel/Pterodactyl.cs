using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        // Public instance variables
        public bool charging;
        // Private instance variables
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
            height = 30;
            width = 120;
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
            stateMachine = new StateMachine();
            EnemyFlappingState flap = new EnemyFlappingState(this) { Angle = 90 };
            stateMachine.stateDict.Add("flap", flap);
            stateMachine.stateDict.Add("flap_right", new EnemyFlappingState(this) { Angle = 45 });
            stateMachine.stateDict.Add("flap_left", new EnemyFlappingState(this) { Angle = 135 });
            stateMachine.stateDict.Add("fall", new EnemyFallingState(this) { Angle = 270 });
            stateMachine.stateDict.Add("fall_right", new EnemyFallingState(this) { Angle = 315 });
            stateMachine.stateDict.Add("fall_left", new EnemyFallingState(this) { Angle = 225 });
            stateMachine.stateDict.Add("charge", new PterodactylChargeState(this));
            stateMachine.stateDict.Add("destroyed", new PterodactylDestroyedState(this));

            stateMachine.currentState = flap;

            imagePath = "Images/Enemy/pterodactyl_fly1.png";
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
            // Check Collision
            CheckEnemyCollision();

            if (!charging)
            {
                // Determine the next state
                EnemyState.GetNextState(this);
                stateMachine.currentState.Update();
            }

            if (stateMachine.currentState is EnemyFlappingState)
            {
                // "Gravity" purposes
                if (speed > TERMINAL_VELOCITY)
                {
                    if (prevAngle == 225 || prevAngle == 270 || prevAngle == 315) speed = 0.05;
                    else speed = TERMINAL_VELOCITY;
                }
            }
            else if (stateMachine.currentState is EnemyFallingState)
            {
                // "Gravity" purposes
                if (speed > TERMINAL_VELOCITY)
                {
                    if (prevAngle == 45 || prevAngle == 90 || prevAngle == 135) speed = 0.05;
                    else speed = TERMINAL_VELOCITY;
                }
            }
            else if (stateMachine.currentState is PterodactylDestroyedState)
            {
                // Slow the last 2 frames of the destroyed pterodactyl
                dieAnimateTime++;
                updateGraphicRate = 10;
                if (dieAnimateTime > 18)
                {
                    if (PterodactylDestroyed != null)
                        PterodactylDestroyed(this, null);
                    Die();
                }
            }
            else if (stateMachine.currentState is PterodactylChargeState)
            {
                // Don't allow the state to change when charging
                Trace.WriteLine("Charging: " + chargeTime);
                charging = true;
                chargeTime++;
                speed = 10;
                if (chargeTime > 50)
                {
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
            if (updateGraphic == 0)
            {
                if (PterodactylStateChange != null)
                    PterodactylStateChange(this, null);
            }
        }

        public void CheckEnemyCollision()
        {
            // Check Collision
            WorldObject objHit = CheckCollision();
            if (objHit != null) // special case for fleeing, fix later. 
            {
                if (objHit.ToString() == "Ostrich")
                {
                    if (this.coords.y > objHit.coords.y)
                    {
                        this.stateMachine.Change("destroyed");
                        stateMachine.currentState.Update();
                        (objHit as Ostrich).score += Value;
                    }
                    else
                    {
                        (objHit as Ostrich).Die();
                    }
                }
                else
                {
                    Point minTV = FindMinTV(objHit);
                    if (minTV.y > 0)
                    {
                        this.stateMachine.Change("flap"); //if hit top
                    }
                    else if (minTV.y < 0)
                    {
                        this.stateMachine.Change("fall"); // if hit bottom
                    }
                    else if (minTV.x > 0)
                    {
                        this.stateMachine.Change("flap_left"); // if hit left
                    }
                    else if (minTV.x < 0)
                    {
                        this.stateMachine.Change("flap_right"); // if hit right
                    }
                }
            }
        }

        // returns the properties of this Pterodactyl object in string form
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

        public override string ToString()
        {
            return "Pterodactyl";
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
