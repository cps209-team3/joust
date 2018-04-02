using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class Platform : WorldObject, ISerializable
    {
        public string Serialize()
        {
            return Convert.ToString(this.coords);
        }

        // Set coords to value read from file
        public void Deserialize(string coords)
        {
            this.coords = coords;
        }
    }
}