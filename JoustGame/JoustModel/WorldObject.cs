using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace JoustModel
{
    public abstract class WorldObject : ISerializable
    {
        public Point coords = new Point(0,0);
        //public Hitbox hitbox;
        public int height;
        public int width;
        public string imagePath;
        public string type;


        abstract public string Serialize();
        abstract public void Deserialize(string data);
    }
}
