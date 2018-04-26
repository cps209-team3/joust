//-----------------------------------------------------------
//  File:   FlapState.cs
//  Desc:   Holds the FlapState class
//----------------------------------------------------------- 

using System;
using System.Threading;
using System.Threading.Tasks;

namespace JoustModel
{
    //-----------------------------------------------------------
    //  Desc:   Determines what happens to Ostrich while in the flapping state
    //----------------------------------------------------------- 
    public class FlapState : IState
    {
        // State machine to be updated
        StateMachine stateMachine;
        // Ostrich object to be updated
        Ostrich ostrich;

        // Constructor for the flapstate
        public FlapState(Ostrich ostrich)
        {
            this.ostrich = ostrich;
            this.stateMachine = ostrich.stateMachine;
        }

        // Moves the Ostrich up and changes to the falling state after 100 miliseconds
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

        // Determines whether the ostrich goes left or right based on the input
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

        // Check for collisions with other WorldObjects
        public void CheckCollisions()
        {
            ostrich.CheckEnemyCollision(ostrich.CheckCollision());
        }
    }
}
