using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class GameController
    {
        public World WorldRef { get; set; }

        public GameController()
        {
            WorldRef = World.Instance;
        }

        public void Update()
        {
            try
            {
                // Update everything 50 times per second (subject to change)
                foreach (WorldObject worldObject in WorldRef.objects)
                {
                    Entity entity = worldObject as Entity;
                    if (entity != null)
                    {
                        entity.Update();
                        World.Instance.UpdateAllEnemies_Position();
                    }
                }
            }
            catch (InvalidOperationException)
            {
                return;
            } 
        }

        public void CalculateNumEnemies(int stage, ref int numBuzzards, ref int numPterodactyls)
        {
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
                WorldRef.objects.Add(b);
            }
        }

        public void Load(string filename)
        {
            string loadedLine = System.IO.File.ReadAllText(string.Format(@"{0}.txt", filename)); // Where should the game save files be put?
            string[] savedObjects = loadedLine.Split(':');
            foreach (string savedObj in savedObjects)
            {
                if (savedObj.Length > 0)
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

            string path = string.Format(@"{0}.txt", filename);
            System.IO.File.WriteAllText(path, line2save); 
            return line2save;
        }

        public WorldObject CreateWorldObj(string type)
        {
            switch (type)
            {
                case "Ostrich":
                    return new Ostrich();
                case "Base":
                    return new Base();
                case "Buzzard":
                    return new Buzzard();
                case "Egg":
                    return new Egg();
                case "Platform":
                    return new Platform();
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
