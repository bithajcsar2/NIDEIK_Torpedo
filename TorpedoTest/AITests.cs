using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Torpedo.OnePlayerGame;

namespace TorpedoTest
{
    [TestClass]
    public class AITests
    {

        [TestMethod]
        public void WallDetectorTest()
        {
            AI ai = new AI();

            Assert.IsTrue(ai.WallDetector(1, 8, 1));
            Assert.IsTrue(ai.WallDetector(2, 10, 10));
            Assert.IsTrue(ai.WallDetector(3, 9, 9));
            Assert.IsTrue(ai.WallDetector(4, 6, 7));

            Assert.IsFalse(ai.WallDetector(1, 21, 2));
            Assert.IsFalse(ai.WallDetector(2, 10, 0));
            Assert.IsFalse(ai.WallDetector(3, 2, 2));
            Assert.IsFalse(ai.WallDetector(4, 10, 0));
        }

        [TestMethod]
        public void ShipDetectorTest()
        {
            AI ai = new AI();
            List<int> shipLocNumbers = new List<int>() { 10, 25, 50, 97, 98 };

            Assert.IsFalse(ai.ShipDetector(1, 3, 2, shipLocNumbers));
            Assert.IsFalse(ai.ShipDetector(2, 5, 3, shipLocNumbers));
            Assert.IsFalse(ai.ShipDetector(3, 14, 5, shipLocNumbers));
            Assert.IsFalse(ai.ShipDetector(4, 8, 23, shipLocNumbers));

            Assert.IsTrue(ai.ShipDetector(1, 20, 5, shipLocNumbers));
            Assert.IsTrue(ai.ShipDetector(2, 40, 11, shipLocNumbers));
            Assert.IsTrue(ai.ShipDetector(3, 10, 15, shipLocNumbers));
            Assert.IsTrue(ai.ShipDetector(4, 30, 6, shipLocNumbers));
        }
        [TestMethod]
        public void GenerateShipsByAiTest()
        {
            AI ai = new AI();
            for (int i = 0; i < 100; i++)
            {
                var coords = ai.GenerateShipsByAi();
                int actualSize = coords.Count;
                int expectedSize = 15;
                CollectionAssert.AllItemsAreUnique(coords);
                Assert.AreEqual(expectedSize, actualSize);
            }
            
        }
    }
}
