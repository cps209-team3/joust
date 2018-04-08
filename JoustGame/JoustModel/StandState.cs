using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JoustModel
{
    public class StandState : IState
    {
        StateMachine stateMachine;
        Ostrich ostrich;

        public StandState(Ostrich ostrich)
        {
            this.ostrich = ostrich;
            this.stateMachine = ostrich.stateMachine;
        }

        public void Update()
        {
            // Check for collisions
            // if collision:
            // stateMachine.Change("collide");
        }

        public void HandleInput(string command)
        {
            switch (command)
            {
                case "flap":
                    ostrich.accelerationAngle = 90;
                    stateMachine.Change("flap");
                    break;
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

        public void Enter() { /* Play animation */ }
        public void Exit() { }
    }
}