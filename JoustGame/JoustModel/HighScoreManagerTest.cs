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
        public void TestScoreCreation()
        {
            List<Score> testList = new List<Score>();

            Score a = new Score(14,"efting");
            Score c = new Score(12, "franklin");
            Score b = new Score(13, "cockrell");

            Score sa = new Score(3, "sandy");
            Score sb = new Score(2, "sandy");
            Score sc = new Score(1, "sandy");
            Score sd = new Score(4, "sandy");

            testList.Add(a);
            testList.Add(b);
            testList.Add(c);

            testList.Add(sa);
            testList.Add(sb);
            testList.Add(sc);
            testList.Add(sd);

            foreach (Score i in testList)
            {
                HighScoreManager.Instance.AddScore(i);
            }

            List<Score> checkList = HighScoreManager.Instance.AllScores;

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
