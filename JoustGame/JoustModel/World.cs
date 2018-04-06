using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class World
    {
        public List<WorldObject> objects = new List<WorldObject>() { };
        public int stage;

        private World() { }

        private static World instance = new World();
        public static World Instance
        {
            get { return instance; }
        }
    }
}
