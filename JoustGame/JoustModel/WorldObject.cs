//-----------------------------------------------------------
//  File:   WorldObject.cs
//  Desc:   Holds the WorldObject class
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace JoustModel
{
    //-----------------------------------------------------------
    //  Desc:   Base class for all world objects
    //----------------------------------------------------------- 
    public abstract class WorldObject : ISerializable
    {
        // Position of the object
        public Point coords = new Point(0,0);
        // height
        public int height;
        //width 
        public int width;
        // path to image
        public string imagePath;
        // type of object
        public string type;


        abstract public string Serialize();
        abstract public void Deserialize(string data);
    }
}
