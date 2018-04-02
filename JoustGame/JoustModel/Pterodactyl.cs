using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class Pterodactyl : Enemy, ISerializable
    {
        public override int Value { get; set; }

        public Pterodactyl()
        {
            Value = 1000;
        }
        
        
        //Serialization
        public string Serialize()
        {
            return string.Format("Pterodactyl, {0}, {1}, {2}", this.coords, this.speed, this.angle);
        }

        public void Deserialize(string coords, string speed, string angle)
        {
            this.coords = coords; //convert to point
            this.speed = Convert.ToDouble(speed);
            this.angle = Convert.ToDouble(angle);

        }
    }
}
