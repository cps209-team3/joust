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

        public override string Serialize()
        {
            return string.Format("Buzzard, {0}, {1}, {2}", this.coords, this.speed, this.angle);
        }

        public override void Deserialize(string data)
        {
            //set coords
            //set speed 
            //set angle 
        }
    }
}
