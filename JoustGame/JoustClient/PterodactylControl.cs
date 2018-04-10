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
    public class PterodactylControl : WorldObjectControl
    {
        private bool spawned;

        public PterodactylControl(string imagePath) : base(imagePath)
        {
            Height = 30;
            Width = 120;
            spawned = false;
            Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        public void NotifyMoved(object sender, EventArgs e) {
            Pterodactyl pterodactyl = sender as Pterodactyl;

            // Determine the endpoint of the move vector
            lock (Buzzard.lock_this) {
                double xPos = Canvas.GetLeft(this) + pterodactyl.speed * Math.Cos(((pterodactyl.angle + 180) * Math.PI / 180));
                double yPos = Canvas.GetTop(this) + pterodactyl.speed * Math.Sin(((pterodactyl.angle + 180) * Math.PI / 180));

                pterodactyl.coords = new JoustModel.Point(xPos, yPos);

                if (xPos < Canvas.GetLeft(this)) LayoutTransform = new ScaleTransform() { ScaleX = 1 };
                else LayoutTransform = new ScaleTransform() { ScaleX = -1 };

                if (xPos > (1440 - Width)) xPos -= 1440 - Width;
                else if (xPos < 0) xPos += 1440 - Width;

                if (yPos >= 0 && yPos <= 900 - Height) Canvas.SetTop(this, yPos);
                Canvas.SetLeft(this, xPos);
            }
        }

        public void NotifyState(object sender, EventArgs e) {
            Pterodactyl pterodactyl = sender as Pterodactyl;
            Source = new BitmapImage(new Uri(pterodactyl.imagePath, UriKind.Relative));
        }

        public void NotifyDestroy(object sender, EventArgs e) {
            Canvas canvas = Parent as Canvas;
            canvas.Children.Remove(this);
        }

        public void NotifySpawn(object sender, EventArgs e) {
            if (!spawned) {
                Trace.WriteLine("Spawn a Pterodactyl");
                Pterodactyl p = new Pterodactyl(new JoustModel.Point(0, 0));
                PterodactylControl pCtrl = new PterodactylControl(p.imagePath);
                p.PterodactylMoveEvent += pCtrl.NotifyMoved;
                p.PterodactylStateChange += pCtrl.NotifyState;
                p.PterodactylDestroyed += pCtrl.NotifyDestroy;

                Canvas.SetTop(pCtrl, p.coords.y);
                Canvas.SetLeft(pCtrl, p.coords.x);
                Canvas canvas = Parent as Canvas;
                canvas.Children.Add(pCtrl);
                spawned = true;
            }
        }
    }
}
