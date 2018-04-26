//-----------------------------------------------------------
//  File:   StateMachine.cs
//  Desc:   Holds the StateMachine class
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;

namespace JoustModel
{
    //-----------------------------------------------------------
    //  Desc:   Controls the state of every entity that implements it
    //----------------------------------------------------------- 
    public class StateMachine
    {
        public Dictionary<string, IState> stateDict = new Dictionary<string, IState>();
        public IState currentState = new EmptyState();

        public IState Current { get { return currentState; } }
        public void Add(string id, IState state) { stateDict.Add(id, state); }
        public void Remove(string id) { stateDict.Remove(id); }
        public void Clear() { stateDict.Clear(); }

        public void Change(string id)
        {
            currentState.Exit();
            IState nextState = stateDict[id];
            nextState.Enter();            
            currentState = nextState;
        }

        public void Update()
        {
            currentState.Update();
        }

        public void HandleInput(string data)
        {
            currentState.HandleInput(data);
        }
    }
}