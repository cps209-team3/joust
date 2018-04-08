using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class World
    {
        public List<WorldObject> objects;
        public int stage;

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
            foreach (WorldObject obj in objects) {
                Buzzard buzzardObj = obj as Buzzard;
                if (buzzardObj != null) {
                    buzzardObj.Update();
                }
            }
        }

        public void UpdateAllEnemies_State(object sender, EventArgs e) {
            foreach (WorldObject obj in objects) {
                Buzzard buzzardObj = obj as Buzzard;
                if (buzzardObj != null) {
                    buzzardObj.UpdateState();
                }
            }
        }
    }
}
