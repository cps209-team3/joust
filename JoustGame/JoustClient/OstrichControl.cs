using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private string currentMove;
        public OstrichControl(string imagePath) : base(imagePath)
        {
            Height = 75;
            Width = 50;
            currentMove = "Sprites/player_stand.png";
        }

        public void NotifyMoved(object sender, int e)
        {
            Ostrich o = sender as Ostrich;
            Canvas.SetTop(this, o.coords.y);
            Canvas.SetLeft(this, o.coords.x);

            if (o.leftDown) LayoutTransform = new ScaleTransform() { ScaleX = -1 };
            else if (o.rightDown) LayoutTransform = new ScaleTransform() { ScaleX = 1 };

            if (o.stateMachine.Current is StandState) {
                if (o.leftDown || o.rightDown) {
                    if (currentMove.EndsWith("stand.png")) currentMove = "Sprites/player_move3.png";
                    else if (currentMove.EndsWith("move3.png")) currentMove = "Sprites/player_move2.png";
                    else if (currentMove.EndsWith("move2.png")) currentMove = "Sprites/player_move1.png";
                    else currentMove = "Sprites/player_stand.png";

                    Task.Run(() => {
                        Dispatcher.Invoke(() => Source = new BitmapImage(new Uri(currentMove, UriKind.Relative)));
                    });
                }
                else {
                    Source = new BitmapImage(new Uri(o.imagePath, UriKind.Relative));
                }
            }
            else if (o.stateMachine.Current is FallState) {
                Source = new BitmapImage(new Uri("Sprites/player_fly1.png", UriKind.Relative));
            }

        }
    }
}
