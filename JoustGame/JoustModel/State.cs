using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JoustModel
{
    public abstract class State
    {
        public string name;
        public int o = 0;
        [JsonIgnore]
        public Ostrich ostrich;
        [JsonIgnore]
        public StateMachine stateMachine;
        
        public State() { }
        
        public State(string name)
        {
            this.name = name;
            this.ostrich = World.Instance.players[World.Instance.playerNames.FindIndex(s => s == this.name)];
            this.stateMachine = this.ostrich.stateMachine;
        }

        [JsonConstructor]
        public State(string name, int o)
        {
            this.name = name;
        }

        public abstract void Update();
        public abstract void HandleInput(string data);
        public abstract void Enter();
        public abstract void Exit();
        public abstract void CheckCollisions();
    }
}
