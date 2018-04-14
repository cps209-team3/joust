using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace JoustModel
{
    [TestClass]
    public class HighScoreManagerTest
    {
        [TestMethod]
        public void TestScoreCreation()
        {
            HighScoreManager.Instance.testReset_pathChange();

            List<Score> testList = new List<Score>();

            Score sa = new Score(1, "sandy");
            Score sb = new Score(2, "sandy");
            Score sc = new Score(3, "sandy");

            Score sz = new Score(1, "sandy");

            testList.Add(sa);
            testList.Add(sb);
            testList.Add(sc);

            foreach (Score i in testList)
            {
                HighScoreManager.Instance.AddScore(i);
            }

            List<Score> checkList = HighScoreManager.Instance.AllScores;

            List<Score> checkList2 = new List<Score>();

            checkList2.Add(sc);
            checkList2.Add(sb);
            checkList2.Add(sa);

            for (int i = 0; i < 3; i++)
            {
                Assert.IsTrue(Score.Equals_Mine(checkList[i], checkList2[i]));
            }
        }
    }
}
