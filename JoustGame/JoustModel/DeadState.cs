using System;
using Newtonsoft.Json;

namespace JoustModel
{
    public class DeadState : State
    {
        public DeadState() : base() { }
        public DeadState(string name) : base(name) { }
        [JsonConstructor]
        public DeadState(string name, int o) : base(name, o) { }

        public override void Update()
        {
            ostrich.nSpeed = 600;
            ostrich.nAngle = 0;
            if (ostrich.coords.x > 1500)
            {
                stateMachine.Change("spawn");
            }
        }

        public override void HandleInput(string command) { }
        public override void Enter() { }
        public override void Exit() { }

        public override string ToString()
        {
            return "dead";
        }

        public override void CheckCollisions() { }
    }
}