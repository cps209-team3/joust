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
			lock (ostrich.oLock)
            {
                ostrich.nSpeed = 50;
                ostrich.nAngle = 270;
            }
        }

        public void HandleInput(string command)
        {
            switch (command)
            {
                case "flap":
                    stateMachine.Change("flap");
                    break;
                case "left":
                    lock (ostrich.oLock)
                    {
                        ostrich.nSpeed = 500;
                        ostrich.nAngle = 180;
                    }
                    break;
                case "right":
                    lock (ostrich.oLock)
                    {
                        ostrich.nSpeed = 500;
                        ostrich.nAngle = 0;
                    }
                    break;
                default:
                    break;
            }
        }

        public void Enter() { }
        public void Exit() { }
    }
}

