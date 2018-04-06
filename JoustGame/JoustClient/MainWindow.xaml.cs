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

        public void LoadGameView()
        {
            // Get stage num from controls once the proper screens are implemented
            JoustModel.Point coords = new JoustModel.Point(720, 450);
            Ostrich o = new Ostrich(coords);
            OstrichControl oControl = new OstrichControl(o.imagePath);
            Canvas.SetTop(oControl, coords.y);
            Canvas.SetLeft(oControl, coords.x);
            canvas.Children.Add(oControl);

            int stage = 0;
            control.WorldObj.stage = stage;
            int numBuzzards = 0;
            int numPterodactyls = 0;
            control.CalculateNumEnemies(control.WorldObj.stage, ref numBuzzards, ref numPterodactyls);

            for (int i = 0; i < numBuzzards; i++)
            {
                JoustModel.Point bCoords = new JoustModel.Point((i + 1) * 50, (i + 1) * 50);
                Buzzard b = new Buzzard(bCoords);
                BuzzardControl bControl = new BuzzardControl(b.imagePath);
                Canvas.SetTop(bControl, bCoords.y);
                Canvas.SetLeft(bControl, bCoords.x);
                canvas.Children.Add(bControl);
            }

            for (int i = 0; i < numPterodactyls; i++)
            {
                JoustModel.Point pCoords = new JoustModel.Point((i + 1) * 50, (i + 1) * 50);
                Pterodactyl p = new Pterodactyl(pCoords);
                PterodactylControl pControl = new PterodactylControl(p.imagePath);
                Canvas.SetTop(pControl, pCoords.y);
                Canvas.SetLeft(pControl, pCoords.x);
                canvas.Children.Add(pControl);
            }
        }
    }
}
