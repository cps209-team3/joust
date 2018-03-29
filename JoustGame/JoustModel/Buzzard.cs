using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class Buzzard : Enemy
    {
        public override int Value { get; set; }

        public Buzzard()
        {
            Value = 500;
        }
    }
}