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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GameController control = new GameController();

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // This is only here for faster testing
            // If you need a different screen on window load, comment out the line below
            LoadGameView();
        }

        public void ImageFactory(string control, JoustModel.Point point)
        {
            Image i;
            switch (control)
            {
                case "ostrich":
                    Ostrich o = new Ostrich(point);
                    i = new OstrichControl(o.imagePath);
                    break;
                case "buzzard":
                    Buzzard b = new Buzzard(point);
                    i = new BuzzardControl(b.imagePath);
                    break;
                case "pterodactyl":
                    Pterodactyl p = new Pterodactyl(point);
                    i = new PterodactylControl(p.imagePath);
                    break;
                case "egg":
                    Egg e = new Egg(point);
                    i = new EggControl(e.imagePath);
                    break;
                case "platform":
                    Platform pl = new Platform(point);
                    i = new PlatformControl(pl.imagePath);
                    break;
                case "respawn":
                    Respawn r = new Respawn(point);
                    i = new RespawnControl(r.imagePath);
                    break;
                default:
                    Base ba = new Base(point);
                    i = new BaseControl(ba.imagePath);
                    break;
            }
            Canvas.SetTop(i, point.y);
            Canvas.SetLeft(i, point.x);
            canvas.Children.Add(i);
        }

        public void LoadGameView()
        {
            // Get stage num from controls once the proper screens are implemented
            JoustModel.Point oCoords = new JoustModel.Point(720, 450);
            ImageFactory("ostrich", oCoords);

            int stage = 0;
            control.WorldObj.stage = stage;
            int numBuzzards = 0;
            int numPterodactyls = 0;
            control.CalculateNumEnemies(control.WorldObj.stage, ref numBuzzards, ref numPterodactyls);

            for (int i = 0; i < numBuzzards; i++)
            {
                JoustModel.Point bCoords = new JoustModel.Point((i + 1) * 50, (i + 1) * 50);
                ImageFactory("buzzard", bCoords);
            }

            for (int i = 0; i < numPterodactyls; i++)
            {
                JoustModel.Point pCoords = new JoustModel.Point((i + 1) * 50, (i + 1) * 50);
                ImageFactory("pterodactyl", pCoords);
            }
        }
    }
}
