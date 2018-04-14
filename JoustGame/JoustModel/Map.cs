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
        public int score;
        public int lives;

        
        public string Serialize()
        {
            return string.Format("Map,{0},{1}", numOfPlats, numOfRespawns);
        }

        // Set coords to value read from file
        public void Deserialize(string data)
        {
            string[] properties = data.Split(',');
            numOfPlats = Convert.ToInt32(properties[1]);
            numOfRespawns = Convert.ToInt32(properties[2]);
        }
    }
}
