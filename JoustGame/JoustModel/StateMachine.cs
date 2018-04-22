using System;
using System.Collections.Generic;

namespace JoustModel
{
    public class StateMachine
    {
        public Dictionary<string, State> stateDict = new Dictionary<string, State>();
        public State currentState = new EmptyState();
        
        public void Add(string id, State state) { stateDict.Add(id, state); }
        public void Remove(string id) { stateDict.Remove(id); }
        public void Clear() { stateDict.Clear(); }

        public void Change(string id)
        {
            currentState.Exit();
            State nextState = stateDict[id];
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