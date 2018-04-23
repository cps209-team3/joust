using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JoustModel
{
    public class FlapState : State
    {
        public FlapState() : base() { }
        public FlapState(string name) : base(name) { }
        [JsonConstructor]
        public FlapState(string name, int o) : base(name, o) { }

        public override void Update()
        {
            ostrich.nSpeed = 1000;
            ostrich.nAngle = 90;
            Task.Run(() =>
            {
                Thread.Sleep(100);
                if (stateMachine.currentState.ToString() == "flap")
                {
                    stateMachine.Change("fall");
                }
            });
            ostrich.MoveLeftRight();
            ostrich.WrapAround();
        }

        public override void HandleInput(string command)
        {
            switch (command)
            {
                case "left":
                    ostrich.nSpeed = 1166;
                    ostrich.nAngle = 120;
                    break;
                case "right":
                    ostrich.nSpeed = 1166;
                    ostrich.nAngle = 60;
                    break;
                default:
                    break;
            }
        }

        public override void Enter() { ostrich.changing = true; }

        public override void Exit() { ostrich.changing = false; }

        public override string ToString()
        {
            return "flap";
        }

        public override void CheckCollisions()
        {
            ostrich.CheckEnemyCollision(ostrich.CheckCollision());
        }
    }
}
