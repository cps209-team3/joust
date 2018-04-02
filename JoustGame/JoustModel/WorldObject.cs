using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    interface Iserialization
    {
        void Serialize();
        void Deserialize();
    }
    
    public abstract class WorldObject
    {
        public Point coords;
        public Hitbox hitbox;
        public string image;
    }
}
