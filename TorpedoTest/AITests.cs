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
            Assert.IsTrue(ai.WallDetector(3, 9, 10));
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

        [TestMethod]

        public void MakeAiMoveTest()
        {
            int coord;
            AI ai = new AI();
            List<int> hitsByAi = new List<int>();
            
            ai.InformAiAboutMove(AI.PrevHitLevel.NoHit, ref hitsByAi);
            List<int> clickableCoords = new List<int>();
            for (int i = 0; i < 50; i++)
            {
                clickableCoords.Add(i);
            }
            for (int i = 0; i < 100; i++)
            {
                Assert.IsTrue(ai.MakeAiMove(clickableCoords) < 50);
            }

            clickableCoords.Clear();
            for (int i = 0; i < 100; i++)
            {
                if(i!=22)
                    clickableCoords.Add(i);
            }
            hitsByAi.Add(22);
            ai.InformAiAboutMove(AI.PrevHitLevel.Hit, ref hitsByAi);
            for (int i = 0; i < 100; i++)
            {
                ai.clickedBtnCoord = 22;
                coord = ai.MakeAiMove(clickableCoords);
                Assert.IsTrue(coord == 12 || coord == 23 || coord == 32 || coord == 21);
            }
            clickableCoords.Remove(12);
            clickableCoords.Remove(23);
            clickableCoords.Remove(32);
            clickableCoords.Remove(21);

            for (int i = 0; i < 100; i++)
            {
                ai.clickedBtnCoord = 22;
                coord = ai.MakeAiMove(clickableCoords);
                Assert.IsTrue(ai.radiusFromFirstHit == 2);
                Assert.IsTrue(coord == 2 || coord == 24 || coord == 42 || coord == 20);
            }
            
            hitsByAi.Add(77);
            clickableCoords.Remove(77);
            ai.InformAiAboutMove(AI.PrevHitLevel.Sunk, ref hitsByAi);
            hitsByAi.Remove(22);
            coord = ai.MakeAiMove(clickableCoords);
            Assert.IsTrue(ai.radiusFromFirstHit == 1);
            Assert.IsTrue(coord == 67 || coord == 78 || coord == 87 || coord == 76);

            hitsByAi.Clear();
            ai.InformAiAboutMove(AI.PrevHitLevel.Hit, ref hitsByAi);
            for (int i = 0; i < 100; i++)
            {
                ai.clickedBtnCoord = 9;
                coord = ai.MakeAiMove(clickableCoords);
                Assert.IsTrue(ai.radiusFromFirstHit == 1);
                Assert.IsTrue(coord == 8 || coord == 19);
            }

            clickableCoords.Remove(8);
            ai.clickedBtnCoord = 9;
            coord = ai.MakeAiMove(clickableCoords);
            Assert.IsTrue(ai.radiusFromFirstHit == 1);
            Assert.IsTrue(coord == 19);
        }
    }
}
