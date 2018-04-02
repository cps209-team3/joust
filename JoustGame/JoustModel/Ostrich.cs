using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoustModel
{
    public class Ostrich : Entity, ISerializable
    {
        public int score;
        public int lives;

        public override int Value { get; set; }

        public Ostrich()
        {
            Value = 750;
            lives = 3;
            score = 0;
        }

        public string Serialize()
        {
            return string.Format("Ostrich, {0}, {1}, {2}, {3}", this.score, lives, stage, this.coords);
        }

        public void Deserialize(string score, string lives, string stage, string coords)
        {
            this.score = Convert.ToInt32(score);
            this.lives = Convert.ToInt32(lives);
            this.coords = coords; //convert to point
        }
}
}