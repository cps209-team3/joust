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
            // To load straight into a game for faster testing
            // Comment this out if needed
            LoadGameView();
        }

        // Usually only called by single player or multiplayer game setup screen
        public void LoadGameView()
        {
            // When we implement level progression, will need to get stage number from view controls instead
            int stageNum = 0;
            int numBuzzards = 0;
            int numPterodactyls = 0;
            control.CalculateNumEnemies(stageNum, ref numBuzzards, ref numPterodactyls);
        }
    }
}
