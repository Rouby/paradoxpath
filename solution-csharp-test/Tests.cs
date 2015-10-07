using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using solution_csharp;
using System.Linq;

namespace solution_csharp_test
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestExample1()
        {
            var pMap = new char[] { (char)1, (char)1, (char)1, (char)1, (char)0, (char)1, (char)0, (char)1, (char)0, (char)1, (char)1, (char)1 };
            var pOutBuffer = new int[12];
            var result = Solution.FindPath(0, 0, 1, 2, pMap, 4, 3, pOutBuffer, 12);

            Assert.AreEqual(3, result);
            Assert.AreEqual(1, pOutBuffer[0]);
            Assert.AreEqual(5, pOutBuffer[1]);
            Assert.AreEqual(9, pOutBuffer[2]);
        }

        [TestMethod]
        public void TestExample2()
        {
            var pMap = new char[] { (char)0, (char)0, (char)1, (char)0, (char)1, (char)1, (char)1, (char)0, (char)1 };
            var pOutBuffer = new int[7];
            var result = Solution.FindPath(2, 0, 0, 2, pMap, 3, 3, pOutBuffer, 7);

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void TestCustom1()
        {
            var pMap = Enumerable.Range(0, 225).Select(i => (char)1).ToArray();
            var pOutBuffer = new int[200];
            var result = Solution.FindPath(0, 0, 14, 14, pMap, 15, 15, pOutBuffer, 200);

            Assert.AreEqual(28, result);
        }

        [TestMethod]
        public void TestCustom2()
        {
            var pMap = Enumerable.Range(0, 1000000).Select(i => (char)1).ToArray();
            var pOutBuffer = new int[1000000];
            var result = Solution.FindPath(0, 0, 999, 999, pMap, 1000, 1000, pOutBuffer, 1000000);

            Assert.AreEqual(1998, result);
        }
    }
}
