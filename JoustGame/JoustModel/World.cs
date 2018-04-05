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
            stage = 0;
        }

        private static World instance = new World();
        public static World Instance
        {
            get { return instance; }
        }
    }
}
