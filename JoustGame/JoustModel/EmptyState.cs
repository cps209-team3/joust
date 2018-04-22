using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace JoustModel
{
    public class EmptyState : IState
    {
        public void Update() { }
        public void HandleInput(string data) { }
        public void Enter() { }
        public void Exit() { }
        public void CheckCollisions() { }
    }
}
