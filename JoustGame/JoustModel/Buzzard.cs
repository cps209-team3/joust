using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class Buzzard : Enemy, Iserialization
    {
        public override int Value { get; set; }

        public Buzzard()
        {
            Value = 500;
        }
        
        //Serialization
        public string Serialize()
        {
            return string.Format("Buzzard, {0}, {1}, {2}", this.coords, this.speed, this.angle);
        }

        public void Deserialize(string coords, string speed, string angle)
        {
            this.coords = coords; //convert to point
            this.speed = Convert.ToDouble(speed);
            this.angle = Convert.ToDouble(angle);
        }
    }
}
