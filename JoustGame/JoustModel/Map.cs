using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JoustModel
{
    public class Map : ISerializable
    {
        public List<Platform> platforms = new List<Platform>();
        public int numOfPlats;
        public int numOfRespawns;
        
        
        public string Serialize()
        {
            return string.Format("Map,{0},{1}", this.numOfPlats, this.numOfRespawns);
        }

        // Set coords to value read from file
        public void Deserialize(string data)
        {
            // set numOfPlats
            // set numOfRespawns
        }
    }
}
