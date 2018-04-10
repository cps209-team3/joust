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
        public event EventHandler SpawnPterodactyl;
        public List<WorldObject> objects;
        public int stage;

        private int stageTimeMinutes;
        private int stageTimeSeconds;
        private int stageTimeFrame;


        private World()
        {
            objects = new List<WorldObject>();
        }

        private static World instance = new World();
        public static World Instance
        {
            get { return instance; }
        }

        public void UpdateAllEnemies_Position(object sender, EventArgs e) {
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

                    stageTimeFrame++;

                    if (stageTimeFrame == 500) {
                        stageTimeSeconds++;
                        stageTimeFrame = 0;
                        Trace.WriteLine(stageTimeSeconds);
                    }

                    if (stageTimeSeconds == 60) {
                        stageTimeMinutes++;
                        stageTimeSeconds = 0;
                        Trace.WriteLine(stageTimeMinutes);
                    }

                    if (stageTimeSeconds == 20) {
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
                Trace.WriteLine(op.Message);
            }
        }
    }
}
