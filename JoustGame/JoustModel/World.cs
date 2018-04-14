using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class World
    {
        // Event handler to notify the view
        public event EventHandler SpawnPterodactyl;

        // Public instance variables
        public List<WorldObject> objects;
        public Ostrich player;
        public int stage;

        // Handle stage time to spawn pterodactyls
        private int stageTimeMinutes;
        private int stageTimeSeconds;
        private int stageTimeFrame;

        // Spawn pterodactyls after 1 minute
        private const int PTERODACTYL_SPAWN_MINUTES = 1;


        private World()
        {
            objects = new List<WorldObject>();
        }

        private static World instance = new World();
        public static World Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Executes on moveTimer Tick in MainWindow.xaml.cs. It loops through the
        /// objects in the World and executes their Update method if a Buzzard, Egg,
        /// or a Pterodactyl.
        /// </summary>
        public void UpdateAllEnemies_Position() {
            try {
                foreach (WorldObject obj in objects) {
                    Buzzard buzzardObj = obj as Buzzard;
                    if (buzzardObj != null) {
                        buzzardObj.Update();
                    }
                    Egg eggObj = obj as Egg;
                    if (eggObj != null) {
                        eggObj.Update();
                    }

                    // Used to keep track of the stage time for spawning the Pterodactyls
                    stageTimeFrame++;
                    if (stageTimeFrame == 500) {
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

                    Pterodactyl pterodactylObj = obj as Pterodactyl;
                    if (pterodactylObj != null) {
                        pterodactylObj.Update();
                    }
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
