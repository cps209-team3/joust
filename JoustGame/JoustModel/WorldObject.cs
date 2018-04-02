using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public abstract class WorldObject
    {
        public Point coords;
        public Hitbox hitbox;
        public string image;
    }
}