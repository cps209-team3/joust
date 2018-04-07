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
using System.Windows.Navigation;
using System.Windows.Shapes;
using JoustModel;

namespace JoustClient
{
    public class BuzzardControl : Image
    {
        public BuzzardControl()
        {
            Source = new BitmapImage(new Uri("Images/Enemy/buzzard_stand.png", UriKind.Relative));
        }

        public void NotifyMoved(object sender, EventArgs e) {
            Buzzard buzzard = sender as Buzzard;

            // Determine the endpoint of the move vector
            double xPos = Canvas.GetLeft(this) + buzzard.speed * Math.Cos(buzzard.angle);
            double yPos = Canvas.GetTop(this) + buzzard.speed * Math.Sin(buzzard.angle);


            Canvas.SetTop(this, yPos);
            Canvas.SetLeft(this, xPos);
        }
    }
}
