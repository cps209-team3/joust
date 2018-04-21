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
            ostrich.coords.y = World.Instance.basePlatform.coords.y;
            ostrich.speed = 0.075;
            ostrich.angle = 90;
            Task.Run(() =>
            {
                Thread.Sleep(5000);
                ostrich.speed = 0;
            });
        }
        public void Exit() { }

        public override string ToString()
        {
            return "spawn";
        }
    }
}