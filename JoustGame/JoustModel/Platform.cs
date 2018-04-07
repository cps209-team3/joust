using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class Platform : WorldObject
    {
        public Platform(Point coords)
        {
            imagePath = "Images/Platform/platform_short1.png";
            this.coords = coords;
            World.Instance.objects.Add(this);
        }

        public override string Serialize()
        {
            return string.Format("Platform, {0}", this.coords);
        }

        // Set coords to value read from file
        public override void Deserialize(string data)
        {
            // set coords
        }
    }
}
