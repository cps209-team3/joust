using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JoustModel
{
    public interface IState
    {
        void Update();
        void HandleInput(string data);
        void Enter();
        void Exit();
    }
}
