using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public event EventHandler<int> buzzDied;

        // Public property Value (used to determine points awarded upon destroying)
        public override int Value { get; set; }
        // Public property Color (used to determine the Mik color)
        public string Color { get { return color; } }

        // Public instance variables
        public bool droppedEgg;
        public Egg pickupEgg;
        public int respawning;
        // Private instance variables
        private string color;
        private bool isSpawning;
        private int updateGraphic;
        private double prevAngle;
        // Constants
        private const double SPEED = 3;
        private const double TERMINAL_VELOCITY = 4;

        public Buzzard()
        {
            // Initialize instance variables
            height = 75;
            width = 50;
            type = "Buzzard";
            Value = 500;
            speed = SPEED;
            angle = 0;
            updateGraphic = 0;
            prevAngle = angle;
            droppedEgg = false;
            // Starting state is standing
            stateMachine = new StateMachine();
            EnemySpawningState spawn = new EnemySpawningState(this);
            stateMachine.stateDict.Add("stand", new EnemyStandingState(this));
            stateMachine.stateDict.Add("run_right", new EnemyRunningState(this) { Angle = 0 });
            stateMachine.stateDict.Add("run_left", new EnemyRunningState(this) { Angle = 180 });
            stateMachine.stateDict.Add("flap", new EnemyFlappingState(this) { Angle = 90 });
            stateMachine.stateDict.Add("flap_right", new EnemyFlappingState(this) { Angle = 45 });
            stateMachine.stateDict.Add("flap_left", new EnemyFlappingState(this) { Angle = 135 });
            stateMachine.stateDict.Add("fall", new EnemyFallingState(this) { Angle = 270 });
            stateMachine.stateDict.Add("fall_right", new EnemyFallingState(this) { Angle = 315 });
            stateMachine.stateDict.Add("fall_left", new EnemyFallingState(this) { Angle = 225 });
            stateMachine.stateDict.Add("flee", new BuzzardFleeingState(this));
            stateMachine.stateDict.Add("pickup", new BuzzardPickupState(this));
            stateMachine.stateDict.Add("spawn", spawn);

            stateMachine.currentState = spawn;
            isSpawning = true;

            // Determine the color of the Mik
            Random rand = new Random();
            switch (rand.Next(3))
            {
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
            if (!isSpawning) {
                if (!droppedEgg) CheckEnemyCollision();

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
            else if (stateMachine.currentState is EnemyRunningState)
            {
                speed = SPEED;
            }
            else if (stateMachine.currentState is BuzzardFleeingState)
            {
                // When the Buzzard has been hit, it drops an egg and
                // flies to the left side. Destroy the Buzzard when
                // close enough to the edge of the screen.
                if (!droppedEgg)
                {
                    if (BuzzardDropEgg != null)
                        BuzzardDropEgg(this, null);
                    droppedEgg = true;
                }

                if (coords.x < 3)
                {
                    if (BuzzardDestroyed != null)
                        BuzzardDestroyed(this, null);
                    Die();
                }
            }
            else if (stateMachine.currentState is EnemySpawningState) {
                respawning++;
                Trace.WriteLine("Spawning...");
                isSpawning = true;
                prevAngle = 90;
                angle = 90;
                speed = 0.5;
                imagePath = "Images/Enemy/mik_respawn.png";
                if (respawning > 100) {
                    if (droppedEgg) {
                        stateMachine.Change("pickup");
                        BuzzardPickupState pState = stateMachine.currentState as BuzzardPickupState;
                        pState.TargetEgg = pickupEgg;
                        stateMachine.currentState.Update();
                    }
                    else {
                        stateMachine.Change("spawn");
                    }
                    respawning = 0;
                    isSpawning = false;
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
            if (updateGraphic == 0)
            {
                if (BuzzardStateChange != null)
                    BuzzardStateChange(this, null);
            }
        }

        public void CheckEnemyCollision()
        {
            // Check Collision
            WorldObject objHit = CheckCollision();
            if (objHit != null && stateMachine.currentState.ToString() != "JoustModel.BuzzardFleeingState") // special case for fleeing, fix later. 
            {
                if (objHit.ToString() == "Ostrich")
                {
                    string state = (objHit as Ostrich).stateMachine.currentState.ToString();
                    //Console.WriteLine("ostrich state = " + state);
                    if (state != "dead" && state != "spawn")
                    {
                        if (this.coords.y > objHit.coords.y)
                        {
                            if (buzzDied != null)
                            {
                                buzzDied(this, 0);
                            }
                            this.stateMachine.Change("flee");
                            stateMachine.currentState.Update();
                            (objHit as Ostrich).score += Value;
                        }
                        else
                        {
                            (objHit as Ostrich).Die();
                        }
                    }
                }
                else
                {
                    Point minTV = FindMinTV(objHit);
                    if (minTV.y > 0)
                    {
                        this.stateMachine.Change("stand"); //if hit top
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
