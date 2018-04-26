//-----------------------------------------------------------
//  File:   Base.cs
//  Desc:   Holds the Base class
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace JoustModel
{
    //-----------------------------------------------------------
    //  Desc: Handles initialization and saveing/loading of Base object
    //----------------------------------------------------------- 
    public class Base : Platform
    {

        /// <summary>
        /// Constructor for Base class
        /// </summary>
        public Base() : base()
        {
            height = 100;
            width = 700;
            type = "Base";
            imagePath = "Images/Platform/platform_bottom.png";
            World.Instance.basePlatform = this;
        }

        // returns the properties of this Base object in string form
        public override string Serialize()
        {
            return string.Format("Base,{0},{1}", coords.x, coords.y);
        }

        // Set coords to value read from file
        public override void Deserialize(string data)
        {
            string[] properties = data.Split(',');
            coords.x = Convert.ToDouble(properties[1]); // set x coord
            coords.y = Convert.ToDouble(properties[2]); // set y coord
        }

        public override string ToString()
        {
            return "Base";
        }
    }
}
