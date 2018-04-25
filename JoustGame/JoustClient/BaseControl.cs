////////////////////////////////////////////////////////
// filename: BaseControl.cs
// contents: for the lower platform in the game
//
////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JoustModel;

namespace JoustClient
{
    public class BaseControl : WorldObjectControl
    {
        public BaseControl(string imagePath) : base(imagePath)
        {
            Height = 100;
            Width = 700;
        }

        // Set up spawn for 
        public void CreateSpawn(int x, int y) {
            Respawn respawn = new Respawn();
            RespawnControl rCtrl = new RespawnControl(respawn.imagePath);
            Canvas.SetZIndex(rCtrl, 3);
            respawn.coords.x = ((Width / 2) - 20) + x;
            respawn.coords.y = y;
            Canvas.SetLeft(rCtrl, respawn.coords.x);
            Canvas.SetTop(rCtrl, respawn.coords.y);
            Trace.WriteLine(Parent.ToString());
            Canvas canvas = Parent as Canvas;
            canvas.Children.Add(rCtrl);
        }
    }
}
