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

        public Respawn(Point coords) : base(coords)
        {
            imagePath = "Images/Platform/platform_short2.png";
        }

        public override string Serialize()
        {
            return string.Format("Respawn, {1}", this.coords);
        }

        // Set coords to value read from file
        public override void Deserialize(string data)
        {
            // set coords
        }
    }
}
