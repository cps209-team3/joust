using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public class OstrichControl : WorldObjectControl
    {
        public OstrichControl(string imagePath) : base(imagePath)
        {
            Height = 75;
            Width = 50;
        }

        public void NotifyMoved(object sender, int e)
        {
            Ostrich o = sender as Ostrich;
            Canvas.SetTop(this, o.coords.y);
            Canvas.SetLeft(this, o.coords.x);

            if (o.leftDown) LayoutTransform = new ScaleTransform() { ScaleX = -1 };
            else if (o.rightDown) LayoutTransform = new ScaleTransform() { ScaleX = 1 };

            if (o.stateMachine.Current is StandState) {
                Source = new BitmapImage(new Uri(o.imagePath, UriKind.Relative));
            }
            else if (o.stateMachine.Current is FallState) {
                Source = new BitmapImage(new Uri("Sprites/player_fly1.png", UriKind.Relative));
            }
            else if (o.stateMachine.Current is FlapState) {
                Task.Run(() => {
                    Dispatcher.Invoke(() => Source = new BitmapImage(new Uri("Sprites/player_fly2.png", UriKind.Relative)));
                    Thread.Sleep(900);
                    Dispatcher.Invoke(() => Source = new BitmapImage(new Uri("Sprites/player_fly1.png", UriKind.Relative)));
                });
            }
        }
    }
}
