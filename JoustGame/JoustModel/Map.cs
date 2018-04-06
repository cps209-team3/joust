using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JoustModel
{
    public class Map : ISerializable
    {
        public List<Platform> platforms = new List<Platform>();
    }
}