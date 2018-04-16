using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    public class Ostrich : Entity
    {
        public event EventHandler<int> ostrichMoved;
        public StateMachine stateMachine;
        public int lives;
        public int score;
        public override int Value { get; set; }
        public string input;
        public string oLock;
        public bool leftDown;
        public bool rightDown;
        public bool cheatMode;

        public Ostrich()
        {
            hitbox.height = 75;
            hitbox.width = 50;
            cheatMode = false;
            type = "Ostrich";
            oLock = "lock";
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
            stateMachine.currentState = stand;
            imagePath = "Images/Player/player_stand.png";
            leftDown = false;
            rightDown = false;
            World.Instance.objects.Add(this);
        }

        public override void Die()
        {
            if (!cheatMode)
            {
                World.Instance.objects.Remove(this);
            }
        }


        public override void Update()
        {
            CheckCollision();

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
            if (speed > 1000) { speed = 1000; }
            nSpeed = 0;
            nAngle = 0;
            coords.x += xNew;
            coords.y -= yNew;
            hitbox.xPos = coords.x;
            hitbox.yPos = coords.y;

            if (ostrichMoved != null) { ostrichMoved(this, 0); }

            stateMachine.Update();
        }

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

        // Serialization
        public override string Serialize()
        {
            return string.Format("Ostrich,{0},{1},{2},{3},{4},{5}", score, lives, speed, angle, coords.x, coords.y);
        }

        public override void Deserialize(string data)
        {
            string[] properties = data.Split(',');
            score = Convert.ToInt32(properties[1]); // set score
            lives = Convert.ToInt32(properties[2]); // set lives
            speed = Convert.ToDouble(properties[3]); // set speed
            angle = Convert.ToDouble(properties[4]); // set angle
            coords.x = Convert.ToDouble(properties[5]); // set x coord
            coords.y = Convert.ToDouble(properties[6]); // set y coord
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
