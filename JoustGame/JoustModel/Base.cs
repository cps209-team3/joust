using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JoustModel
{
    public class Base : Respawn
    {
        
        public override string Serialize()
        {
            return string.Format("Base, {0}", this.coords);
        }

        // Set coords to value read from file
        public override void Deserialize(string data)
        {
            // set coords
        }
    }
}
