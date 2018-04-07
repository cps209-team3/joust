using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JoustModel
{
    public class EmptyState : IState
    {
        public void Update() { }
        public void HandleInput(string data) { }
        public void Enter(params object[] args) { }
        public void Exit() { }
    }

    public class StateMachine
    {
        Dictionary<string, IState> stateDict = new Dictionary<string, IState>();
        IState currentState = new EmptyState();

        public IState Current { get { return currentState; } }
        public void Add(string id, IState state) { stateDict.Add(id, state); }
        public void Remove(string id) { stateDict.Remove(id); }
        public void Clear() { stateDict.Clear(); }


        public void Change(string id, params object[] args)
        {
            currentState.Exit();
            IState nextState = stateDict[id];
            nextState.Enter(args);
            currentState = nextState;
        }

        public void Update(float data)
        {
            currentState.Update(data);
        }

        public void HandleInput()
        {
            currentState.HandleInput();
        }
    }

    public class StandState : IState
    {
        StateMachine stateMachine;
        Ostrich ostrich;

        public StandState(Ostrich ostrich)
        {
            this.ostrich = ostrich;
            this.stateMachine = ostrich.StateMachine;
        }

        public void Update() { }

        public void HandleInput(string command)
        {
            switch (command)
            {
                case "flap":
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

        public Enter(params object[] args) { }
        public void Exit() { }
    }

    public class FlapState : IState
    {
        StateMachine stateMachine;
        Ostrich ostrich;

        public FlapState(Ostrich ostrich)
        {
            this.ostrich = ostrich;
            this.stateMachine = ostrich.StateMachine;
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

        public Enter(params object[] args) { }
        public void Exit() { }
    }
}
