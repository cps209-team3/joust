using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class Ostrich : Entity
    {
        public int score;

        public override int Value { get; set; }

        public Ostrich()
        {
            Value = 750;
            score = 0;
        }
    }
}