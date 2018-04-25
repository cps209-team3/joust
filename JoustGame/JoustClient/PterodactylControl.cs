using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using JoustModel;

namespace JoustClient
{
    public class PterodactylControl : WorldObjectControl
    {
        // Ensures only 1 Pterodactyl is spawned by this Control
        private bool spawned;

        // Class Constructor
        public PterodactylControl(string imagePath) : base(imagePath)
        {
            Height = 30;
            Width = 120;
            spawned = false;
            Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        /// <summary>
        /// Runs when the PterodactylMoveEvent fires. It updates the position
        /// of the Control to that of the model.
        /// </summary>
        public void NotifyMoved(object sender, EventArgs e) {
            Pterodactyl pterodactyl = sender as Pterodactyl;

            // Determine the endpoint of the move vector
            double xPos = Canvas.GetLeft(this) + pterodactyl.speed * Math.Cos(((pterodactyl.angle + 180) * Math.PI / 180));
            double yPos = Canvas.GetTop(this) + pterodactyl.speed * Math.Sin(((pterodactyl.angle + 180) * Math.PI / 180));

            pterodactyl.coords = new Point(xPos, yPos);

            // Flipping the image to turn left or right
            if (xPos < Canvas.GetLeft(this)) LayoutTransform = new ScaleTransform() { ScaleX = 1 };
            else LayoutTransform = new ScaleTransform() { ScaleX = -1 };

            // Handles transporting the Control from one side of the screen to the other
            if (xPos > (1440 - Width)) xPos -= 1440 - Width;
            else if (xPos < 0) xPos += 1440 - Width;

            if (yPos >= 0 && yPos <= 900 - (Height * 2)) Canvas.SetTop(this, yPos);
            Canvas.SetLeft(this, xPos);
        }

        /// <summary>
        /// Runs when the PterodactylStateChange fires. It updates the graphic
        /// of the Control to that of the model's state.
        /// </summary>
        public void NotifyState(object sender, EventArgs e) {
            Pterodactyl pterodactyl = sender as Pterodactyl;
            Source = new BitmapImage(new Uri(pterodactyl.imagePath, UriKind.Relative));
        }

        /// <summary>
        /// Runs when the PterodactylDestroyed fires. It removes this Control.
        /// </summary>
        public void NotifyDestroy(object sender, EventArgs e)
        {
            DisplayFloatingNumbers(sender as Entity, new SolidColorBrush(Colors.Red));
            Canvas canvas = Parent as Canvas;
            canvas.Children.Remove(this);

            Task.Run(() => {
                PlaySounds.Instance.Play_Drop();
            });
        }

        /// <summary>
        /// Runs when the World.Instance.SpawnPterodactyl fires. It creates the 
        /// Pterodactyls.
        /// </summary>
        public void NotifySpawn(object sender, EventArgs e) {
            if (!spawned) {
                Pterodactyl p = new Pterodactyl();
                p.coords = new Point(0, 0); // *** Set this to a respawn platform ***
                PterodactylControl pCtrl = new PterodactylControl(p.imagePath);

                // Subscribe to the event handlers
                p.PterodactylMoveEvent += pCtrl.NotifyMoved;
                p.PterodactylStateChange += pCtrl.NotifyState;
                p.PterodactylDestroyed += pCtrl.NotifyDestroy;

                Canvas.SetTop(pCtrl, p.coords.y);
                Canvas.SetLeft(pCtrl, p.coords.x);
                Canvas canvas = Parent as Canvas;
                canvas.Children.Add(pCtrl);
                spawned = true;

                Task.Run(() => {
                    PlaySounds.Instance.Play_Spawn();
                });
            }
        }
    }
}
