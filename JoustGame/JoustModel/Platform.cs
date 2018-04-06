using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class Platform : WorldObject
    {
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
