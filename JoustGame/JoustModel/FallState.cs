using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JoustModel
{
    public class FallState : State
    {
        public FallState() : base() { }
        public FallState(string name) : base(name) { }
        [JsonConstructor]
        public FallState(string name, int o) : base(name, o) { }

        public override void Update()
        {
            ostrich.nSpeed = 200;
            ostrich.nAngle = 270;
            ostrich.MoveLeftRight();
            ostrich.WrapAround();
        }

        public override void HandleInput(string command)
        {
            switch (command)
            {
                case "flap":
                    stateMachine.Change("flap");
                    break;
                case "left":
                    ostrich.nSpeed = 632;
                    ostrich.nAngle = 198;
                    break;
                case "right":
                    ostrich.nSpeed = 632;
                    ostrich.nAngle = 341;
                    break;
                default:
                    break;
            }
        }

        public override void Enter() { }
        public override void Exit() { }

        public override string ToString()
        {
            return "fall";
        }

        public override void CheckCollisions()
        {
            ostrich.CheckEnemyCollision(ostrich.CheckCollision());
        }
    }
}

