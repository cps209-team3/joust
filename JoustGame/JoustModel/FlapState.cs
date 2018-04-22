using System;
using System.Threading;
using System.Threading.Tasks;

namespace JoustModel
{
    public class FlapState : IState
    {
        StateMachine stateMachine;
        Ostrich ostrich;

        public FlapState(Ostrich ostrich)
        {
            this.ostrich = ostrich;
            this.stateMachine = ostrich.stateMachine;
        }

        public void Update()
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

        public void HandleInput(string command)
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

        public void Enter() { ostrich.changing = true; }

        public void Exit() { ostrich.changing = false; }

        public override string ToString()
        {
            return "flap";
        }

        public void CheckCollisions()
        {
            ostrich.CheckEnemyCollision(ostrich.CheckCollision());
        }
    }
}
