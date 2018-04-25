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
        public WorldObjectControl(string imagePath)
        {
            Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

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
