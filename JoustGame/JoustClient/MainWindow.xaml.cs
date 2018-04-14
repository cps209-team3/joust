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
        public int currentEndScore = 199991; // must set this when game end conditions have been met
        public TextBox namebox = new TextBox();
        public DispatcherTimer updateTimer;
        public StateMachine playerStateMachine;
        public bool flapLock;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // This is only here for faster testing
            // If you need a different screen on window load, comment out the line below
            //NewGame();

            Title_Screen(null, EventArgs.Empty);
        }
        
        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    //show escape menu
                    break;
                case Key.W:
                case Key.Up:
                    if (!flapLock)
                    {
                        Task.Run(() => playerStateMachine.HandleInput("flap"));
                    }
                    flapLock = true;
                    Task.Run(() =>
                    {
                        Thread.Sleep(100);
                        flapLock = false;
                    });
                    break;
                case Key.A:
                case Key.Left:
                    control.WorldRef.player.leftDown = false;
                    break;
                case Key.D:
                case Key.Right:
                    control.WorldRef.player.rightDown = false;
                    break;
                default:
                    break;
            }
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    // display escape menu here
                    break;
                case Key.A:
                case Key.Left:
                    control.WorldRef.player.leftDown = true;
                    break;
                case Key.D:
                case Key.Right:
                    control.WorldRef.player.rightDown = true;
                    break;
                default:
                    break;
            }
        }

        public void WorldObjectControlFactory(WorldObject worldObject)
        {
            string woString = worldObject.ToString();
            Image i;
            switch (woString)
            {
                case "Ostrich":
                    Ostrich o = worldObject as Ostrich;
                    i = new OstrichControl(o.imagePath);
                    OstrichControl oC = i as OstrichControl;
                    o.ostrichMoved += oC.NotifyMoved;
                    break;
                case "Buzzard":
                    Buzzard b = worldObject as Buzzard;
                    i = new BuzzardControl(b.imagePath);
                    BuzzardControl bC = i as BuzzardControl;

                    // Used to update the view with model updates
                    b.BuzzardMoveEvent += bC.NotifyMoved;
                    b.BuzzardStateChange += bC.NotifyState;
                    b.BuzzardDropEgg += bC.NotifyDrop;
                    b.BuzzardDestroyed += bC.NotifyDestroy;
                    // Used to update all enemies in the world
                    //DispatcherTimer moveTimer = new DispatcherTimer();
                    //moveTimer.Interval = new TimeSpan(0, 0, 0, 0, 33);
                    //moveTimer.Tick += World.Instance.UpdateAllEnemies_Position;
                    //moveTimer.Start();

                    /*  Comment:    Clayton Cockrell
                     *  The Random object in Buzzard would give the same random number to all the 
                     *  Buzzard objects if their creation was not halted for a little bit of time.
                     */
                    Thread.Sleep(20);
                    break;
                case "Pterodactyl":
                    PterodactylControl pCtrl = new PterodactylControl("Images/Enemy/pterodactyl.fly1");
                    i = pCtrl;

                    /*  Comment:    Clayton Cockrell
                     *  Pterodactyls spawn after a certain number of minutes. I currently have it set
                     *  to 1 minute. (To change, see PTERODACTYL_SPAWN_MINUTES constant in World class)
                     */
                    World.Instance.SpawnPterodactyl += pCtrl.NotifySpawn;
                    break;
                case "Egg":
                    Egg e = worldObject as Egg;
                    i = new EggControl(e.imagePath);
                    EggControl eC = i as EggControl;
                    break;
                case "Platform":
                    Platform pl = worldObject as Platform;
                    i = new PlatformControl(pl.imagePath);
                    PlatformControl pC = i as PlatformControl;
                    break;
                case "Respawn":
                    Respawn r = worldObject as Respawn;
                    i = new RespawnControl(r.imagePath);
                    RespawnControl rC = i as RespawnControl;
                    break;
                default:
                    Base ba = worldObject as Base;
                    i = new BaseControl(ba.imagePath);
                    BaseControl baC = i as BaseControl;
                    break;
            }
            canvas.Children.Add(i);
            Canvas.SetTop(i, worldObject.coords.y);
            Canvas.SetLeft(i, worldObject.coords.x);

            //Title_Screen(null, EventArgs.Empty);
            //Finish_HighScores(null, EventArgs.Empty);
            // called when game end conditions have been met
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    // display escape menu here
                    break;
                case Key.W:
                case Key.Up:
                    Task.Run(() => playerStateMachine.HandleInput("flap"));
                    break;
                case Key.A:
                case Key.Left:
                    Task.Run(() => playerStateMachine.HandleInput("left"));
                    break;
                case Key.D:
                case Key.Right:
                    Task.Run(() => playerStateMachine.HandleInput("right"));
                    break;
                default:
                    break;
            }
        }

        public void SaveGame(object sender, RoutedEventArgs e)
        {
            control.Save();
        }
        public void LoadGame(object sender, RoutedEventArgs e)
        {
            string fileName = "";
            foreach (UIElement element in canvas.Children)
            {
                if ((element as FrameworkElement).Name == "LoadName")
                {
                    fileName = (element as TextBox).Text;
                }
            }
            control.WorldRef.objects.Clear();
            
            control.Load(fileName);

            // refresh method

            canvas.Children.Clear();
            StartGameScreen(sender, e);
            foreach (WorldObject obj in control.WorldRef.objects)
            {
                // set player
                if (obj.ToString() == "Ostrich")
                {
                    control.WorldRef.player = (obj as Ostrich);
                    playerStateMachine = control.WorldRef.player.stateMachine;
                    Console.WriteLine("player has been set");
                }
                WorldObjectControlFactory(obj);
            }
            // end refresh method
        }

        public void NewGame(object sender, EventArgs e)
        {
            StartGameScreen(sender, e);

            // Load Map here

            // Get stage num from controls once the proper screens are implemented
            Ostrich o = InitiateWorldObject("Ostrich", 720, 450) as Ostrich;
            control.WorldRef.player = o;
            playerStateMachine = control.WorldRef.player.stateMachine;

            /*  Comment:    Clayton Cockrell
             *  Pterodactyls start spawning at stage 5. stage is set this for testing
             *  purposes.
             */

            int stage = 0;
            control.WorldRef.stage = stage;
            int numBuzzards = 0;
            int numPterodactyls = 0;
            control.CalculateNumEnemies(control.WorldRef.stage, ref numBuzzards, ref numPterodactyls);

            InitiateWorldObject("Platform", 100, 300);
            InitiateWorldObject("Platform", 700, 500);
            InitiateWorldObject("Platform", 500, 300);
            InitiateWorldObject("Platform", 950, 200);
            InitiateWorldObject("Respawn", 700, 100);
            InitiateWorldObject("Respawn", 1100, 600);
            InitiateWorldObject("Respawn", 200, 600);
            InitiateWorldObject("Base", 375, 775);

            for (int i = 0; i < numBuzzards; i++)
            {
                InitiateWorldObject("Buzzard", 100, 300);
            }

            for (int i = 0; i < numPterodactyls; i++)
            {
                InitiateWorldObject("Pterodactyl", 300, 300);
            }

            updateTimer = new DispatcherTimer(
                TimeSpan.FromMilliseconds(5), 
                DispatcherPriority.Render,
                UpdateTick,
                Dispatcher.CurrentDispatcher);
            updateTimer.Start();
        }

        public WorldObject InitiateWorldObject(string type, double x, double y)
        {
            WorldObject obj = control.CreateWorldObj(type);
            obj.coords.x = x;
            obj.coords.y = y;
            WorldObjectControlFactory(obj);
            return obj;
        }

        public void UpdateTick(object sender, EventArgs e)
        {
            control.Update();
        }
       
       // Title screens
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

        private void StartGameScreen(object sender, EventArgs e)
        {
            canvas.Children.Clear();
            canvas.Background = Brushes.Black;
            Button saveBtn = new Button();
            saveBtn.Content = "Save";
            saveBtn.Click += new RoutedEventHandler(SaveGame);
            canvas.Children.Add(saveBtn);

            TextBox loadName = new TextBox();
            loadName.Name = "LoadName";
            loadName.Text = "file2load";
            loadName.Margin = new Thickness(0, 20, 0, 0);
            canvas.Children.Add(loadName);

            Button loadBtn = new Button();
            loadBtn.Content = "Load";
            loadBtn.Click += new RoutedEventHandler(LoadGame);
            loadBtn.Margin = new Thickness(0, 40, 0, 0);
            canvas.Children.Add(loadBtn);
        }

        private void Title_Screen(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Spawn();
            });

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
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            List<Button> btnList = new List<Button>();

            Button easy = Make_Button("Easy", 100.0, Easy_Screen);
            Button medium = Make_Button("Medium", 200.0, Medium_Screen);
            Button hard = Make_Button("Hard", 300.0, Hard_Screen);

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
       
        private void Finish_HighScores(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            namebox.Height = 50;
            namebox.Width = 400;
            Canvas.SetLeft(namebox, 520);
            Canvas.SetTop(namebox, 280);
            namebox.Text = "[Name]";
            namebox.MaxLength = 15;
            canvas.Children.Add(namebox);

            List<Button> btnList = new List<Button>();

            Button yes = Make_Button("Save score", 300.0, Accept_SaveScore);
            Button no = Make_Button("Don't save score", 300.0, Title_Screen);

            btnList.Add(yes);
            btnList.Add(no);

            foreach (Button x in btnList)
            {
                x.Height = 50;
                x.Width = 200;
                canvas.Children.Add(x);
            }

            yes.SetValue(Canvas.LeftProperty, 520.0);
            no.SetValue(Canvas.LeftProperty, 720.0);
        }

        private void Accept_SaveScore(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            TextBlock scoreBlock = new TextBlock();
            scoreBlock.Height = 500;
            scoreBlock.Width = 400;
            Canvas.SetLeft(scoreBlock, 620);
            Canvas.SetTop(scoreBlock, 280);
            canvas.Children.Add(scoreBlock);

            // FIX
            Score thisScore = new Score(currentEndScore, namebox.Text);
            HighScoreManager.Instance.AddScore(thisScore);

            foreach (Score s in HighScoreManager.Instance.AllScores)
            {
                scoreBlock.Text += s.Serialize() + "\n";
            }

            Button back = Make_Button("Title screen", 625.0, Title_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }

        private void Easy_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            Button back = Make_Button("Back", 425.0, Single_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);

            Button game = Make_Button("Start Game", 200.0, NewGame);
            game.SetValue(Canvas.LeftProperty, 620.0);
            game.Height = 100;
            game.Width = 200;
            canvas.Children.Add(game);

            /*
            Button back = Make_Button("Back", 425.0, Single_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);*/
        }

        private void Medium_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            Button back = Make_Button("Back", 425.0, Single_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);

            Button game = Make_Button("Start Game", 200.0, NewGame);
            game.SetValue(Canvas.LeftProperty, 620.0);
            game.Height = 100;
            game.Width = 200;
            canvas.Children.Add(game);
        }

        private void Hard_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            Button back = Make_Button("Back", 425.0, Single_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);

            Button game = Make_Button("Start Game", 200.0, NewGame);
            game.SetValue(Canvas.LeftProperty, 620.0);
            game.Height = 100;
            game.Width = 200;
            canvas.Children.Add(game);
        }

        private void Multi_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            Button back = Make_Button("Back", 425.0, Title_Screen);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            canvas.Children.Add(back);
        }

        private void Help_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

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
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

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
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            TextBlock about = new TextBlock();
            about.Height = 300;
            about.Width = 600;
            about.Text += "\n\n";
            foreach (Score s in HighScoreManager.Instance.AllScores)
            {
                // FIX
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
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            TextBlock about = new TextBlock();
            about.Height = 300;
            about.Width = 600;
            about.Text += "\n\n\n";
            about.Text += "\tEach player controls a knight riding an ostrich. The knight holds a lance above the bird's head.\n\tThe player moves left by pressing the A or Left Arrow key. The player moves right by pressing\n\tthe D or Right Arrow key. If a player moves through the left or right side of the screen, they will\n\tpass through to the opposite side of the screen. If the player presses the W or Up Arrow key,\n\tthe ostrich will flap its wings and gain some height. The rate at which the player flaps the\n\tostrich's wings will control how fast the ostrich ascends or descends.\n";
            canvas.Children.Add(about);

            // FIX
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
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

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
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

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
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

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
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

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
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

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
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

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