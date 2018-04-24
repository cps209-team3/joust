using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JoustModel
{
    public class World
    {
        // Event handler to notify the view
        public event EventHandler SpawnPterodactyl;
        public event EventHandler<int> win;

        // Public instance variables
        public List<WorldObject> objects;
        public List<Enemy> enemies;
        public Ostrich player;
        public Base basePlatform;
        public int stage;
        public List<Point[]> SpawnPoints { get; set; }

        // Handle stage time to spawn pterodactyls
        private int stageTimeMinutes;
        private int stageTimeSeconds;
        private int stageTimeFrame;

        // Spawn pterodactyls after 1 minute
        private const int PTERODACTYL_SPAWN_MINUTES = 1;


        private World()
        {
            objects = new List<WorldObject>();
            enemies = new List<Enemy>();
            players = new List<Ostrich>();
            playerNames = new List<string>();
        }

        public void Reset() {
            objects = new List<WorldObject>();
            enemies = new List<Enemy>();
        }

        private static World instance = new World();
        public static World Instance
        {
            get { return instance; }
        }

        public void CheckWin()
        {
            if (enemies.Count == 0 && win != null)
            {
                win(this, 0);
            }
        }

        /// <summary>
        /// Executes on moveTimer Tick in MainWindow.xaml.cs. It loops through the
        /// objects in the World and executes their Update method if a Buzzard, Egg,
        /// or a Pterodactyl.
        /// </summary>
        public void TrackTime() {
            try {
                // Used to keep track of the stage time for spawning the Pterodactyls
                stageTimeFrame++;
                if (stageTimeFrame == 300) {
                    stageTimeSeconds++;
                    stageTimeFrame = 0;
                }
                if (stageTimeSeconds == 60) {
                    stageTimeMinutes++;
                    stageTimeSeconds = 0;
                }
                if (stageTimeMinutes == PTERODACTYL_SPAWN_MINUTES) {
                    stageTimeMinutes = 0;
                    if (SpawnPterodactyl != null)
                        SpawnPterodactyl(Instance, null);
                }
            } catch (InvalidOperationException op) {
                // This exception was being thrown when I added to the objects 
                // List from other code while this loop is executing. No noticable
                // bugs. Possible that it skips a Tick, but not an extreme problem.
                Trace.WriteLine(op.Message);
            }
        }
    }
}
