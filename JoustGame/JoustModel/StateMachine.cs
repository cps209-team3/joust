using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JoustModel
{
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