using System;
using System.Diagnostics;
using System.Windows.Threading;
using System.Collections.Generic;

namespace JoustModel
{
    public class GameController
    {
        public DispatcherTimer updateTimer;
        public World WorldRef { get; set; }

        public GameController()
        {
            WorldRef = World.Instance;

            updateTimer = new DispatcherTimer(
                TimeSpan.FromMilliseconds(5),
                DispatcherPriority.Render,
                Update,
                Dispatcher.CurrentDispatcher);
        }
        
        public void Update(object sender, EventArgs e)
        {
            try
            {
                //Trace.WriteLine("WorldObject Count: " + WorldRef.objects.Count);
                // Update everything 200 times per second (subject to change)
                if (WorldRef.multiplayerStarted)
                {
                    foreach (Ostrich ostrich in WorldRef.players)
                    {
                        ostrich.Update();
                        World.Instance.TrackTime();
                    }
                }
                else
                {
                    foreach (WorldObject worldObject in WorldRef.objects)
                    {
                        Entity entity = worldObject as Entity;
                        if (entity != null)
                        {
                            entity.Update();
                            World.Instance.TrackTime();
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
                return;
            } 
        }

        public void SetPlatforms()
        {
            InitiateWorldObject("Platform", 100, 300);
            InitiateWorldObject("Platform", 700, 500);
            InitiateWorldObject("Platform", 500, 300);
            InitiateWorldObject("Platform", 950, 200);
            InitiateWorldObject("Respawn", 700, 100);
            InitiateWorldObject("Respawn", 1100, 600);
            InitiateWorldObject("Respawn", 200, 600);
            InitiateWorldObject("Base", 375, 775);
        }

        public WorldObject InitiateWorldObject(string type, double x, double y)
        {
            WorldObject obj = CreateWorldObj(type);
            obj.coords.x = x;
            obj.coords.y = y;
            return obj;
        }

        public void InitializePlayers()
        {
            for (int i = 0; i < WorldRef.playerNames.Count; i++)
            {
                Platform spawn = WorldRef.platforms[i];
                Ostrich o = InitiateWorldObject("Ostrich", spawn.coords.x + spawn.width / 2, spawn.coords.y - 75) as Ostrich;
            }
        }

        public void CalculateNumEnemies(ref int numBuzzards, ref int numPterodactyls)
        {
            int stage = WorldRef.stage;
            numBuzzards = stage + 3;
            if (stage >= 5)
            {
                numPterodactyls = (stage - 4) + (stage / 2);
            }
        }

        public void SpawnEnemies(int numBuzzards, int numPterodactyls)
        {
            for (int i = 0; i < numBuzzards; i++)
            {
                Buzzard b = new Buzzard();
                b.coords = new Point(500, 500);
            }
        }

        public void Load(string filename)
        {
            string loadedLine = System.IO.File.ReadAllText(string.Format(@"../../Saves/GameSaves/{0}.txt", filename));
            string[] savedObjects = loadedLine.Split(':');
            foreach (string savedObj in savedObjects)
            {
                if (savedObj != "\r\n" && savedObj.Length > 0)
                {
                    string type = savedObj.Substring(0, savedObj.IndexOf(","));
                    WorldObject obj = CreateWorldObj(type);
                    obj.Deserialize(savedObj);
                }
            }
        }

        public string Save()
        {
            string filename = DateTime.Now.ToString("H-mm-ss");
            string line2save = "";
            foreach (WorldObject obj in WorldRef.objects)
            {
                line2save += obj.Serialize() + ':';
            }

            string path = string.Format(@"../../Saves/GameSaves/{0}.txt", filename);
            System.IO.File.WriteAllText(path, line2save); 
            return line2save;
        }

        public WorldObject CreateWorldObj(string type)
        {
            switch (type)
            {
                case "Ostrich":
                    string name;
                    if (WorldRef.hosted)
                    {
                        name = WorldRef.playerNames[WorldRef.players.Count];
                    }
                    else
                    {
                        name = "single";
                    }
                    Ostrich ostrich = new Ostrich(name);
                    return ostrich;
                case "Base":
                    return new Base();
                case "Buzzard":
                    return new Buzzard();
                case "Egg":
                    return new Egg();
                case "Platform":
                    Platform platform = new Platform();
                    WorldRef.platforms.Add(platform);
                    return platform;
                case "Pterodactyl":
                    return new Pterodactyl();
                case "Respawn":
                    return new Respawn();
                default:
                    return null;
            }
        }
    }
}
