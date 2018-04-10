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

        public Ostrich()
        {
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
            World.Instance.objects.Add(this);
        }

        public override void Die()
        {
            World.Instance.objects.Remove(this);
        }

        public override void Update()
        {
            // Check for collisions

            double xSpeed = speed * (Math.Cos(angle * Math.PI / 180));
            double ySpeed = speed * (Math.Sin(angle * Math.PI / 180));
            //Console.WriteLine("speed: " + Convert.ToString(speed));
            //Console.WriteLine("angle: " + Convert.ToString(angle));
            //Console.WriteLine("cos: " + Convert.ToString(Math.Cos(angle * Math.PI / 180)));
            //Console.WriteLine("sin: " + Convert.ToString(Math.Sin(angle * Math.PI / 180)));
            //Console.WriteLine(" ");
            double nXSpeed;
            double nYSpeed;
            nXSpeed = nSpeed * (Math.Cos(nAngle * Math.PI / 180));
            nYSpeed = nSpeed * (Math.Sin(nAngle * Math.PI / 180));
            //Console.WriteLine(speed);
            //Console.WriteLine(angle);
            //Console.WriteLine(acceleration);
            //Console.WriteLine(accelerationAngle);
            //Console.WriteLine();
            double xNew = (xSpeed + nXSpeed) / 50;
            double yNew = (ySpeed + nYSpeed) / 50;
            //Console.WriteLine(xSpeed);
            //Console.WriteLine(xAcceleration);
            //Console.WriteLine(ySpeed);
            //Console.WriteLine(yAcceleration);
            //Console.WriteLine(" ");
            //Console.WriteLine(xNew);
            //Console.WriteLine(yNew);
            //Console.WriteLine(" ");
            speed = Math.Sqrt(Math.Pow(xNew, 2) + Math.Pow(yNew, 2));
            angle = Math.Atan2(yNew, xNew) * 180 / Math.PI;
            if (speed > 500) { speed = 500; }
            lock (oLock)
            {
                nSpeed = 0;
                nAngle = 0;
            }
            //Console.WriteLine(yNew);
            //Console.WriteLine(xNew);
            //Console.WriteLine(yNew / xNew);
            //Console.WriteLine();
            coords.x += xNew;
            coords.y -= yNew;

            if (ostrichMoved != null) { ostrichMoved(this, 0); }

            stateMachine.Update();
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
