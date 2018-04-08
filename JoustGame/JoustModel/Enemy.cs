using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public abstract class Enemy : Entity
    {
        public enum EnemyState { Standing, Running_Right, Running_Left, InAir, InAir_Right, InAir_Left, Flapping, Flapping_Right, Flapping_Left };
    }
}