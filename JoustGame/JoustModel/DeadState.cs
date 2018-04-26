//-----------------------------------------------------------
//  File:   DeadState.cs
//  Desc:   Holds the DeadState class
//----------------------------------------------------------- 
using System;

namespace JoustModel
{
    //-----------------------------------------------------------
    //  Desc:   Controls what happens to the ostrich while in the 
    //          state of being dead.
    //----------------------------------------------------------- 
    public class DeadState : IState
    {
        // stateMachine that gets updated
        StateMachine stateMachine;
        // Ostrich object that gets updated
        Ostrich ostrich;

        // Constructor for the state
        public DeadState(Ostrich ostrich)
        {
            this.ostrich = ostrich;
            this.stateMachine = ostrich.stateMachine;
        }

        // moves the ostrich off screen then sets it to spawn
        public void Update()
        {
            ostrich.nSpeed = 600;
            ostrich.nAngle = 0;
            if (ostrich.coords.x > 1500)
            {
                stateMachine.Change("spawn");
            }
        }

        // Empty methods that must be defined but have no implementation
        public void HandleInput(string command) { }
        public void Enter() { }
        public void Exit() { }
        public void CheckCollisions() { }

        public override string ToString()
        {
            return "dead";
        }

        
    }
}