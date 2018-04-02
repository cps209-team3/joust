using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    class Ostrich : Entity, ISerializable
    {
        public int lives;
        public int stage;
        public int score;

        public override int Value { get; set; }

        public Ostrich()
        {
            Value = 750;
            score = 0;
        }
        
        //Serialization
        public string Serialize()
        {
            return string.Format("Ostrich, {0}, {1}, {2}, {3}", this.score, lives, stage, this.coords);
        }

        public void Deserialize(string data)
        {
            // set score
            // set lives
            // set stage
            // set coords

        }
    }
}
