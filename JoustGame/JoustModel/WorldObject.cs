using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public abstract class WorldObject : ISerializable
    {
        public Point coords;
        public Hitbox hitbox;
        public string imagePath;

        abstract public string Serialize();
        abstract public void Deserialize(string data);
    }
}
