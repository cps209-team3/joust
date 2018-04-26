using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JoustModel
{
    // handles saving and loading high scores
    public class HighScoreManager
    {
        // a list of scores representing the file's contents
        public List<Score> AllScores { get; set; }

        // lowest score on file; used for adding
        public Score lowestScore;

        // a path that is fully set by the constructor
        public string path = "";

        // represents the amount of lines in the file; set 
        public int lineCount;

        /// <summary>
        /// private constructor
        /// </summary>
        private HighScoreManager()
        {
            string newpath = Directory.GetCurrentDirectory();
            int indexPos = newpath.IndexOf("\\JoustClient");

            if (!(indexPos == -1))
            {
                // just in case condition; if, for example, exe gets moved or something

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
            this.AllScores = Load(this.path);
            this.replaceLow();
        }

        // instance of the HighScoreManager class
        private static HighScoreManager instance = new HighScoreManager();
        public static HighScoreManager Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// adds the give Score to the local list; then saves it to the file
        /// </summary>
        /// <param name="newScore"></param>
        public void AddScore(Score newScore)
        {
            if (AllScores.Count < 10)
            {
                this.AllScores.Add(newScore);
            }
            else if (newScore.points >= lowestScore.points)
            {
                this.AllScores.Remove(lowestScore);
                this.AllScores.Add(newScore);
            }
            this.AllScores = OrderList(this.AllScores.OrderBy(o => o.points).ToList());
            this.Save();
        }

        /// <summary>
        /// sets the lineCount variable
        /// </summary>
        public void FindLines()
        {
            this.lineCount = File.ReadLines(this.path).Count();
        }

        /// <summary>
        /// takes the local list and saves it to the file
        /// </summary>
        public void Save()
        {
            // when a player's score is created, the points attribute is 
            // checked to see if it is less than the lowestScore. If it is, 
            // it replaces that score
            // 
            // calls the Load method to update the list here

            File.WriteAllText(this.path, String.Empty);
            foreach (Score newScore in this.AllScores)
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
                this.replaceLow();
                this.FindLines();
                HighScoreManager.Load(this.path);
            }
        }

        /// <summary>
        /// gives back all the lines of the file
        /// </summary>
        /// <returns></returns>
        private string[] GetText()
        {
            return File.ReadAllLines(this.path);
        }

        /// <summary>
        /// loads the local list with the file contents
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private static List<Score> Load(string filepath)
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
                toReturn = OrderList(toReturn);
                return toReturn;
            }
        }

        /// <summary>
        /// orders a give Score list
        /// </summary>
        /// <param name="listToOrder"></param>
        /// <returns></returns>
        private static List<Score> OrderList(List<Score> listToOrder)
        {
            List<Score> toReturn = listToOrder.OrderBy(o => o.points).ToList();
            toReturn.Reverse();
            return toReturn;
        }

        /// <summary>
        /// sets the lowest score on file
        /// </summary>
        public void replaceLow()
        {
            if (this.AllScores.Count > 0)
            {
                Score compare = this.AllScores[0];
                foreach (Score s in this.AllScores)
                {
                    if (s.points < compare.points)
                    {
                        compare = s;
                    }
                }

                this.lowestScore = compare;
            }
        }

        /// <summary>
        /// for testing
        /// </summary>
        public void testReset_pathChange()
        {
            this.lowestScore = null;
            this.AllScores = new List<Score>();

            string newpath = this.path;
            int indexPos = newpath.IndexOf("\\highscores.txt");
            newpath = newpath.Substring(0, indexPos);
            newpath += "\\test.txt";

            this.path = newpath;
            File.WriteAllText(this.path, String.Empty);
        }
    }
}
