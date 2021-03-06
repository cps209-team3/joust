using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class Platform : WorldObject
    {
        private string PlatformType { get; set; }
        private int PlatformNumber { get; set; }
        public Platform()
        {
            width = 200;
            height = 30;
            type = "Platform";
            imagePath = "Images/Platform/platform_short1.png";
            World.Instance.objects.Add(this);
        }

        public void SetType(string type, int number) { 
            PlatformType = type;
            PlatformNumber = number;
            Console.WriteLine("platform type = " + type);
            switch (type) {
                case "long":
                    width = 400;
                    height = 40;
                    type = "Platform";
                    if (number < 4) imagePath = "Images/Platform/platform_long" + number + ".png";
                    else imagePath = "Images/Platform/platform_long1.png";
                    World.Instance.objects.Add(this); 
                    break;
                case "short":
                default:
                    Console.WriteLine("Entered into default");
                    Console.WriteLine("number = " + number);
                    width = 200;
                    height = 30;
                    type = "Platform";
                    if (number < 5) imagePath = "Images/Platform/platform_short" + number + ".png";
                    else imagePath = "Images/Platform/platform_short1.png";
                    World.Instance.objects.Add(this); 
                    break;
            }
        }

        // returns the properties of this Platform object in string form
        public override string Serialize()
        {
            Console.WriteLine("platformType = " + PlatformType);
            return string.Format("Platform,{0},{1},{2},{3}", PlatformType, PlatformNumber, coords.x, coords.y);
        }

        // Set coords to value read from file
        public override void Deserialize(string data)
        {
            string[] properties = data.Split(',');
            coords.x = Convert.ToDouble(properties[3]); // set x coord
            coords.y = Convert.ToDouble(properties[4]); // set y coord
        }

        public override string ToString()
        {
            return "Platform";
        }
    }
}
