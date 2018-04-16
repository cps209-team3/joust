using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class Platform : WorldObject
    {
        public Platform()
        {
            hitbox.width = 300;
            hitbox.height = 30;
            type = "Platform";
            imagePath = "Images/Platform/platform_short1.png";
            World.Instance.objects.Add(this);
        }

        public override string Serialize()
        {
            return string.Format("Platform,{0},{1}", coords.x, coords.y);
        }

        // Set coords to value read from file
        public override void Deserialize(string data)
        {
            string[] properties = data.Split(',');
            coords.x = Convert.ToDouble(properties[1]); // set x coord
            coords.y = Convert.ToDouble(properties[2]); // set y coord
        }

        public override string ToString()
        {
            return "Platform";
        }
    }
}
