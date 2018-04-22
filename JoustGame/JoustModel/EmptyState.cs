using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace JoustModel
{
    public class EmptyState : State
    {
        public EmptyState() : base() { }
        public EmptyState(string name) : base(name) { }

        public override void Update() { }
        public override void HandleInput(string data) { }
        public override void Enter() { }
        public override void Exit() { }
        public override void CheckCollisions() { }
    }
}