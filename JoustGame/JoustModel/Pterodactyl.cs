using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class Pterodactyl : Enemy
    {
        public override int Value { get; set; }

        public Pterodactyl()
        {
            Value = 1000;
        }
    }
}