using System;
using System.Threading.Tasks;
using System.Threading;

namespace JoustModel
{
    public class SpawnState : IState
    {
        StateMachine stateMachine;
        Ostrich ostrich;

        public SpawnState(Ostrich ostrich)
        {
            this.ostrich = ostrich;
            this.stateMachine = ostrich.stateMachine;
        }

        public void Update() { }

        public void HandleInput(string command) { }
        public void Enter()
        {
            ostrich.coords.x = World.Instance.basePlatform.coords.x + 350;
            ostrich.coords.y = World.Instance.basePlatform.coords.y + 90;
            ostrich.speed = 0;
            ostrich.angle = 0;
            Task.Run(() =>
            {
                Thread.Sleep(3000);
                stateMachine.Change("stand");
            });
        }
        public void Exit() { }

        public override string ToString()
        {
            return "spawn";
        }

        public void CheckCollisions() { }
    }
}