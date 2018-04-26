//-----------------------------------------------------------
//  File:   FallState.cs
//  Desc:   Holds the FallState class
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JoustModel
{
    //-----------------------------------------------------------
    //  Desc:   Controls the movement of Ostrich when it is in the state of falling.
    //----------------------------------------------------------- 
    public class FallState : IState
    {
        // Statemachine that gets changed
        StateMachine stateMachine;
        // Ostrich object that gets changed
        Ostrich ostrich;

        // Constructor for the state
        public FallState(Ostrich ostrich)
        {
            this.ostrich = ostrich;
            this.stateMachine = ostrich.stateMachine;
        }

        // Movement changes that happens while in the fall state
        public void Update()
        {
            ostrich.nSpeed = 300;
            ostrich.nAngle = 270;
            ostrich.MoveLeftRight();
            ostrich.WrapAround();
        }

        // movement is changed depending on input
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

        // Check for collisions with other WorldObjects
        public void CheckCollisions()
        {
            ostrich.CheckEnemyCollision(ostrich.CheckCollision());
        }
    }
}

