﻿using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using JoustModel;

namespace JoustClient
{
    public class BuzzardControl : WorldObjectControl
    {
        // Class Constructor
        public BuzzardControl(string imagePath) : base(imagePath) 
        {
            Height = 75;
            Width = 50;
            Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }

        /// <summary>
        /// Runs when the BuzzardMoveEvent fires. It updates the position
        /// of the Control to that of the model.
        /// </summary>
        public void NotifyMoved(object sender, EventArgs e) {
            Buzzard buzzard = sender as Buzzard;

            // Determine the endpoint of the move vector
            double xPos = Canvas.GetLeft(this) + buzzard.speed * Math.Cos(((buzzard.angle + 180) * Math.PI / 180));
            double yPos = Canvas.GetTop(this) + buzzard.speed * Math.Sin(((buzzard.angle + 180) * Math.PI / 180));

            buzzard.coords = new Point(xPos, yPos);

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
        /// Runs when the BuzzardStateChange fires. It updates the graphic
        /// of the Control to that of the model's state.
        /// </summary>
        public void NotifyState(object sender, EventArgs e) {
            Buzzard buzzard = sender as Buzzard;
            Source = new BitmapImage(new Uri(buzzard.imagePath, UriKind.Relative));
        }

        /// <summary>
        /// Runs when the BuzzardDropEgg fires. It creates an Egg object
        /// and sends it in the direction the Buzzard was going. The graphic
        /// is changed to just the Buzzard as it flies to the left of the screen
        /// and disappears.
        /// </summary>
        public void NotifyDrop(object sender, EventArgs e) {
            Buzzard buzzard = sender as Buzzard;
            // Create a new Egg
            Egg egg = new Egg();
            egg.coords = buzzard.coords;

            // Set to a downwards angle
            if (buzzard.angle > 180) egg.angle = buzzard.angle;
            else egg.angle = buzzard.angle + 180;
            egg.speed = buzzard.speed;
            egg.state = new EnemyFallingState() { Angle = buzzard.state.Angle, StateEnemy = egg };

            // Create a new EggControl
            EggControl eCtrl = new EggControl(egg.imagePath);
            // Subscribe to the event handlers
            egg.EggMoveEvent += eCtrl.NotifyMoved;
            egg.EggStateChange += eCtrl.NotifyState;
            egg.EggHatched += eCtrl.NotifyHatch;
            egg.EggDestroyed += eCtrl.NotifyDestroy;

            Canvas.SetTop(eCtrl, egg.coords.y);
            Canvas.SetLeft(eCtrl, egg.coords.x);
            Canvas canvas = Parent as Canvas;
            canvas.Children.Add(eCtrl);
        }

        /// <summary>
        /// Runs when the BuzzardDestroyed fires. It removes this Control.
        /// </summary>
        public void NotifyDestroy(object sender, EventArgs e) {
            Canvas canvas = Parent as Canvas;
            canvas.Children.Remove(this);
        }
    }
}
