using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using JoustModel;
using System.Diagnostics;
using System.IO;

namespace JoustClient
{
    public enum SpaceType
    {
        Spawn,
        Plat,
        Blank,
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GameController control = new GameController();
        public TextBox namebox = new TextBox();
        public DispatcherTimer updateTimer;
        public StateMachine playerStateMachine;
        public bool flapLock;
        public bool cheatMode = false;
        public bool controls_on = false;
        public TextBox diff = new TextBox();
        TextBlock Announce;

        public string tester;

        private System.Windows.Point mousePosition;
        private bool dragging = false;
        private Image currImg;
        private WorldObjectControl currPlat;
        private Button ex2;
        private TextBlock errorBlock;
        private int ids = 0;
        private bool designer_on = false;
        private bool isLastCheck = false;


        // This makes flying create fewer threads
        // to change the animation which makes the
        // game run better
        private OstrichControl ostrichCtrl;

        

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
            Announce = new TextBlock();
            Canvas.SetTop(Announce, 425);
            Canvas.SetLeft(Announce, 550);
            Canvas.SetLeft(Announce, 790);
            Announce.HorizontalAlignment = HorizontalAlignment.Center;
            Announce.VerticalAlignment = VerticalAlignment.Center;
            Announce.FontSize = 32;
            Announce.Height = 50;
            Announce.Foreground = new SolidColorBrush(Colors.White);
            //Finish_HighScores(null, EventArgs.Empty);
            //Designer_Screen(null, EventArgs.Empty);
        }
        
        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            // wrapped key event in check to prevent crashes on menus

            if (controls_on)
            {
                switch (e.Key)
                {
                    case Key.Escape:
                        //show escape menu
                        break;
                    case Key.W:
                    case Key.Up:
                        if (playerStateMachine.Current is DeadState || playerStateMachine.Current is SpawnState) { }
                        else {
                            if (!flapLock) {
                                Task.Run(() => playerStateMachine.HandleInput("flap"));
                            }
                            flapLock = true;
                            Task.Run(() => {
                                PlaySounds.Instance.Play_Flap();
                                Dispatcher.Invoke(() => ostrichCtrl.Source = new BitmapImage(new Uri("Sprites/player_fly2.png", UriKind.Relative)));
                                Thread.Sleep(100);
                                Dispatcher.Invoke(() => ostrichCtrl.Source = new BitmapImage(new Uri("Sprites/player_fly1.png", UriKind.Relative)));
                                flapLock = false;
                            });
                        }
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
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            // wrapped key event in check to prevent crashes on menus

            if (controls_on)
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
        }

        public void WorldObjectControlFactory(WorldObject worldObject)
        {
            string woString = worldObject.ToString();
            WorldObjectControl i;
            switch (woString)
            {
                case "Ostrich":
                    Ostrich o = worldObject as Ostrich;
                    i = new OstrichControl(o.imagePath);
                    ostrichCtrl = i as OstrichControl;
                    o.ostrichMoved += ostrichCtrl.NotifyMoved;
                    break;
                case "Buzzard":
                    Buzzard b = worldObject as Buzzard;
                    Console.WriteLine("Spawning Buzzard");
                    i = new BuzzardControl(b.imagePath);
                    BuzzardControl bC = i as BuzzardControl;

                    // Used to update the view with model updates
                    b.BuzzardMoveEvent += bC.NotifyMoved;
                    b.BuzzardStateChange += bC.NotifyState;
                    b.BuzzardDropEgg += bC.NotifyDrop;
                    b.BuzzardDestroyed += bC.NotifyDestroy;

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
                    pC.Resize(pl.width, pl.height);
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

        public void NotifyWon(object sender, int e)
        {
            int stage = control.WorldRef.stage;
            if (stage == 99)
            {
                stage = 0;
            }
            else
            {
                stage += 1;
            }
            control.WorldRef.stage = stage;
            control.GetSpawnPoints();
            Announce.Text = "WAVE CLEARED!";
            canvas.Children.Add(Announce);

            Task.Run(() =>
            {
                Thread.Sleep(1000);
                Dispatcher.Invoke(() => Announce.Text = "3");
                Thread.Sleep(1000);
                Dispatcher.Invoke(() => Announce.Text = "2");
                Thread.Sleep(1000);
                Dispatcher.Invoke(() => Announce.Text = "1");
                Thread.Sleep(1000);
                Dispatcher.Invoke(() => Announce.Text = String.Format("WAVE {0}", Convert.ToString(stage)));
                Thread.Sleep(1000);
                SpawnEnemies();
                Dispatcher.Invoke(() =>
                {
                    Announce.Text = "";
                    canvas.Children.Remove(Announce);
                });
            });
        }

        public void NotifyLost(object sender, int e)
        {
            controls_on = false;
            updateTimer.Stop();
            control.WorldRef.objects.Clear();
            control.WorldRef.basePlatform = null;
            control.WorldRef.stage = 0;
            Announce.Text = "GAME OVER";
            canvas.Children.Add(Announce);
            control.WorldRef.player.ostrichDied -= this.NotifyLost;
            control.WorldRef.player = null;
            Task.Run(() =>
            {
                Thread.Sleep(3000);
                Dispatcher.Invoke(() => HighScores_Screen(sender, new RoutedEventArgs()));
            });
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
            // switched bool to activate controls
            controls_on = true;
            
            control.WorldRef.win += this.NotifyWon;
            flapLock = false;
            canvas.Children.Clear();
            canvas.Background = Brushes.Black;
            
            StartGameScreen(sender, e);

            // Load Map here

            // Get stage num from controls once the proper screens are implemented
            Ostrich o = InitiateWorldObject("Ostrich", 720, 350) as Ostrich;
            control.WorldRef.player = o;
            control.WorldRef.player.ostrichDied += this.NotifyLost;
            playerStateMachine = control.WorldRef.player.stateMachine;
            if (cheatMode)
            {
                o.cheatMode = true;
            }

            // difficulty setting
            int difficulty = 0;
            bool result = Int32.TryParse(diff.Text, out difficulty);
            if (difficulty < 0)
            {
                difficulty = 0;
            }
            control.WorldRef.stage = difficulty;

            InitiateWorldObject("Platform", 100, 300);
            InitiateWorldObject("Platform", 700, 500);
            InitiateWorldObject("Platform", 500, 300);
            InitiateWorldObject("Platform", 950, 200);
            InitiateWorldObject("Respawn", 700, 100);
            InitiateWorldObject("Respawn", 1100, 600);
            InitiateWorldObject("Respawn", 200, 600);
            InitiateWorldObject("Base", 375, 775);

            control.GetSpawnPoints();

            SpawnEnemies();

            updateTimer = new DispatcherTimer(
                TimeSpan.FromMilliseconds(5), 
                DispatcherPriority.Render,
                UpdateTick,
                Dispatcher.CurrentDispatcher);
            updateTimer.Start();
        }

        public void CustomStage_Screen(object sender, EventArgs e) {
            canvas.Children.Clear();
            canvas.Background = Brushes.Black;
            // Get the save files in the Custom Stages directory
            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo("../../Saves/Custom Stages/");
            System.IO.FileInfo[] files = info.GetFiles();
            ListBox listv = new ListBox();
            listv.SetValue(Canvas.TopProperty, 100.0);
            listv.SetValue(Canvas.LeftProperty, 613.0);
            listv.Width = 233;
            listv.Height = 500;
            canvas.Children.Add(listv);
            for (int fileNum = 0; fileNum < files.Length; fileNum++)
            {
                Button b = Make_Button(files[fileNum].Name.Substring(0, (files[fileNum].Name.Length - 4)), (fileNum + 1) * 100, LoadStage);
                canvas.Children.Remove(b);
                listv.Items.Add(b);
            }
            Button back = Make_BackButton(625.0, Single_Screen);
        }

        public void LoadStage(object sender, EventArgs e) {
            canvas.Children.Clear();
            canvas.Background = Brushes.Black;
            Button b = sender as Button;
            control.StageLoad(b.Content + ".txt");
            // Make the controls for each world object
            foreach (WorldObject obj in control.WorldRef.objects) {
                WorldObjectControlFactory(obj);
            }

            controls_on = true;
            control.WorldRef.win += this.NotifyWon;

            Ostrich o = InitiateWorldObject("Ostrich", 720, 350) as Ostrich;
            control.WorldRef.player = o;
            playerStateMachine = control.WorldRef.player.stateMachine;
            control.WorldRef.player.ostrichDied += this.NotifyLost;
            if (cheatMode) {
                o.cheatMode = true;
            }

            // difficulty setting
            int difficulty = 0;
            bool result = Int32.TryParse(diff.Text, out difficulty);
            if (difficulty < 0) {
                difficulty = 0;
            }
            control.WorldRef.stage = difficulty;

            control.GetSpawnPoints();

            SpawnEnemies();
            InitiateWorldObject("Base", 375, 775);

            updateTimer = new DispatcherTimer(
                TimeSpan.FromMilliseconds(5),
                DispatcherPriority.Render,
                UpdateTick,
                Dispatcher.CurrentDispatcher);
            updateTimer.Start();
        }

        public void SpawnEnemies()
        {
            int numBuzzards = 0;
            int numPterodactyls = 0;
            control.CalculateNumEnemies(ref numBuzzards, ref numPterodactyls);

            for (int i = 0; i < numBuzzards; i++)
            {
                int spawnX = 0;
                int spawnY = 0;
                int randNum = new Random().Next(control.WorldRef.SpawnPoints.Count - 1);

                JoustModel.Point[] p = control.WorldRef.SpawnPoints[randNum];

                spawnX = (int)(((p[1].x - p[0].x) / 2) + (p[0].x - 10));
                spawnY = (int)p[0].y;

                Dispatcher.Invoke(() => InitiateWorldObject("Buzzard", spawnX, spawnY));
            }

            for (int i = 0; i < numPterodactyls; i++)
            {
                Dispatcher.Invoke(() => InitiateWorldObject("Pterodactyl", 300, 300));
            }
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

        private Button Make_Button(object content, double top, RoutedEventHandler eventx)
        {
            Button btnReturn = new Button();

            string contentString = content as string;
            btnReturn.Content = contentString.ToUpper();

            btnReturn.SetValue(Canvas.TopProperty, top);

            if (!(eventx == null))
            {
                btnReturn.Click += new RoutedEventHandler(eventx);
            }
            btnReturn.BorderBrush = Brushes.Red;
            btnReturn.Background = Brushes.Yellow;
            btnReturn.Foreground = Brushes.Green;
            btnReturn.FontSize = 20;
            btnReturn.FontFamily = new FontFamily("Century Gothic");
            btnReturn.FontWeight = FontWeights.Bold;
            btnReturn.Height = 100;
            btnReturn.Width = 200;
            btnReturn.SetValue(Canvas.LeftProperty, 620.0);

            canvas.Children.Add(btnReturn);

            return btnReturn;
        }

        private Button Make_BackButton(double top, RoutedEventHandler eventx)
        {
            Button back = Make_Button("Back", top, eventx);
            back.SetValue(Canvas.LeftProperty, 620.0);
            back.Height = 100;
            back.Width = 200;
            return back;
        }

        private TextBlock Make_TextBlock(double top, double left, int height, int width)
        {
            TextBlock newBlock = new TextBlock();
            newBlock.Height = height;
            newBlock.Width = width;
            Canvas.SetLeft(newBlock, left);
            Canvas.SetTop(newBlock, top);
            newBlock.Background = Brushes.Blue;
            newBlock.Foreground = Brushes.Yellow;
            newBlock.FontFamily = new FontFamily("Century Gothic");
            newBlock.FontSize = 25;

            canvas.Children.Add(newBlock);

            return newBlock;
        }

        private Image Make_Image(string path, double top, double left, int height, int width)
        {
            string newpath = HighScoreManager.Instance.path;
            int indexPos = newpath.IndexOf("\\JoustGame");
            newpath = newpath.Substring(0, indexPos);
            newpath += path;

            BitmapImage playerpng = new BitmapImage(new Uri(newpath, UriKind.RelativeOrAbsolute));
            var image = new Image();
            image.Height = height;
            image.Width = width;
            Canvas.SetTop(image, top);
            Canvas.SetLeft(image, left);
            image.Source = playerpng;

            canvas.Children.Add(image);

            return image;
        }

        private void Title_Screen(object sender, EventArgs e)
        {
            controls_on = false;
            designer_on = true;
            canvas.Children.Clear();
            canvas.Background = Brushes.Black;

            Button startSingle = Make_Button("Single Player", 200.0, Single_Screen);
            Button startMulti = Make_Button("Mulitplayer", 300.0, Multi_Screen);
            Button help = Make_Button("Help", 400.0, Help_Screen);
            Button about = Make_Button("About", 500.0, About_Screen);
            Button scores = Make_Button("High Scores", 600.0, HighScores_Screen);
            Button designer = Make_Button("Level Designer", 700.0, Designer_Screen);

            Image image = Make_Image("\\Images\\joust2.png", 25.0, 510.0, 150, 400);

            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Spawn();
            });
        }

        private void Single_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            Button back = Make_BackButton(625.0, Title_Screen);

            Button game = Make_Button("Start Game", 100.0, NewGame);

            Button customStage = Make_Button("Custom Stage", 200.0, CustomStage_Screen);

            Button cheat = Make_Button("CHEAT OFF", 350.0, Cheat_Toggle);
            cheat.Height = 50;
            cheatMode = false;


            TextBlock descrip = Make_TextBlock(425, 620, 50, 200);
            descrip.Text = "Set stage:{0-99}";
            descrip.Height = 40;
            descrip.Width = 200;
            descrip.TextAlignment = TextAlignment.Center;

            Canvas.SetLeft(diff, 620);
            Canvas.SetTop(diff, 475);
            diff.Height = 50;
            diff.Width = 200;
            diff.Text = "0";
            diff.FontSize = 40;
            diff.TextAlignment = TextAlignment.Center;
            diff.FontFamily = new FontFamily("Century Gothic");
            diff.BorderBrush = Brushes.Red;
            diff.MaxLength = 2;
            canvas.Children.Add(diff);
        }

        private void Cheat_Toggle(object sender, RoutedEventArgs e)
        {
            Button sent = sender as Button;

            if (cheatMode == true)
            {
                cheatMode = false;
                sent.Content = "CHEAT OFF";
                sent.Background = Brushes.Yellow;
            }
            else
            {
                cheatMode = true;
                sent.Content = "CHEAT ON";
                sent.Background = Brushes.Orange;
            }

        }

        private void Finish_HighScores(object sender, EventArgs e)
        {
            controls_on = false;

            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();
            canvas.Background = Brushes.Black;

            namebox.Height = 50;
            namebox.Width = 400;
            Canvas.SetLeft(namebox, 520);
            Canvas.SetTop(namebox, 272);
            namebox.Text = "[Name]";
            namebox.FontSize = 18;
            namebox.FontFamily = new FontFamily("Century Gothic");
            namebox.BorderBrush = Brushes.Red;
            namebox.MaxLength = 10;
            canvas.Children.Add(namebox);

            Button yes = Make_Button("Save score", 300.0, Accept_SaveScore);
            Button no = Make_Button("Don't save score", 300.0, Title_Screen);

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

            TextBlock scoreBlock = Make_TextBlock(150, 470, 420, 500);

            Score thisScore;
            try
            {
                thisScore = new Score(control.WorldRef.player.score, namebox.Text);
            }
            catch
            {
                // for test
                control.WorldRef.player = new Ostrich();
                control.WorldRef.player.score = 100;
                thisScore = new Score(control.WorldRef.player.score, namebox.Text);
            }
            HighScoreManager.Instance.AddScore(thisScore);

            scoreBlock.Text += "\n";
            scoreBlock.FontFamily = new FontFamily("Courier New");
            int number = 1;
            foreach (Score s in HighScoreManager.Instance.AllScores)
            {
                string space = "";
                for (int x = 0; x < (12 - s.username.Length); x++)
                {
                    space += " ";
                }

                if (number != 10)
                {
                    scoreBlock.Text += "    " + number.ToString() + ":\t" + s.username + "," + space + s.points.ToString() + "\n";
                    number++;
                }
                else
                {
                    scoreBlock.Text += "   " + number.ToString() + ":\t" + s.username + "," + space + s.points.ToString() + "\n";
                    break;
                }
            }

            Button back = Make_BackButton(625.0, Title_Screen);
        }

        private void Multi_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            Button back = Make_BackButton(625.0, Title_Screen);
        }

        private void Help_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            Button progression = Make_Button("Progression", 200, Progression_Screen);
            Button player = Make_Button("Player/Controls", 300, Player_Screen);
            Button enemies = Make_Button("Enemies", 400, Enemies_Screen);
            Button scoring = Make_Button("Scoring", 500, Scoring_Screen);

            Button back = Make_BackButton(625.0, Title_Screen);
        }

        private void About_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            TextBlock about = Make_TextBlock(50, 220, 450, 1000);
            about.Text += "\n\n\tCreated by: Clayton Cockrell (Enemies), JD Efting (Serialization),\n\tJacob Franklin " +
                "(Player Control), and Sandeep Kattepogu (High Scores)\n\t===============================================" +
                "===================================\n\tWelcome " +
                "to Joust. On earth in the year 2050, the evil dictator Mik Gnoj Nu of North Aerok carried out\n\ta mass " +
                "nuclear-holocaust of the planet earth, leaving nature to crumble. With lava pits opening up\n\teverywhere and" +
                " reanimated dinosaurs now ruling the planet with their radiation-enhanced\n\tintellect and power, you and your" +
                " mutated flying ostrich Ralph travel the hellscape searching\n\tand saving the last vestiges of humanity.\n\n\tHowever," +
                " the evil Un still has his evil lackeys (all named Mik) flying about on their evil mutated\n\tbuzzards trying to subjugate" +
                " all survivors with pterodactyls making their day even worse. As\n\tthe hero of our story, it's your duty to end North Aerok's " +
                "reign of terror by traversing\n\tdanger-fraught levels, each with more enemies than the last all the while breaking the dropped\n\t" +
                "eggs before they hatch into more trouble.\n\n\tCan you bring earth back from this radical nightmare?\n";
            about.FontSize = 15;

            Button back = Make_BackButton(625.0, Title_Screen);
        }

        private void HighScores_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            TextBlock scoreBlock = Make_TextBlock(150, 470, 420, 500);

            scoreBlock.Text += "\n";
            scoreBlock.FontFamily = new FontFamily("Courier New");
            int number = 1;
            foreach (Score s in HighScoreManager.Instance.AllScores)
            {
                string space = "";
                for (int x = 0; x < (12 - s.username.Length); x++)
                {
                    space += " ";
                }

                if (number != 10)
                {
                    scoreBlock.Text += "    " + number.ToString() + ":\t" + s.username + "," + space + s.points.ToString() + "\n";
                    number++;
                }
                else
                {
                    scoreBlock.Text += "   " + number.ToString() + ":\t" + s.username + "," + space + s.points.ToString() + "\n";
                    break;
                }
            }

            Button back = Make_BackButton(625.0, Title_Screen);
        }

        private void Player_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            TextBlock player = Make_TextBlock(50, 220, 450, 1000);
            player.Text += "\n\n\n\tEach player controls a knight riding an ostrich. The" +
                " knight holds a lance above the bird's head.\n\tThe player moves left by pressing " +
                "the A or Left Arrow key. The player moves right by pressing\n\tthe D or Right Arrow " +
                "key. If a player moves through the left or right side of the screen, they will\n\tpass " +
                "through to the opposite side of the screen. If the player presses the W or Up Arrow key,\n" +
                "\tthe ostrich will flap its wings and gain some height. The rate at which the player flaps " +
                "the\n\tostrich's wings will control how fast the ostrich ascends or descends.\n";
            player.FontSize = 18;

            Image image = Make_Image("//Images//player.png", 40, 100, 100, 100);

            Button back = Make_BackButton(625.0, Help_Screen);
        }

        private void Progression_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            TextBlock progression = Make_TextBlock(50, 220, 450, 1000);
            progression.FontSize = 18;
            progression.Text += "\n\n\tWhen two birds collide with each other, the " +
                "bird whose lance or beak is higher than the other\n\twill win the " +
                "joust. If two birds collide with lances at an equal height, they will" +
                " bounce off of each\n\tother. If a bird hits the side or bottom of a " +
                "platform, it will bounce off of it.\n\n\tOnce all enemies are defeated, " +
                "the top three scores will be shown before the next stage starts\n\tif more " +
                "than one player is connected. Players start with three lives. When all players" +
                " but one\n\thave run out of lives, the winner will be declared. The winner can" +
                " keep playing or leave the\n\tgame. As the stages progress, more buzzards and" +
                " pterodactyls will spawn.";

            Button back = Make_BackButton(625.0, Help_Screen);
        }

        private void Enemies_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            Button buzzard = Make_Button("Buzzards", 200, Buzzard_Screen);
            Button egg = Make_Button("Eggs", 300, Egg_Screen);
            Button ptero = Make_Button("Pterodactyls", 400, Pterodactyl_Screen);

            Button back = Make_BackButton(625.0, Help_Screen);
        }

        private void Buzzard_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            TextBlock buzzard = Make_TextBlock(50, 220, 450, 1000);
            buzzard.FontSize = 18;
            buzzard.Text += "\n\n\n\tBuzzards are the basic enemy type, and they have the same range of movement as the player.\n\tBuzzards have riders and lances.\n";

            Image image = Make_Image("//Images//buzzard.png", 40, 100, 100, 100);

            Button back = Make_BackButton(625.0, Enemies_Screen);
        }

        private void Egg_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            TextBlock egg = Make_TextBlock(50, 220, 450, 1000);
            egg.FontSize = 18;
            egg.Text += "\n\n\n\tIf a buzzard loses a joust, the buzzard will die and " +
                "an egg will fall from the buzzard's last\n\tlocation with the buzzard's " +
                "last speed and direction. If it lands in the lava, the egg is destroyed.\n\tIf " +
                "it lands on a platform, the egg will eventually hatch into a new rider. A new" +
                " buzzard will fly in\n\tfor the rider to mount. Players can collect eggs by " +
                "touching them, which will prevent them from\n\thatching.\n";

            Image image = Make_Image("//Images//egg.png", 50, 100, 100, 100);

            Button back = Make_BackButton(625.0, Enemies_Screen);
        }

        private void Pterodactyl_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            TextBlock ptero = Make_TextBlock(50, 220, 450, 1000);
            ptero.FontSize = 18;
            ptero.Text += "\n\n\n\tPterodactyls move faster than buzzards, and their beak is used as a lance.\n";

            Image image = Make_Image("//Images//pterodactyl.png", 18, 100, 100, 100);

            Button back = Make_BackButton(625.0, Enemies_Screen);
        }

        private void Scoring_Screen(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                PlaySounds.Instance.Play_Select();
            });

            canvas.Children.Clear();

            TextBlock scoring = Make_TextBlock(50, 220, 450, 1000);
            scoring.FontSize = 18;
            scoring.Text += "\n\n\n\tEach player can see his points at the bottom of the screen. " +
                "Collecting an egg is worth 250\n\tpoints. Killing a buzzard is worth 500 points. " +
                "Killing a pterodactyl is worth 1000 points.\n\tKilling another player is worth 750 " +
                "points. Players who survive a stage receive 200 points. Players gain\n\tone life " +
                "every time they earn 10000 points.\n";

            Button back = Make_BackButton(625.0, Help_Screen);
        }

        private void Designer_Screen(object sender, EventArgs e)
        {
            canvas.Children.Clear();

            designer_on = true;

            ids = 0;

            Image one = Make_Image("//Images//Sprites//platform_short1.png", 0, 0, 30, 200);
            Image two = Make_Image("//Images//Sprites//platform_respawn1.png", 0, 0, 30, 200);
            canvas.Children.Remove(one);
            canvas.Children.Remove(two);

            Button ex1 = Make_Button("PLATFORM", 0.0, plat_button);
            ex1.Height = 30;
            ex1.Width = 200;
            ex1.Background = Brushes.Orange;

            ex2 = Make_Button("SPAWN", 0.0, spawn_button);
            ex2.Height = 30;
            ex2.Width = 200;
            ex2.SetValue(Canvas.LeftProperty, 420.0);
            ex2.Background = Brushes.Orange;
            ex2.IsEnabled = false;

            namebox.Height = 30;
            namebox.Width = 200;
            Canvas.SetLeft(namebox, 820);
            Canvas.SetTop(namebox, 0);
            namebox.Text = "[Name]";
            namebox.FontSize = 18;
            namebox.FontFamily = new FontFamily("Century Gothic");
            namebox.BorderBrush = Brushes.Red;
            namebox.MaxLength = 10;
            canvas.Children.Add(namebox);

            Button save = Make_Button("Save", 0.0, Level_Save);
            save.SetValue(Canvas.LeftProperty, 1020.0);
            save.Width = 100;
            save.Height = 30;

            Button exit = Make_Button("Exit", 0.0, Title_Screen);
            exit.SetValue(Canvas.LeftProperty, 1120.0);
            exit.Width = 100;
            exit.Height = 30;

            Button delete = Make_Button("Delete", 0.0, Plat_Delete);
            delete.SetValue(Canvas.LeftProperty, 1220.0);
            delete.Width = 100;
            delete.Height = 30;

            errorBlock = Make_TextBlock(0.0, 220.0, 30, 200);

            TextBlock editArea = Make_TextBlock(30.0, 0.0, 730, 1414);
            editArea.Background = Brushes.Gray;

            TextBlock editAreaPlayer = Make_TextBlock(350.0, 720.0, 75, 50);
            editAreaPlayer.Background = Brushes.Black;
            editAreaPlayer.Text = "player\nspawn";
            editAreaPlayer.FontSize = 12;

            InitiateWorldObject("Base", 375, 775);

        }

        private void Level_Save(object sender, EventArgs e)
        {

            string newpath = HighScoreManager.Instance.path;
            int indexPos = newpath.IndexOf("\\JoustModel");
            newpath = newpath.Substring(0, indexPos);
            newpath += "\\JoustClient\\Saves\\Custom Stages";

            int indextracker = 0;
            FileStream fs = File.Create(newpath + "\\" + namebox.Text + ".txt");
            using (fs)
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    foreach(object o in canvas.Children)
                    {
                        indextracker++;

                        PlatformControl pc = o as PlatformControl;
                        if (pc != null)
                        {
                            Platform platty = new Platform();
                            System.Windows.Point point = pc.TransformToAncestor(canvas).Transform(new System.Windows.Point(0, 0));
                            platty.coords.x = point.X;
                            platty.coords.y = point.Y;
                            platty.SetType("short", ids + 1);
                            ids++;
                            string x = platty.Serialize();
                            writer.Write(x);

                            if (indextracker < canvas.Children.Count) { writer.Write(":"); }
                        }

                        RespawnControl rc = o as RespawnControl;
                        if (rc != null) {
                            Respawn platty = new Respawn();
                            System.Windows.Point point = rc.TransformToAncestor(canvas).Transform(new System.Windows.Point(0, 0));
                            platty.coords.x = point.X;
                            platty.coords.y = point.Y;
                            ids++;
                            string x = platty.Serialize();
                            writer.Write(x);

                            if (indextracker < canvas.Children.Count) { writer.Write(":"); }
                        }
                    }
                }
            }

            canvas.Children.Clear();
            designer_on = false;
            TextBlock message = Make_TextBlock(300, 620.0, 50, 200);
            message.Text = "Level Created!";
            message.TextAlignment = TextAlignment.Center;
            Button back = Make_BackButton(625.0, Title_Screen);

        }

        private void plat_button(object sender, EventArgs e)
        {
            PlatformControl platctrl = new PlatformControl("Images/Platform/platform_short1.png");
            platctrl.SetValue(Canvas.TopProperty, 0.0);
            platctrl.SetValue(Canvas.LeftProperty, 0.0);
            canvas.Children.Add(platctrl);
            platctrl.MouseDown += plat_MouseDown;
            ex2.IsEnabled = true;
        }

        private void spawn_button(object sender, EventArgs e)
        {
            RespawnControl spwnctrl = new RespawnControl("Images/Platform/platform_respawn1.png");
            spwnctrl.SetValue(Canvas.TopProperty, 0.0);
            spwnctrl.SetValue(Canvas.LeftProperty, 0.0);
            spwnctrl.Width = 100;
            spwnctrl.Height = 15;
            spwnctrl.Tag = "disposeable";
            // Bring forward
            Canvas.SetZIndex(spwnctrl, 3);
            canvas.Children.Add(spwnctrl);
            spwnctrl.MouseDown += plat_MouseDown;

            Button sent = sender as Button;
            sent.IsEnabled = false;
        }

        private void Plat_Delete(object sender, EventArgs e)
        {
            canvas.Children.Remove(currPlat);
            if (currPlat.Tag != null)
            {
                ex2.IsEnabled = false;
            }
        }

        private void plat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mousePosition = e.GetPosition(canvas);
            dragging = true;

            if (sender is PlatformControl) {
                currPlat = sender as PlatformControl;
            }
            else if (sender is RespawnControl) {
                currPlat = sender as RespawnControl;
            }
            currImg = null;
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!designer_on) return;

            dragging = false;
            Positioning();
        }

        private void Positioning()
        {
            foreach (object x in canvas.Children)
            {
                if (currPlat != null)
                {
                    if (x is PlatformControl) {
                        PlatformControl z = x as PlatformControl;

                        if (currPlat is RespawnControl) {
                            int objects = 0;
                            foreach (object test in canvas.Children) {
                                PlatformControl testCtrl = test as PlatformControl;
                                objects++;
                                if (objects >= canvas.Children.Count - 1) {
                                    isLastCheck = true;
                                    objects = 0;
                                }
                                else {
                                    isLastCheck = false;
                                }

                                if (testCtrl != null) {
                                    CheckBadPlacementRespawn(testCtrl);
                                }
                            }
                        }
                        else {
                            if (z != null) {
                                CheckBadPlacement(z);
                            }
                        }
                    }
                }
            }
        }

        private void CheckBadPlacement(PlatformControl z) {
            System.Windows.Point relativePoint = z.TransformToAncestor(canvas).Transform(new System.Windows.Point(0, 0));
            System.Windows.Point relativePoint2 = currPlat.TransformToAncestor(canvas).Transform(new System.Windows.Point(0, 0));

            if ((relativePoint2.Y <= 30.0) || (relativePoint2.Y > 730)) {
                //canvas.Children.Remove(currPlat);
                currPlat.SetValue(Canvas.TopProperty, 0.0);
                currPlat.SetValue(Canvas.LeftProperty, 0.0);

                Task.Run(() => {
                    Dispatcher.Invoke(() => {
                        errorBlock.Background = Brushes.Red;
                        errorBlock.Text = "out of edit area";
                    });
                    Thread.Sleep(2000);
                    Dispatcher.Invoke(() => {
                        errorBlock.Background = Brushes.Blue;
                        errorBlock.Text = "";
                    });
                });
            }

            if ((relativePoint2.X < 0.0) || (relativePoint2.X > 1240)) {
                //canvas.Children.Remove(currPlat);
                currPlat.SetValue(Canvas.TopProperty, 0.0);
                currPlat.SetValue(Canvas.LeftProperty, 0.0);

                Task.Run(() => {
                    Dispatcher.Invoke(() => {
                        errorBlock.Background = Brushes.Red;
                        errorBlock.Text = "out of edit area";
                    });
                    Thread.Sleep(2000);
                    Dispatcher.Invoke(() => {
                        errorBlock.Background = Brushes.Blue;
                        errorBlock.Text = "";
                    });
                });
            }

            if (((relativePoint2.Y >= 350) && (relativePoint.Y <= 425)) && ((relativePoint.X >= 720.0) && (relativePoint.X <= 770.0))) {
                //canvas.Children.Remove(currPlat);
                currPlat.SetValue(Canvas.TopProperty, 0.0);
                currPlat.SetValue(Canvas.LeftProperty, 0.0);

                Task.Run(() => {
                    Dispatcher.Invoke(() => {
                        errorBlock.Background = Brushes.Red;
                        errorBlock.Text = "spawn block";
                    });
                    Thread.Sleep(2000);
                    Dispatcher.Invoke(() => {
                        errorBlock.Background = Brushes.Blue;
                        errorBlock.Text = "";
                    });
                });
            }

            if (((relativePoint2.X >= 520) && (relativePoint2.X <= 770)) && ((relativePoint2.Y >= 320) && (relativePoint2.Y <= 425))) {
                //TextBlock editAreaPlayer = Make_TextBlock(350.0, 720.0, 75, 50);

                //canvas.Children.Remove(currPlat);
                currPlat.SetValue(Canvas.TopProperty, 0.0);
                currPlat.SetValue(Canvas.LeftProperty, 0.0);

                Task.Run(() => {
                    Dispatcher.Invoke(() => {
                        errorBlock.Background = Brushes.Red;
                        errorBlock.Text = "spawn block";

                    });
                    Thread.Sleep(2000);
                    Dispatcher.Invoke(() => {
                        errorBlock.Background = Brushes.Blue;
                        errorBlock.Text = "";
                    });
                });
            }

            if (!Object.ReferenceEquals(z, currPlat)) {
                if (System.Math.Abs((relativePoint.Y - relativePoint2.Y)) <= 75.0 && System.Math.Abs((relativePoint.X - relativePoint2.X)) <= z.Width) {
                    //canvas.Children.Remove(currPlat);
                    currPlat.SetValue(Canvas.TopProperty, 0.0);
                    currPlat.SetValue(Canvas.LeftProperty, 0.0);

                    Task.Run(() => {
                        Dispatcher.Invoke(() => {
                            errorBlock.Background = Brushes.Red;
                            errorBlock.Text = "bad collision";
                            ///
                            if (currPlat.Tag != null) {
                                Task.Run(() => {
                                    Dispatcher.Invoke(() => {
                                        errorBlock.Text = "invalid spawn";
                                    });
                                    Thread.Sleep(2000);
                                    Dispatcher.Invoke(() => {
                                        errorBlock.Text = "";
                                    });
                                });
                            }
                            ///
                        });
                        Thread.Sleep(2000);
                        Dispatcher.Invoke(() => {
                            errorBlock.Background = Brushes.Blue;
                            errorBlock.Text = "";
                        });
                    });
                }
            }
        }

        private void CheckBadPlacementRespawn(PlatformControl z) {
            System.Windows.Point relativePoint = z.TransformToAncestor(canvas).Transform(new System.Windows.Point(0, 0));
            System.Windows.Point relativePoint2 = currPlat.TransformToAncestor(canvas).Transform(new System.Windows.Point(0, 0));

            if ((relativePoint2.Y <= 30.0) || (relativePoint2.Y > 730)) {
                //canvas.Children.Remove(currPlat);
                currPlat.SetValue(Canvas.TopProperty, 0.0);
                currPlat.SetValue(Canvas.LeftProperty, 0.0);

                Task.Run(() => {
                    Dispatcher.Invoke(() => {
                        errorBlock.Background = Brushes.Red;
                        errorBlock.Text = "out of edit area";
                    });
                    Thread.Sleep(2000);
                    Dispatcher.Invoke(() => {
                        errorBlock.Background = Brushes.Blue;
                        errorBlock.Text = "";
                    });
                });
            }

            if ((relativePoint2.X < 0.0) || (relativePoint2.X > 1240)) {
                //canvas.Children.Remove(currPlat);
                currPlat.SetValue(Canvas.TopProperty, 0.0);
                currPlat.SetValue(Canvas.LeftProperty, 0.0);

                Task.Run(() => {
                    Dispatcher.Invoke(() => {
                        errorBlock.Background = Brushes.Red;
                        errorBlock.Text = "out of edit area";
                    });
                    Thread.Sleep(2000);
                    Dispatcher.Invoke(() => {
                        errorBlock.Background = Brushes.Blue;
                        errorBlock.Text = "";
                    });
                });
            }

            if (((relativePoint2.Y >= 350) && (relativePoint.Y <= 425)) && ((relativePoint.X >= 720.0) && (relativePoint.X <= 770.0))) {
                //canvas.Children.Remove(currPlat);
                currPlat.SetValue(Canvas.TopProperty, 0.0);
                currPlat.SetValue(Canvas.LeftProperty, 0.0);

                Task.Run(() => {
                    Dispatcher.Invoke(() => {
                        errorBlock.Background = Brushes.Red;
                        errorBlock.Text = "spawn block";

                    });
                    Thread.Sleep(2000);
                    Dispatcher.Invoke(() => {
                        errorBlock.Background = Brushes.Blue;
                        errorBlock.Text = "";
                    });
                });
            }

            if (((relativePoint2.X >= 520) && (relativePoint2.X <= 770)) && ((relativePoint2.Y >= 320) && (relativePoint2.Y <= 425))) {
                //TextBlock editAreaPlayer = Make_TextBlock(350.0, 720.0, 75, 50);

                //canvas.Children.Remove(currPlat);
                currPlat.SetValue(Canvas.TopProperty, 0.0);
                currPlat.SetValue(Canvas.LeftProperty, 0.0);

                Task.Run(() => {
                    Dispatcher.Invoke(() => {
                        errorBlock.Background = Brushes.Red;
                        errorBlock.Text = "spawn block";

                    });
                    Thread.Sleep(2000);
                    Dispatcher.Invoke(() => {
                        errorBlock.Background = Brushes.Blue;
                        errorBlock.Text = "";
                    });
                });
            }

            if (!Object.ReferenceEquals(z, currPlat)) {
                if (System.Math.Abs((relativePoint.Y - relativePoint2.Y)) > 20 || System.Math.Abs((relativePoint.X - relativePoint2.X)) > z.Width) {
                    //canvas.Children.Remove(currPlat);
                    currPlat.SetValue(Canvas.TopProperty, 0.0);
                    currPlat.SetValue(Canvas.LeftProperty, 0.0);
                    if (isLastCheck) {
                        Task.Run(() => {
                            Dispatcher.Invoke(() => {
                                errorBlock.Background = Brushes.Red;
                                errorBlock.Text = "bad collision";
                                ///
                                if (currPlat.Tag != null) {
                                    Task.Run(() => {
                                        Dispatcher.Invoke(() => {
                                            errorBlock.Text = "invalid spawn";
                                        });
                                        Thread.Sleep(2000);
                                        Dispatcher.Invoke(() => {
                                            errorBlock.Text = "";
                                        });
                                    });
                                }
                                ///
                            });
                            Thread.Sleep(2000);
                            Dispatcher.Invoke(() => {
                                errorBlock.Background = Brushes.Blue;
                                errorBlock.Text = "";
                            });
                        });
                    }
                }
                else {
                    Canvas.SetLeft(currPlat, Canvas.GetLeft(z) + 50);
                    Canvas.SetTop(currPlat, Canvas.GetTop(z));
                }
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!designer_on) return;
            if (!dragging) return;

            if (currImg == null)
            {
                var position = e.GetPosition(canvas);
                var offset = position - mousePosition;
                mousePosition = position;
                Canvas.SetLeft(currPlat, Canvas.GetLeft(currPlat) + offset.X);
                Canvas.SetTop(currPlat, Canvas.GetTop(currPlat) + offset.Y);
            }

            else if (currPlat == null)
            {
                var position = e.GetPosition(canvas);
                var offset = position - mousePosition;
                mousePosition = position;
                Canvas.SetLeft(currImg, Canvas.GetLeft(currImg) + offset.X);
                Canvas.SetTop(currImg, Canvas.GetTop(currImg) + offset.Y);
            }
        }
    }
    
}
