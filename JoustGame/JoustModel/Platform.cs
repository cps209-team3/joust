using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class Platform : WorldObject, ISerializable
    {

        // Convert coords to one line string to be put into save file
        public string Serialize()
        {
            return Convert.ToString(this.coords);
        }

        // Set coords to value read from file
        public void Deserialize(string data)
        {
            // set coords
        }
    }
}
