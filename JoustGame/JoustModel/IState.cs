//-----------------------------------------------------------
//  File:   IState.cs
//  Desc:   Holds the IState interface that must be implemented 
//          by all entities with a state machine.
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace JoustModel
{
    public interface IState
    {
        void Update();
        void HandleInput(string data);
        void Enter();
        void Exit();
        void CheckCollisions();
    }
}
