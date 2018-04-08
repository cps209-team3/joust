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
    public class BuzzardControl : Image
    {
        public BuzzardControl(string imagePath)
        {
            Height = 75;
            Width = 50;
            Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        public void NotifyMoved(object sender, EventArgs e) {
            Buzzard buzzard = sender as Buzzard;

            // Determine the endpoint of the move vector
            lock (Buzzard.lock_this) {
                double xPos = Canvas.GetLeft(this) + buzzard.speed * Math.Cos(((buzzard.angle + 180) * Math.PI / 180));
                double yPos = Canvas.GetTop(this) + buzzard.speed * Math.Sin(((buzzard.angle + 180) * Math.PI / 180));

                buzzard.coords = new JoustModel.Point(xPos, yPos);
                
                if (xPos < Canvas.GetLeft(this)) LayoutTransform = new ScaleTransform() { ScaleX = -1 };
                else LayoutTransform = new ScaleTransform() { ScaleX = 1 };

                if (xPos > (1440 - Width)) xPos -= 1440 - Width;
                else if (xPos < 0) xPos += 1440 - Width;

                if (yPos >= 0 && yPos <= 900 - Height) Canvas.SetTop(this, yPos);
                Canvas.SetLeft(this, xPos);
            }
        }

        public void NotifyState(object sender, EventArgs e) {
            Buzzard buzzard = sender as Buzzard;
            Source = new BitmapImage(new Uri(buzzard.imagePath, UriKind.Relative));
        }
    }
}
