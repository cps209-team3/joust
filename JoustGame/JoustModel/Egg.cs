using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    //-----------------------------------------------------------
    //  File:   Egg.cs
    //  Desc:   This class handles the Egg enemy states.
    //----------------------------------------------------------- 
    public class Egg : Enemy
    {
        // Event handlers to notify the view
        public event EventHandler EggMoveEvent;
        public event EventHandler EggStateChange;
        public event EventHandler EggHatched;
        public event EventHandler EggDestroyed;

        // Public property Value (used to determine points awarded upon destroying)
        public override int Value { get; set; }

        // Public instance variables
        public int seconds;
        public int milliseconds;
        public bool mounted;
        public bool collected;
        // Private instance variables
        private int updateGraphic;
        private bool alreadyHatched;

        // Class Constructor        
        public Egg()
        {
            // Initialize variables
            height = 30;
            width = 30;
            type = "Egg";
            Value = 250;
            updateGraphic = 0;
            seconds = 0;
            milliseconds = 3;
            alreadyHatched = false;
            mounted = false;
            // Initialize the State Machine dictionary
            stateMachine = new StateMachine();
            EnemyStandingState stand = new EnemyStandingState(this);
            stateMachine.stateDict.Add("stand", stand);
            stateMachine.stateDict.Add("fall", new EnemyFallingState(this) { Angle = 270 });
            stateMachine.stateDict.Add("fall_right", new EnemyFallingState(this) { Angle = 315 });
            stateMachine.stateDict.Add("fall_left", new EnemyFallingState(this) { Angle = 225 });
            stateMachine.stateDict.Add("hatching", new EggHatchingState(this));
            stateMachine.stateDict.Add("hatched", new EggHatchedState(this));

            imagePath = "Images/Enemy/egg1.png";
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
            Trace.WriteLine("EGG DIED");
            World.Instance.objects.Remove(this);
            World.Instance.enemies.Remove(this);
            World.Instance.CheckWin();
            coords = null;
        }

        /// <summary>
        /// Determines the objects next state and fires the appropriate event
        /// handler to notify the view of the state change.
        /// </summary>
        public override void Update()
        {
            // Check Collision
            CheckEnemyCollision();

            // Determine the next state
            EnemyState.GetNextState(this);
            stateMachine.currentState.Update();

            if (stateMachine.currentState is EggHatchedState)
            {
                // Allow the hatched animation to run before notifying
                if (milliseconds > 30 && !alreadyHatched)
                {
                    if (EggHatched != null)
                    {
                        EggHatched(this, null);
                        alreadyHatched = true;
                    }
                }
            }

            // Check if the hatched Mik has mounted a Buzzard
            if (mounted || collected)
            {
                if (collected) {
                    Task.Run(() => {
                        PlaySounds.Instance.Play_Collect();
                    });
                }
                if (EggDestroyed != null)
                    EggDestroyed(this, null);
                Die();
                Trace.WriteLine("EGG NEW COORDS" + coords);
            }

            // Slow the rate of updating the graphic
            if (updateGraphic > 3) updateGraphic = 0;
            else updateGraphic++;

            // Always fire the move event
            if (EggMoveEvent != null)
                EggMoveEvent(this, null);

            // Slow the rate of updating the graphic
            if (updateGraphic == 0)
            {
                if (EggStateChange != null)
                    EggStateChange(this, null);
            }
        }

        /// <summary>
        /// Determines if a collision happened with this object and changes
        /// to the appropriate state.
        /// </summary>
        public void CheckEnemyCollision()
        {
            // Check Collision
            WorldObject objHit = CheckCollision();
            if (objHit != null) // special case for fleeing, fix later. 
            {
                if (objHit.ToString() == "Ostrich")
                {
                    collected = true;
                    (objHit as Ostrich).score += Value;
                }

            }
        }

        /// <summary>
        /// Returns the properties of this Egg object in string form
        /// </summary>
        public override string Serialize()
        {
            return string.Format("Egg,{0},{1},{2},{3}",speed, angle, coords.x, coords.y);
        }

        /// <summary>
        /// Extracts the properties of the Egg object from a string.
        /// </summary>
        /// <param name="data">The string of properties to extract</param>
        public override void Deserialize(string data)
        {
            string[] properties = data.Split(',');
            speed = Convert.ToDouble(properties[1]); // set speed
            angle = Convert.ToDouble(properties[2]); // set angle
            coords.x = Convert.ToDouble(properties[3]); // set x coord
            coords.y = Convert.ToDouble(properties[4]); // set y coord
        }

        /// <summary>
        /// Returns the object's class name.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Egg";
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
