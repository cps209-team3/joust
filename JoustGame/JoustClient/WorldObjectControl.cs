////////////////////////////////////////////////////////
// filename: WorldObjectControl.cs
// contents: Base class for GUI elements created in-game
//
////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using JoustModel;

namespace JoustClient
{
    public class WorldObjectControl : Image
    {
        /// <summary>
        /// takes the image path parameter and provides a GUI representation of a world object
        /// not positioned in this base class yet, so not yet visible
        /// </summary>
        /// <param name="imagePath"></param>
        public WorldObjectControl(string imagePath)
        {
            Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        /// <summary>
        /// provides the floating numbers for points when enemies are killed
        /// </summary>
        /// <param name="b"></param>
        /// <param name="color"></param>
        public void DisplayFloatingNumbers(Entity b, SolidColorBrush color)
        {
            Canvas canvas = Parent as Canvas;
            TextBlock floating = new TextBlock();
            floating.Text = Convert.ToString(b.Value);
            floating.Foreground = color;
            Canvas.SetTop(floating, b.coords.y);
            Canvas.SetLeft(floating, b.coords.x);
            canvas.Children.Add(floating);
            Task.Run(() =>
            {   // add animation later
                Dispatcher.Invoke(() => Canvas.SetTop(floating, b.coords.y - 1));
                Thread.Sleep(1000);
                Dispatcher.Invoke(() => canvas.Children.Remove(floating));
            });
        }
    }
}
