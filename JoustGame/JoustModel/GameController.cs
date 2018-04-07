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

        public void Load(string filename)
        {
            string loadedLine = System.IO.File.ReadAllText("C:/Users/Soarex/Desktop/Github Projects/joust/joust/Save2018-4-2-5-32-00");
            string[] savedObjects = loadedLine.Split(':');
            foreach (string savedObj in savedObjects)
            {
                string type = savedObj.Substring(0, savedObj.IndexOf(" "));
                WorldObject obj = CreateWorldObj(type);
                obj.Deserialize(savedObj.Substring(savedObj.IndexOf(" "), -1));

                
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
