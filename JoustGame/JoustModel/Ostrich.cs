//-----------------------------------------------------------
//  File:   Ostrich.cs
//  Desc:   Holds the Ostrich class and Ostrich unit tests
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    //----------------------------------------------------------- 
    //  Desc:   This class handles the Ostrich (player) entity. 
    //          Handles movement, controls, Collision, and 
    //          its saving/loading.
    //----------------------------------------------------------- 
    public class Ostrich : Entity
    {   
        // Event handlers to notify view
        public event EventHandler<int> ostrichMoved;
        public event EventHandler<int> ostrichDied;
        // Statemachine to control the states of ostrich
        public StateMachine stateMachine;
        // Lives the player has left
        public int lives;
        // Total score of the player
        public int score;
        // Current stage the player is on
        public int stage;
        // How much the Ostrich is worth in points
        public override int Value { get; set; }
        // The input the user provides
        public string input;
        // whether player is moving left and downward
        public bool leftDown;
        // whether player is moving right and downward
        public bool rightDown;
        // Is in cheat mode or not
        public bool cheatMode;
        // Whether state is changing
        public bool changing;

        /// <summary>
        /// Constructor for the Ostrich
        /// </summary>
        public Ostrich()
        {
            changing = false;
            height = 67;
            width = 50;
            cheatMode = false;
            type = "Ostrich";
            Value = 2000;
            lives = 3;
            score = 0;
            speed = 0;
            angle = 0;
            nSpeed = 0;
            nAngle = 0;
            stateMachine = new StateMachine();
            StandState stand = new StandState(this);
            stateMachine.stateDict.Add("flap", new FlapState(this));
            stateMachine.stateDict.Add("stand", stand);
            stateMachine.stateDict.Add("fall", new FallState(this));
            stateMachine.stateDict.Add("dead", new DeadState(this));
            stateMachine.stateDict.Add("spawn", new SpawnState(this));
            stateMachine.currentState = stand;
            imagePath = "Images/Player/player_stand.png";
            leftDown = false;
            rightDown = false;
            World.Instance.objects.Add(this);
        }

        /// <summary>
        /// Changes the Otrich's state to the 'DeadState'
        /// </summary>
        public override void Die()
        {
            if (!cheatMode)
            {
                lives -= 1;
                if (lives == 0)
                {
                    if (ostrichDied != null)
                        ostrichDied(this, 0);
                }
                else
                {
                    stateMachine.Change("dead");
                }
            }
        }

        /// <summary>
        /// Updates the ostrich's speed and direction and checks for collision
        /// </summary>
        public override void Update()
        {
            double xSpeed = speed * (Math.Cos(angle * Math.PI / 180));
            double ySpeed = speed * (Math.Sin(angle * Math.PI / 180));
            double nXSpeed;
            double nYSpeed;
            nXSpeed = nSpeed * (Math.Cos(nAngle * Math.PI / 180));
            nYSpeed = nSpeed * (Math.Sin(nAngle * Math.PI / 180));
            double xNew = (xSpeed + nXSpeed) / 100;
            double yNew = (ySpeed + nYSpeed) / 100;
            speed = Math.Sqrt(Math.Pow(xNew, 2) + Math.Pow(yNew, 2));
            angle = Math.Atan2(yNew, xNew) * 180 / Math.PI;
            if (speed > 1000 && stateMachine.currentState.ToString() == "fall") { speed = 1000; }
            else if (speed > 1200 && stateMachine.currentState.ToString() == "flap") { speed = 1200; }
            nSpeed = 0;
            nAngle = 0;
            coords.x += xNew;
            coords.y -= yNew;

            if (coords.y < 0) coords.y = 0;
            else if (coords.y > 900) coords.y = 900;

            if (ostrichMoved != null) { ostrichMoved(this, 0); }
            stateMachine.Update();
            stateMachine.currentState.CheckCollisions();
        }

        /// <summary>
        /// Moves the Ostrich to the other side of the screen when it reaches the boarder
        /// </summary>
        public void WrapAround()
        {
            if (coords.x < -25) coords.x = 1465;
            else if (coords.x > 1465) coords.x = -25;
        }

        /// <summary>
        /// determines whether the ostrich should go left or right
        /// </summary>
        public void MoveLeftRight()
        {
            if (leftDown && !rightDown)
            {
                Task.Run(() => stateMachine.HandleInput("left"));
            }
            else if (rightDown && !leftDown)
            {
                Task.Run(() => stateMachine.HandleInput("right"));
            }
        }

        /// <summary>
        /// Checks for collision with a platform and sets the position of the ostrich to not collide with the platform
        /// </summary>
        /// <param name="objHit">Any WorldObject</param>
        public void CheckEnemyCollision(WorldObject objHit)
        {
            if (objHit != null)
            { 
                if (objHit.ToString() == "Platform" || objHit.ToString() == "Base")
                {
                    Point minTV = FindMinTV(objHit);
                    coords.x -= minTV.x;
                    coords.y -= minTV.y;
                    if (minTV.y > 0)
                    {
                        Console.WriteLine(objHit.ToString());
                        if (!changing)
                        {
                            stateMachine.Change("stand");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// returns the properties of this Ostrich object in string form
        /// </summary>
        public override string Serialize()
        {
            return string.Format("Ostrich,{0},{1},{2},{3},{4},{5},{6},{7}", score, lives, speed, angle, coords.x, coords.y, stateMachine.currentState.ToString(), stage);
        }

        /// <summary>
        /// Sets the properties of Ostrich based on the data passed in.
        /// </summary>
        /// <param name="data">String of Ostrich properties to be set</param>
        public override void Deserialize(string data)
        {
            string[] properties = data.Split(',');
            score = Convert.ToInt32(properties[1]); // set score
            lives = Convert.ToInt32(properties[2]); // set lives
            speed = Convert.ToDouble(properties[3]); // set speed
            angle = Convert.ToDouble(properties[4]); // set angle
            coords.x = Convert.ToDouble(properties[5]); // set x coord
            coords.y = Convert.ToDouble(properties[6]); // set y coord
            stateMachine.Change(properties[7]);
            stage = Convert.ToInt32(properties[8]); // set stage
        }

        public override string ToString()
        {
            return "Ostrich";
        }
    }

    [TestClass]
    public class TestOstrich
    {
        [TestMethod]
        public void TestDie()
        {
            Ostrich o = new Ostrich();
            o.coords = new Point(500, 500);
            o.Die();
            Assert.AreEqual(new List<WorldObject> { }, World.Instance.objects);
        }

        [TestMethod]
        public void TestUpdate()
        {
            // implement when update is implemented
        }
    }
}
