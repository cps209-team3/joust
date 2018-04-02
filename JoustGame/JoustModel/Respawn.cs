using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class Respawn : Platform, ISerializable
    {
        public string Serialize()
        {
            return string.Format("Respawn, {1}", this.coords);
        }

        // Set coords to value read from file
        public void Deserialize(string coords)
        {
            this.coords = coords;
        }
    }
}
