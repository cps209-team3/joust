using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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

        public void Update() { }

        public void HandleInput(string command)
        {
            switch (command)
            {
                case "left":
                    stateMachine.Change("left");
                    break;
                case "right":
                    stateMachine.Change("right");
                    break;
                default:
                    break;
            }
        }

        public void Enter()
        {
            // Play flap animation
            ostrich.acceleration += 1375;
            // Wait 100 ms
            ostrich.acceleration -= 1375;
            Exit();
        }
        public void Exit() { }
    }
}
