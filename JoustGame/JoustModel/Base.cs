using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JoustModel
{
    public class Base : Respawn
    {
        public Base(Point coords) : base(coords)
        {
            imagePath = "Images/Platform/platform_bottom.png";
        }

        public override string Serialize()
        {
            return string.Format("Base, {0}", this.coords);
        }

        // Set coords to value read from file
        public override void Deserialize(string data)
        {
            string[] properties = data.Split(',');
            coords.x = Convert.ToDouble(properties[1]); // set x coord
            coords.y = Convert.ToDouble(properties[2]); // set y coord
        }
    }
}
