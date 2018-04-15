using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
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
        // Private instance variables
        private int updateGraphic;
        private bool alreadyHatched;

        // Class Constructor        
        public Egg()
        {
            // Initialize variables
            hitbox.height = 30;
            hitbox.width = 30;
            type = "Egg";
            Value = 250;
            updateGraphic = 0;
            seconds = 0;
            milliseconds = 3;
            alreadyHatched = false;
            mounted = false;
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
            CheckCollision();

            // Determine the next state
            state = EnemyState.GetNextState(this);
            state.Setup();

            if (state is EggHatchedState) {
                // Allow the hatched animation to run before notifying
                if (milliseconds > 30 && !alreadyHatched) {
                    if (EggHatched != null) {
                        EggHatched(this, null);
                        alreadyHatched = true;
                    }
                }
            }

            // Check if the hatched Mik has mounted a Buzzard
            if (mounted) {
                if (EggDestroyed != null)
                    EggDestroyed(this, null);
                Die();
            }

            // Slow the rate of updating the graphic
            if (updateGraphic > 3) updateGraphic = 0;
            else updateGraphic++;

            // Always fire the move event
            if (EggMoveEvent != null)
                EggMoveEvent(this, null);

            // Slow the rate of updating the graphic
            if (updateGraphic == 0) {
                if (EggStateChange != null)
                    EggStateChange(this, null);
            }
        }

        //Serialization
        public override string Serialize()
        {
            return string.Format("Egg,{0},{1},{2},{3}",speed, angle, coords.x, coords.y);
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
