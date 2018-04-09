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
    public class EggControl : WorldObjectControl
    {
        public EggControl(string imagePath) : base(imagePath)
        {
            Height = 30;
            Width = 30;
        }

        public void NotifyMoved(object sender, EventArgs e) {
            Egg egg = sender as Egg;

            double xPos = Canvas.GetLeft(this) + egg.speed * Math.Cos(((egg.angle + 180) * Math.PI / 180));
            double yPos = Canvas.GetTop(this) + egg.speed * Math.Sin(((egg.angle + 180) * Math.PI / 180));

            egg.coords = new JoustModel.Point(xPos, yPos);

            if (xPos < Canvas.GetLeft(this)) LayoutTransform = new ScaleTransform() { ScaleX = -1 };
            else LayoutTransform = new ScaleTransform() { ScaleX = 1 };

            if (xPos > (1440 - Width)) xPos -= 1440 - Width;
            else if (xPos < 0) xPos += 1440 - Width;

            if (yPos >= 0 && yPos <= 900 - Height) Canvas.SetTop(this, yPos);
            Canvas.SetLeft(this, xPos);
        }

        public void NotifyState(object sender, EventArgs e) {
            Egg egg = sender as Egg;
            Source = new BitmapImage(new Uri(egg.imagePath, UriKind.Relative));
        }

        public void NotifyHatch(object sender, EventArgs e) {
            Egg egg = sender as Egg;

            JoustModel.Point p = new JoustModel.Point(0, egg.coords.y - 70);

            Buzzard b = new Buzzard(p);
            b.angle = 0;
            b.droppedEgg = true;
            b.state = new BuzzardPickupState() { Angle = (int)b.angle, TargetEgg = egg, StateEnemy = b };
            b.state.Setup();

            BuzzardControl eCtrl = new BuzzardControl(b.imagePath);
            b.BuzzardMoveEvent += eCtrl.NotifyMoved;
            b.BuzzardStateChange += eCtrl.NotifyState;
            b.BuzzardDropEgg += eCtrl.NotifyDrop;
            b.BuzzardDestroyed += eCtrl.NotifyDestroy;

            Canvas.SetTop(eCtrl, b.coords.y);
            Canvas.SetLeft(eCtrl, b.coords.x);
            Canvas canvas = Parent as Canvas;
            canvas.Children.Add(eCtrl);
        }

        public void NotifyDestroy(object sender, EventArgs e) {
            Canvas canvas = Parent as Canvas;
            canvas.Children.Remove(this);
        }
    }
}
