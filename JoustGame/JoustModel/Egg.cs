using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class Egg : Enemy
    {
        public override int Value { get; set; }

        public Egg()
        {
            Value = 250;
        }
    }
}