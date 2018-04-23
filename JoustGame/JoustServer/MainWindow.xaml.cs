using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace JoustServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ServerCommunicationManager commManager;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            commManager = new ServerCommunicationManager(this);
            Log("Listening on port " + ServerCommunicationManager.PORT);

            while (true)
            {
                TcpClient client = await commManager.WaitForClientAsync();
                Log("Received incoming connection.");
                commManager.HandleClientAsync(client);
            }
        }

        public void Log(string msg)
        {
            txtLog.Text += msg + "\n";
        }
    }
}

