using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JoustModel
{
    public class Map// : ISerializable
    {
        public List<Platform> platforms = new List<Platform>();
        
        
        //public override string Serialize()
        //{
        //    return string.Format("Map, {0}", this.numOfPlats, this.platCoords, this.numOfRespawns, this.respawnCoords);
        //}

        //// Set coords to value read from file
        //public override void Deserialize(string data)
        //{
        //    // set numOfPlats
        //    // set platCoords
        //    // set numOfRespawns
        //    // set respawnCoords
        //}
    }
}
