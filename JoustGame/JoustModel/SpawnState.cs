using System;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;

namespace JoustModel
{
    public class SpawnState : State
    {
        public SpawnState() : base() { }
        public SpawnState(string name) : base(name) { }
        [JsonConstructor]
        public SpawnState(string name, int o) : base(name, o) { }

        public override void Update() { }

        public override void HandleInput(string command) { }
        public override void Enter()
        {
            ostrich.coords.x = World.Instance.basePlatform.coords.x + 350;
            ostrich.coords.y = World.Instance.basePlatform.coords.y - 76;
            ostrich.speed = 0;
            ostrich.angle = 0;
            Task.Run(() =>
            {
                Thread.Sleep(3000);
                stateMachine.Change("stand");
            });
        }
        public override void Exit() { }

        public override string ToString()
        {
            return "spawn";
        }

        public override void CheckCollisions() { }
    }
}