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

        public Ostrich()
        {
            Value = 2000;
            lives = 3;
            score = 0;
            speed = 0;
            angle = 0;
            acceleration = 0;
            accelerationAngle = 0;
            stateMachine = new StateMachine();
            StandState stand = new StandState(this);
            stateMachine.stateDict.Add("flap", new FlapState(this));
            stateMachine.stateDict.Add("stand", stand);
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
            double xAcceleration = acceleration * (Math.Cos(accelerationAngle * Math.PI / 180)) / 30;
            double yAcceleration = acceleration * (Math.Sin(accelerationAngle * Math.PI / 180)) / 30;
            double xNew = xSpeed + xAcceleration;
            double yNew = ySpeed + yAcceleration;
            speed = Math.Sqrt(Math.Pow(xNew, 2) + Math.Pow(yNew, 2));
            angle = Math.Atan(yNew / xNew);
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
