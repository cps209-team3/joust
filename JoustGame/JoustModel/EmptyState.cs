//-----------------------------------------------------------
//  File:   EmptyState.cs
//  Desc:   Holds the EmptyState class
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace JoustModel
{
    //-----------------------------------------------------------
    //  Desc:   State in which nothing happens
    //----------------------------------------------------------- 
    public class EmptyState : IState
    {
        // Methods that must be defined but have no implementation
        public void Update() { }
        public void HandleInput(string data) { }
        public void Enter() { }
        public void Exit() { }
        public void CheckCollisions() { }
    }
}
