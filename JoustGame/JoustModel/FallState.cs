using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JoustModel
{
    public class FallState : IState
    {
        StateMachine stateMachine;
        Ostrich ostrich;

        public FallState(Ostrich ostrich)
        {
            this.ostrich = ostrich;
            this.stateMachine = ostrich.stateMachine;
        }

        public void Update()
        {
            ostrich.nSpeed = 300;
            ostrich.nAngle = 270;
            ostrich.MoveLeftRight();
            ostrich.WrapAround();
        }

        public void HandleInput(string command)
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

        public void Enter() { }
        public void Exit() { }

        public override string ToString()
        {
            return "fall";
        }

        public void CheckCollisions()
        {
            ostrich.CheckEnemyCollision(ostrich.CheckCollision());
        }
    }
}

