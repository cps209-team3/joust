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
using System.Windows.Threading;
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

            //Title_Screen(null, EventArgs.Empty);
        }
        
        public void WorldObjectFactory(string control, JoustModel.Point point)
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
                    BuzzardControl iCtrl = i as BuzzardControl;

                    // Used to update the view with model updates
                    b.BuzzardMoveEvent += iCtrl.NotifyMoved;
                    b.BuzzardStateChange += iCtrl.NotifyState;
                    b.BuzzardDropEgg += iCtrl.NotifyDrop;
                    b.BuzzardDestroyed += iCtrl.NotifyDestroy;
                    // Used to update all enemies in the world
                    DispatcherTimer moveTimer = new DispatcherTimer();
                    moveTimer.Interval = new TimeSpan(0, 0, 0, 0, 33);
                    moveTimer.Tick += World.Instance.UpdateAllEnemies_Position;
                    moveTimer.Start();

                    /*  Comment:    Clayton Cockrell
                     *  The Random object in Buzzard would give the same random number to all the 
                     *  Buzzard objects if their creation was not halted for a little bit of time.
                     */
                    Thread.Sleep(20);
                    break;
                case "pterodactyl":
                    PterodactylControl pCtrl = new PterodactylControl("Images/Enemy/pterodactyl.fly1");
                    i = pCtrl;

                    /*  Comment:    Clayton Cockrell
                     *  Pterodactyls spawn after a certain number of minutes. I currently have it set
                     *  to 1 minute. (To change, see PTERODACTYL_SPAWN_MINUTES constant in World class)
                     */
                    World.Instance.SpawnPterodactyl += pCtrl.NotifySpawn;
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
            // Load Map here

            // Get stage num from controls once the proper screens are implemented
            JoustModel.Point oCoords = new JoustModel.Point(720, 450);
            WorldObjectFactory("ostrich", oCoords);

            /*  Comment:    Clayton Cockrell
             *  Pterodactyls start spawning at stage 5. stage is set this for testing
             *  purposes.
             */
            int stage = 5;
            control.WorldObj.stage = stage;
            int numBuzzards = 0;
            int numPterodactyls = 0;
            int numPlatforms = 4;
            control.CalculateNumEnemies(control.WorldObj.stage, ref numBuzzards, ref numPterodactyls);

            for (int i = 0; i < numBuzzards; i++)
            {
                JoustModel.Point bCoords = new JoustModel.Point((i + 1) * 100, i);
                WorldObjectFactory("buzzard", bCoords);
            }

            for (int i = 0; i < numPterodactyls; i++)
            {
                JoustModel.Point pCoords = new JoustModel.Point((i + 1) * 50, (i + 1) * 50);
                WorldObjectFactory("pterodactyl", pCoords);
            }

            for (int i = 0; i < numPlatforms; i++) {
                JoustModel.Point pCoords = new JoustModel.Point((i + 1) * 300, (i + 1) * 300);
                WorldObjectFactory("platform", pCoords);
            }
        }
       
       // title screens
        private Button Make_Button(string content, double top, RoutedEventHandler eventx)
        {
            Button btnReturn = new Button();
            btnReturn.Content = content;
            btnReturn.SetValue(Canvas.TopProperty, top);

            if (!(eventx == null))
            {
                btnReturn.Click += new RoutedEventHandler(eventx);
            }

            return btnReturn;
        }

        private void Title_Screen(object sender, EventArgs e)
        {
            canvas.Children.Clear();

            List<Button> btnList = new List<Button>();

            Button startSingle = Make_Button("Single Player", 100.0, Single_Screen);
            Button startMulti = Make_Button("Mulitplayer", 200.0, Multi_Screen);
            Button help = Make_Button("Help", 300.0, Help_Screen);
            Button about = Make_Button("About", 400.0, About_Screen);
            Button scores = Make_Button("High Scores", 500.0, HighScores_Screen);

            btnList.Add(startSingle);
            btnList.Add(startMulti);
            btnList.Add(help);
            btnList.Add(about);
            btnList.Add(scores);

            foreach (Button x in btnList)
            {
                x.Height = 100;
                x.Width = 200;
                canvas.Children.Add(x);
                x.SetValue(Canvas.LeftProperty, 620.0);
            }
        }

        private void Single_Screen(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            List<Button> btnList = new List<Button>();

            Button easy = Make_Button("Easy", 100.0, null);
            Button medium = Make_Button("Medium", 200.0, null);
            Button hard = Make_Button("Hard", 300.0, null);

            btnList.Add(easy);
            btnList.Add(medium);
            btnList.Add(hard);

            foreach (Button x in btnList)
            {
                x.Height = 100;
                x.Width = 200;
                canvas.Children.Add(x);
                x.SetValue(Canvas.LeftProperty, 620.0);
            }
            Button back = Make_Button("Back", 425.0, Title_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }

        private void Easy_Screen(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            Button back = Make_Button("Back", 425.0, Title_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }

        private void Medium_Screen(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            Button back = Make_Button("Back", 425.0, Title_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }

        private void Hard_Screen(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            Button back = Make_Button("Back", 425.0, Title_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }

        private void Multi_Screen(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            Button back = Make_Button("Back", 425.0, Title_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }

        private void Help_Screen(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            List<Button> btnList_help = new List<Button>();

            Button progression = Make_Button("Progression", 100, Progression_Screen);
            Button player = Make_Button("Player/Controls", 200, Player_Screen);
            Button enemies = Make_Button("Enemies", 300, Enemies_Screen);
            Button scoring = Make_Button("Scoring", 400, Scoring_Screen);


            btnList_help.Add(progression);
            btnList_help.Add(player);
            btnList_help.Add(enemies);
            btnList_help.Add(scoring);


            foreach (Button x in btnList_help)
            {
                x.Height = 100;
                x.Width = 200;
                canvas.Children.Add(x);
                x.SetValue(Canvas.LeftProperty, 620.0);
            }

            Button back = Make_Button("Back", 525.0, Title_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }

        private void About_Screen(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            TextBlock about = new TextBlock();
            about.Height = 300;
            about.Width = 600;
            about.Text += "\n\n";
            about.Text += "\tCreated by: Clayton Cockrell (Enemies), JD Efting (Serialization),\n\tJacob Franklin (Player Control), and Sandeep Kattepogu (High Scores)\n";
            about.Text += "\t============================================================================================================================================\n";
            about.Text += "\tWelcome to Joust. On earth in the year 2050, the evil dictator Mik Gnoj Nu of North Aerok carried out\n\ta mass nuclear-holocaust of the planet earth, leaving nature to crumble. With lava pits opening up\n\teverywhere and reanimated dinosaurs now ruling the planet with their radiation-enhanced\n\tintellect and power, you and your mutated flying ostrich Ralph travel the hellscape searching\n\tand saving the last vestiges of humanity.\n\n";
            about.Text += "\tHowever, the evil Un still has his evil lackeys (all named Mik) flying about on their evil mutated\n\tbuzzards trying to subjugate all survivors with pterodactyls making their day even worse. As\n\tthe hero of our story, it's your duty to end North Aerok's reign of terror by traversing\n\tdanger-fraught levels, each with more enemies than the last all the while breaking the dropped\n\teggs before they hatch into more trouble.\n\n\tCan you bring earth back from this radical nightmare?\n";
            canvas.Children.Add(about);

            Button back = Make_Button("Back", 525.0, Title_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }

        private void HighScores_Screen(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            TextBlock about = new TextBlock();
            about.Height = 300;
            about.Width = 600;
            about.Text += "\n\n";
            foreach (Score s in HighScoreManager.Instance.AllScores)
            {
                about.Text += "\t" + s.Serialize() + "\n";
            }
            canvas.Children.Add(about);

            Button back = Make_Button("Back", 525.0, Title_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }

        private void Player_Screen(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            TextBlock about = new TextBlock();
            about.Height = 300;
            about.Width = 600;
            about.Text += "\n\n\n";
            about.Text += "\tEach player controls a knight riding an ostrich. The knight holds a lance above the bird's head.\n\tThe player moves left by pressing the A or Left Arrow key. The player moves right by pressing\n\tthe D or Right Arrow key. If a player moves through the left or right side of the screen, they will\n\tpass through to the opposite side of the screen. If the player presses the W or Up Arrow key,\n\tthe ostrich will flap its wings and gain some height. The rate at which the player flaps the\n\tostrich's wings will control how fast the ostrich ascends or descends.\n";
            canvas.Children.Add(about);

            string newpath = HighScoreManager.Instance.path;
            int indexPos = newpath.IndexOf("\\JoustGame");
            newpath = newpath.Substring(0, indexPos);
            newpath += "\\Images\\player.png";

            BitmapImage playerpng = new BitmapImage(new Uri(newpath, UriKind.RelativeOrAbsolute));
            var image = new Image();
            image.Height = 50;
            image.Width = 50;
            image.Source = playerpng;
            canvas.Children.Add(image);

            Button back = Make_Button("Back", 525.0, Help_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }

        private void Progression_Screen(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            TextBlock about = new TextBlock();
            about.Height = 300;
            about.Width = 600;
            about.Text += "\n";
            about.Text += "\tWhen two birds collide with each other, the bird whose lance or beak is higher than the other\n\twill win the joust. If two birds collide with lances at an equal height, they will bounce off of each\n\tother. If a bird hits the side or bottom of a platform, it will bounce off of it.";
            about.Text += "\n\n";
            about.Text += "\tOnce all enemies are defeated, the top three scores will be shown before the next stage starts\n\tif more than one player is connected. Players start with three lives. When all players but one\n\thave run out of lives, the winner will be declared. The winner can keep playing or leave the\n\tgame. As the stages progress, more buzzards and pterodactyls will spawn.";
            canvas.Children.Add(about);

            Button back = Make_Button("Back", 525.0, Help_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }

        private void Enemies_Screen(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            List<Button> btnList = new List<Button>();

            Button buzzard = Make_Button("Buzzards", 100, Buzzard_Screen);
            Button egg = Make_Button("Eggs", 200, Egg_Screen);
            Button ptero = Make_Button("Pterodactyls", 300, Pterodactyl_Screen);

            btnList.Add(buzzard);
            btnList.Add(egg);
            btnList.Add(ptero);

            foreach (Button x in btnList)
            {
                x.Height = 100;
                x.Width = 200;
                canvas.Children.Add(x);
                x.SetValue(Canvas.LeftProperty, 620.0);
            }

            Button back = Make_Button("Back", 425.0, Help_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }

        private void Buzzard_Screen(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            TextBlock about = new TextBlock();
            about.Height = 300;
            about.Width = 600;
            about.Text += "\n\n\n";
            about.Text += "\tBuzzards are the basic enemy type, and they have the same range of movement as the player.\n\tBuzzards have riders and lances.\n";
            canvas.Children.Add(about);

            string newpath = HighScoreManager.Instance.path;
            int indexPos = newpath.IndexOf("\\JoustGame");
            newpath = newpath.Substring(0, indexPos);
            newpath += "\\Images\\buzzard.png";

            BitmapImage playerpng = new BitmapImage(new Uri(newpath, UriKind.RelativeOrAbsolute));
            var image = new Image();
            image.Height = 50;
            image.Width = 50;
            image.Source = playerpng;
            canvas.Children.Add(image);

            Button back = Make_Button("Back", 425.0, Enemies_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }

        private void Egg_Screen(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            TextBlock about = new TextBlock();
            about.Height = 300;
            about.Width = 600;
            about.Text += "\n\n\n";
            about.Text += "\tIf a buzzard loses a joust, the buzzard will die and an egg will fall from the buzzard's last\n\tlocation with the buzzard's last speed and direction. If it lands in the lava, the egg is destroyed.\n\tIf it lands on a platform, the egg will eventually hatch into a new rider. A new buzzard will fly in\n\tfor the rider to mount. Players can collect eggs by touching them, which will prevent them from\n\thatching.\n";
            canvas.Children.Add(about);

            string newpath = HighScoreManager.Instance.path;
            int indexPos = newpath.IndexOf("\\JoustGame");
            newpath = newpath.Substring(0, indexPos);
            newpath += "\\Images\\egg.png";

            BitmapImage playerpng = new BitmapImage(new Uri(newpath, UriKind.RelativeOrAbsolute));
            var image = new Image();
            image.Height = 50;
            image.Width = 50;
            image.Source = playerpng;
            canvas.Children.Add(image);

            Button back = Make_Button("Back", 425.0, Enemies_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }

        private void Pterodactyl_Screen(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            TextBlock about = new TextBlock();
            about.Height = 300;
            about.Width = 600;
            about.Text += "\n\n\n";
            about.Text += "\tPterodactyls move faster than buzzards, and their beak is used as a lance.\n";
            canvas.Children.Add(about);

            string newpath = HighScoreManager.Instance.path;
            int indexPos = newpath.IndexOf("\\JoustGame");
            newpath = newpath.Substring(0, indexPos);
            newpath += "\\Images\\pterodactyl.png";

            BitmapImage playerpng = new BitmapImage(new Uri(newpath, UriKind.RelativeOrAbsolute));
            var image = new Image();
            image.Height = 50;
            image.Width = 50;
            image.Source = playerpng;
            canvas.Children.Add(image);

            Button back = Make_Button("Back", 425.0, Enemies_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }

        private void Scoring_Screen(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            TextBlock about = new TextBlock();
            about.Height = 300;
            about.Width = 600;
            about.Text += "\n";
            about.Text += "\tEach player can see his points at the bottom of the screen. Collecting an egg is worth 250\n\tpoints. Killing a buzzard is worth 500 points. Killing a pterodactyl is worth 1000 points.\n\tKilling another player is worth 750 points. Players who survive a stage receive 200 points. Players gain\n\tone life every time they earn 10000 points.\n";
            canvas.Children.Add(about);

            Button back = Make_Button("Back", 425.0, Help_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }
    }
}