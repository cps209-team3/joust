using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JoustModel
{
    [TestClass]
    class HighScoreManagerTest
    {
        [TestMethod]
        public void Score_Creation_Test()
        {
            List<Score> testList = new List<Score>();

            Score a = new Score("efting", 14);
            Score c = new Score("franklin", 12);
            Score b = new Score("cockrell", 13);

            Score sa = new Score("sandy", 3);
            Score sb = new Score("sandy", 2);
            Score sc = new Score("sandy", 1);
            Score sd = new Score("sandy", 4);

            testList.Add(a);
            testList.Add(b);
            testList.Add(c);

            testList.Add(sa);
            testList.Add(sb);
            testList.Add(sc);
            testList.Add(sd);

            foreach (Score i in testList)
            {
                HighScore_Manager.AddScore(i);
            }

            List<Score> checkList = HighScore_Manager.getScores("someFile");

            List<Score> checkList2 = new List<Score>();

            checkList2.Add(a);
            checkList2.Add(b);
            checkList2.Add(c);

            checkList2.Add(sd);
            checkList2.Add(sb);
            checkList2.Add(sc);

            Assert.IsTrue(checkList2.Equals(checkList));
        }
    }
}
