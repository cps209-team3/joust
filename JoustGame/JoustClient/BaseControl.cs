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

        public void CreateSpawn(int x, int y) {
            RespawnControl rCtrl = new RespawnControl("Images/Platform/platform_respawn1.png");
            Canvas.SetZIndex(rCtrl, 3);
            Canvas.SetLeft(rCtrl, ((Width / 2) - 20) + x);
            Canvas.SetTop(rCtrl, y);
            Trace.WriteLine(Parent.ToString());
            Canvas canvas = Parent as Canvas;
            canvas.Children.Add(rCtrl);
        }
    }
}
