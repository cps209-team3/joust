using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using JoustModel;

namespace JoustClient
{
    public class EggControl : WorldObjectControl
    {
        // Class Constructor
        public EggControl(string imagePath) : base(imagePath)
        {
            Height = 30;
            Width = 30;
        }

        /// <summary>
        /// Runs when the EggMoveEvent fires. It updates the position
        /// of the Control to that of the model.
        /// </summary>
        public void NotifyMoved(object sender, EventArgs e) {
            Egg egg = sender as Egg;

            // Determine the endpoint of the move vector
            double xPos = Canvas.GetLeft(this) + egg.speed * Math.Cos(((egg.angle + 180) * Math.PI / 180));
            double yPos = Canvas.GetTop(this) + egg.speed * Math.Sin(((egg.angle + 180) * Math.PI / 180));

            egg.coords = new JoustModel.Point(xPos, yPos);

            // Flipping the image to turn left or right
            if (xPos < Canvas.GetLeft(this)) LayoutTransform = new ScaleTransform() { ScaleX = -1 };
            else LayoutTransform = new ScaleTransform() { ScaleX = 1 };

            // Handles transporting the Control from one side of the screen to the other
            if (xPos > (1440 - Width)) xPos -= 1440 - Width;
            else if (xPos < 0) xPos += 1440 - Width;

            if (yPos >= 0 && yPos <= 900 - Height) Canvas.SetTop(this, yPos);
            Canvas.SetLeft(this, xPos);
        }

        /// <summary>
        /// Runs when the EggStateChange fires. It updates the graphic
        /// of the Control to that of the model's state.
        /// </summary>
        public void NotifyState(object sender, EventArgs e) {
            Egg egg = sender as Egg;
            Source = new BitmapImage(new Uri(egg.imagePath, UriKind.Relative));
        }

        /// <summary>
        /// Runs when the EggHatched fires. It updates the graphic
        /// of the Control to a hatched Mik and creates a Buzzard to
        /// pick up the Mik.
        /// </summary>
        public void NotifyHatch(object sender, EventArgs e) {
            Egg egg = sender as Egg;

            Point p = new Point(0, egg.coords.y - 70);
            // Create a new Buzzard
            Buzzard b = new Buzzard();
            b.coords = p;
            b.angle = 0;
            b.droppedEgg = true;
            // Set state to Pickup state
            b.stateMachine.Change("pickup");
            b.pickupEgg = egg;
            b.stateMachine.currentState.Update();
            // Create a new BuzzardControl
            BuzzardControl eCtrl = new BuzzardControl(b.imagePath);
            // Subscribe to event handlers
            b.BuzzardMoveEvent += eCtrl.NotifyMoved;
            b.BuzzardStateChange += eCtrl.NotifyState;
            b.BuzzardDropEgg += eCtrl.NotifyDrop;
            b.BuzzardDestroyed += eCtrl.NotifyDestroy;

            Canvas.SetTop(eCtrl, b.coords.y);
            Canvas.SetLeft(eCtrl, b.coords.x);
            Canvas canvas = Parent as Canvas;
            canvas.Children.Add(eCtrl);
        }

        /// <summary>
        /// Runs when the EggDestroyed fires. It removes this Control.
        /// </summary>
        public void NotifyDestroy(object sender, EventArgs e) {
            Canvas canvas = Parent as Canvas;
            canvas.Children.Remove(this);
        }
    }
}
