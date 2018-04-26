//-----------------------------------------------------------
//  File:   Point.cs
//  Desc:   Holds the Point class
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    //-----------------------------------------------------------
    //  Desc:   A simple point class that that holds an x and y coord.
    //----------------------------------------------------------- 
    public class Point
    {
        // x coordinate
        public double x;
        //y coordinate
        public double y;

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}