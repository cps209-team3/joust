using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class Egg : Enemy, ISerializable
    {
        public override int Value { get; set; }

        public Egg()
        {
            Value = 250;
        }

        public string Serialize()
        {
            return string.Format("Egg, {0}, {1}, {2}", this.coords, this.speed, this.angle);
        }

        public void Deserialize(string coords, string speed, string angle)
        {
            this.coords = coords; //convert to point
            this.speed = speed; //convert to double
            this.angle = angle; //convert to double
        }
    }
}