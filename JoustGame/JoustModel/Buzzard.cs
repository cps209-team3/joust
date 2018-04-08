using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    public class Buzzard : Enemy
    {
        public event EventHandler BuzzardMoveEvent;
        public event EventHandler BuzzardStateChange;

        public override int Value { get; set; }

        public static string lock_this = "Lock this please";
        private EnemyState state;
        private int count;
        private const int SPEED = 2;

        public Buzzard(Point coords)
        {
            Value = 500;
            speed = 0;
            angle = 0;
            count = 0;
            state = EnemyState.Standing;
            imagePath = "Images/Enemy/mik_red_stand.png";
            this.coords = coords;
            World.Instance.objects.Add(this);
        }

        public override void Die()
        {
            World.Instance.objects.Remove(this);
        }

        public void UpdateState() {

        }

        public override void Update()
        {
            // Determine the next state
            DetermineNextState();
            DetermineStateInformation();

            if (BuzzardMoveEvent != null) // Is anyone subscribed?
                BuzzardMoveEvent(this, null); // Raise event

            if (count == 0) {
                if (BuzzardStateChange != null)
                    BuzzardStateChange(this, null);
            }
        }

        public void DetermineStateInformation() {
            if (count > 3) count = 0;
            else count++;

            switch (state) {
                case EnemyState.Standing:
                    // Counteract gravity
                    speed = 0;
                    imagePath = "Images/Enemy/mik_red_stand.png";
                    break;

                case EnemyState.Running_Right:
                    speed = SPEED;
                    angle = 0;
                    switch (imagePath) {
                        case "Images/Enemy/mik_red_stand.png":
                            imagePath = "Images/Enemy/mik_red_move1.png";
                            break;
                        case "Images/Enemy/mik_red_move1.png":
                            imagePath = "Images/Enemy/mik_red_move2.png";
                            break;
                        case "Images/Enemy/mik_red_move2.png":
                            imagePath = "Images/Enemy/mik_red_move3.png";
                            break;
                        case "Images/Enemy/mik_red_move3.png":
                            imagePath = "Images/Enemy/mik_red_stand.png";
                            break;
                        default:
                            imagePath = "Images/Enemy/mik_red_stand.png";
                            break;
                    }
                    break;

                case EnemyState.Running_Left:
                    speed = SPEED;
                    angle = 180;
                    switch (imagePath) {
                        case "Images/Enemy/mik_red_stand.png":
                            imagePath = "Images/Enemy/mik_red_move1.png";
                            break;
                        case "Images/Enemy/mik_red_move1.png":
                            imagePath = "Images/Enemy/mik_red_move2.png";
                            break;
                        case "Images/Enemy/mik_red_move2.png":
                            imagePath = "Images/Enemy/mik_red_move3.png";
                            break;
                        case "Images/Enemy/mik_red_move3.png":
                            imagePath = "Images/Enemy/mik_red_stand.png";
                            break;
                        default:
                            imagePath = "Images/Enemy/mik_red_stand.png";
                            break;
                    }
                    break;

                case EnemyState.InAir:
                    speed = SPEED;
                    angle = 270;
                    imagePath = "Images/Enemy/mik_red_fly1.png";
                    break;

                case EnemyState.InAir_Right:
                    speed = SPEED;
                    angle = 315;
                    imagePath = "Images/Enemy/mik_red_fly1.png";
                    break;

                case EnemyState.InAir_Left:
                    speed = SPEED;
                    angle = 225;
                    imagePath = "Images/Enemy/mik_red_fly1.png";
                    break;

                case EnemyState.Flapping:
                    speed = SPEED;
                    angle = 90;
                    switch (imagePath) {
                        case "Images/Enemy/mik_red_fly1.png":
                            imagePath = "Images/Enemy/mik_red_fly2.png";
                            break;
                        default:
                            imagePath = "Images/Enemy/mik_red_fly1.png";
                            break;
                    }
                    break;

                case EnemyState.Flapping_Right:
                    speed = SPEED;
                    angle = 45;
                    switch (imagePath) {
                        case "Images/Enemy/mik_red_fly1.png":
                            imagePath = "Images/Enemy/mik_red_fly2.png";
                            break;
                        default:
                            imagePath = "Images/Enemy/mik_red_fly1.png";
                            break;
                    }
                    break;

                case EnemyState.Flapping_Left:
                    speed = SPEED;
                    angle = 135;
                    switch (imagePath) {
                        case "Images/Enemy/mik_red_fly1.png":
                            imagePath = "Images/Enemy/mik_red_fly2.png";
                            break;
                        default:
                            imagePath = "Images/Enemy/mik_red_fly1.png";
                            break;
                    }
                    break;
            }
        }

        public void DetermineNextState() {
            Random rand = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 999990; i++) { } // Use up time so each Buzzard's random updates differently
            int chance = rand.Next(100);

            switch (state) {
                case EnemyState.Standing:
                    if (chance % 2 == 0) {
                        if (chance % 10 < 5) state = EnemyState.Running_Right; // Next state is running
                        else state = EnemyState.Running_Left; // Next state is running
                    }
                    else {
                        state = EnemyState.Flapping; // Next state is flapping
                    }
                    break;

                case EnemyState.Running_Right:
                    if (chance < 2) state = EnemyState.Standing; // Next state is standing
                    else if (chance < 99) state = EnemyState.Running_Right; // Next state is running to the right
                    else state = EnemyState.Running_Left; // Next state is running to the right
                    break;

                case EnemyState.Running_Left:
                    if (chance < 2) state = EnemyState.Standing; // Next state is standing
                    else if (chance < 99) state = EnemyState.Running_Left; // Next state is running to the right
                    else state = EnemyState.Running_Right; // Next state is running to the right
                    break;

                case EnemyState.InAir:
                    if (chance % 10 < 3) state = EnemyState.Flapping;
                    else if (chance % 2 == 0) state = EnemyState.InAir_Right;
                    else state = EnemyState.InAir_Left;
                    break;

                case EnemyState.InAir_Right:
                case EnemyState.InAir_Left:
                    if (chance < 3) state = EnemyState.InAir;
                    break;

                case EnemyState.Flapping:
                    if (chance / 10 < 2 && chance % 10 < 4 || coords.y < 10) state = EnemyState.InAir;
                    else if (chance % 2 == 0) state = EnemyState.Flapping_Right;
                    else state = EnemyState.Flapping_Left;
                    break;

                case EnemyState.Flapping_Right:
                case EnemyState.Flapping_Left:
                    if (chance < 2 || coords.y < 10) state = EnemyState.Flapping;
                    break;
            }
        }

        public override string Serialize()
        {
            return string.Format("Buzzard, {0}, {1}, {2}", this.coords, this.speed, this.angle);
        }

        public override void Deserialize(string data)
        {
            //set coords
            //set speed 
            //set angle 
        }
    }

    [TestClass]
    public class TestBuzzard
    {
        [TestMethod]
        public void TestDie()
        {
            Buzzard b = new Buzzard(new Point(500, 500));
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
