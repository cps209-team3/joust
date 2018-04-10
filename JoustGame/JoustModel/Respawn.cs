using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class Respawn : Platform
    {
        public string respawnImagePath = "Images/Platform/platform_respawn1.png";
        public Respawn() : base()
        {
            imagePath = "Images/Platform/platform_short2.png";
        }

        public override string Serialize()
        {
            return string.Format("Respawn,{0},{1}", coords.x, coords.y);
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
