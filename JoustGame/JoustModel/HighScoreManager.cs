using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JoustModel
{
    // handles saving and loading high scores
    public class HighScoreManager
    {
        // must be set with the "Load" method
        public List<Score> AllScores { get; set; }
        public int lowestScore = 0;
        public string path = "";

        private HighScoreManager()
        {
            string newpath = Directory.GetCurrentDirectory();
            int indexPos = newpath.IndexOf("\\JoustClient");

            if (!(indexPos == -1))
            {
                newpath = newpath.Substring(0, indexPos);
                newpath += "\\JoustModel\\Saves\\HighScores\\highscores.txt";
            }
            else
            {
                indexPos = newpath.IndexOf("\\JoustModel");
                newpath = newpath.Substring(0, indexPos);
                newpath += "\\JoustModel\\Saves\\HighScores\\highscores.txt";
            }

            this.path = newpath;
            this.AllScores = HighScoreManager.Load(this.path);
            this.lowestScore = replaceLow(this.AllScores);
        }

        private static HighScoreManager instance = new HighScoreManager();
        public static HighScoreManager Instance
        {
            get { return instance; }
        }

        public void AddScore(Score newScore)
        {
            // when a player's score is created, the points attribute is 
            // checked to see if it is less than the lowestScore. If it is, 
            // it replaces that score
            // 
            // calls the Load method to update the list here

            if (newScore.points >= this.lowestScore)
            {

                int lineCount = File.ReadLines(this.path).Count();

                if (lineCount < 10)
                {
                    StreamWriter sw = File.AppendText(this.path);

                    using (sw)
                    {
                        if (lineCount == 0)
                        {
                            sw.Write(newScore.points.ToString() + "," + newScore.username);
                        }
                        else
                        {
                            sw.WriteLine("");
                            sw.Write(newScore.points.ToString() + "," + newScore.username);
                        }
                    }

                    this.AllScores = HighScoreManager.Load(this.path);
                    HighScoreManager.Instance.replaceLow();
                }
                else if (lineCount == 10)
                {
                    string[] lines = File.ReadAllLines(this.path);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string[] splitted = lines[i].Split(',');
                        if (Convert.ToInt32(splitted[0]) == this.lowestScore)
                        {
                            lines[i] = newScore.points.ToString() + "," + newScore.username;
                        }
                    }
                    File.WriteAllLines(this.path, lines);
                    this.AllScores = HighScoreManager.Load(this.path);
                    HighScoreManager.Instance.replaceLow();
                }
            }
        }

        private static  List<Score> Load(string filepath)
        {
            // line by line will split the txt file and return a 
            // list of the top ten scores. called by the AddScore
            // method

            List<Score> toReturn = new List<Score>();

            if (!(File.Exists(filepath)))
            {
                FileStream fs = File.Create(filepath);
                fs.Close();

                toReturn = HighScoreManager.OrderList(toReturn);
                return toReturn;
            }
            else
            {
                string contents = File.ReadAllText(filepath);
                string[] check = contents.Split('\n');

                foreach (string i in check)
                {
                    if (!(i.Equals("")) && !(char.IsControl(i.ToCharArray()[0])))
                    {
                        string[] attrib = i.Split(',');
                        Score toAdd = new Score(Convert.ToInt32(attrib[0]), attrib[1]);
                        toReturn.Add(toAdd);
                    }
                }
                toReturn = HighScoreManager.OrderList(toReturn);
                return toReturn;
            }
        }



        private static List<Score> OrderList(List<Score> listToOrder)
        {
            List<Score> toReturn = listToOrder.OrderBy(o => o.points).ToList();
            toReturn.Reverse();
            return toReturn;
        }

        public void replaceLow()
        {
            int lineCount = File.ReadLines(this.path).Count();
            if (lineCount > 0)
            {
                this.lowestScore = this.AllScores[this.AllScores.Count - 1].points;
            }
        }

        public int replaceLow(List<Score> listScores)
        {
            int lineCount = File.ReadLines(this.path).Count();
            if (lineCount > 0)
            {
                return listScores[listScores.Count - 1].points;
            }

            return 0;
        }

        public void testReset()
        {
            this.lowestScore = 0;
            this.AllScores = new List<Score>();

            File.WriteAllText(this.path, String.Empty);
        }
    }
}
