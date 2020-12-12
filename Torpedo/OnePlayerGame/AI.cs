using System;
using System.Collections.Generic;
using System.Linq;

namespace Torpedo.OnePlayerGame
{
    public class AI
    {
        Random random = new Random();
        int dir = 0;
        public int clickedBtnCoord;
        bool[] canMoveInDirs = { true, true, true, true };
        public int radiusFromFirstHit = 1;
        List<int> hitsByAi;
        public enum PrevHitLevel
        {
            NoHit,
            Hit,
            Sunk
        }

        PrevHitLevel hitlevel = new PrevHitLevel();

        public void InformAiAboutMove(PrevHitLevel passedHitLevel, ref List<int> hitsByAi)
        {
            hitlevel = passedHitLevel;
            this.hitsByAi = hitsByAi;
        }

        public int MakeAiMove(List<int> clickableCoords)
        {
            radiusFromFirstHit = 1;
            for (int i = 0; i < canMoveInDirs.Length; i++)
            {
                canMoveInDirs[i] = true;
            }

            if (PrevHitLevel.Hit == hitlevel)
            {
                while (true)
                {
                    if (canMoveInDirs.All(canMoveInDir => canMoveInDir == false))
                    {
                        radiusFromFirstHit++;
                        for (int i = 0; i < canMoveInDirs.Length; i++)
                        {
                            canMoveInDirs[i] = true;
                        }
                    }

                    if (dir == 1)
                    {
                        if (clickableCoords.Exists(coord => coord == clickedBtnCoord - 10 * radiusFromFirstHit))
                        {
                            clickedBtnCoord = clickableCoords.Find(coord => coord == clickedBtnCoord - 10 * radiusFromFirstHit);
                            break;
                        }
                        else
                        {
                            dir = random.Next(1, 5);
                            canMoveInDirs[dir - 1] = false;
                        }
                    }
                    if (dir == 2)
                    {
                        if (clickableCoords.Exists(coord => coord == clickedBtnCoord + radiusFromFirstHit) && WallDetector(dir, clickedBtnCoord, radiusFromFirstHit) == false)
                        {
                            clickedBtnCoord = clickableCoords.Find(coord => coord == clickedBtnCoord + radiusFromFirstHit);
                            break;
                        }
                        else
                        {
                            dir = random.Next(1, 5);
                            canMoveInDirs[dir - 1] = false;
                        }
                    }
                    if (dir == 3)
                    {
                        if (clickableCoords.Exists(coord => coord == clickedBtnCoord + 10 * radiusFromFirstHit))
                        {
                            clickedBtnCoord = clickableCoords.Find(coord => coord == clickedBtnCoord + 10 * radiusFromFirstHit);
                            break;
                        }
                        else
                        {
                            dir = random.Next(1, 5);
                            canMoveInDirs[dir - 1] = false;
                        }
                    }
                    if (dir == 4)
                    {

                        if (clickableCoords.Exists(coord => coord == clickedBtnCoord - radiusFromFirstHit) && WallDetector(dir, clickedBtnCoord, radiusFromFirstHit) == false)
                        {
                            clickedBtnCoord = clickableCoords.Find(coord => coord == clickedBtnCoord - radiusFromFirstHit);
                            break;
                        }
                        else
                        {
                            dir = random.Next(1, 5);
                            canMoveInDirs[dir - 1] = false;

                        }
                    }
                }
            }

            if (PrevHitLevel.Sunk == hitlevel || PrevHitLevel.NoHit == hitlevel)
            {
                if (hitsByAi.Any())
                {
                    dir = random.Next(1, 5);
                    hitlevel = PrevHitLevel.Hit;
                    clickedBtnCoord = hitsByAi.ElementAt(0);
                    MakeAiMove(clickableCoords);
                }

                else
                {
                    dir = random.Next(1, 5);
                    random.Next(clickableCoords.Count);
                    clickedBtnCoord = clickableCoords.ElementAt(random.Next(clickableCoords.Count));
                }

            }


            return clickedBtnCoord;
        }




        public bool WallDetector(int direction, int location, int size)
        {
            if (direction == 1)
            {
                if (location - 10 * size > 0)
                    return false;
            }
            if (direction == 2)
            {
                if (location / 10 == (location + size) / 10)
                {
                    return false;
                }
            }
            if (direction == 3)
            {
                if (location + 10 * size < 99)
                    return false;
            }
            if (direction == 4)
            {
                if (location / 10 == (location - size) / 10 && location - size >= 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool ShipDetector(int direction, int location, int size, List<int> shipLocNumbers)
        {

            int locToCheck = 0;
            if (direction == 1)
            {
                for (int i = 0; i < size; i++)
                {
                    locToCheck = location - 10 * i;
                    if (shipLocNumbers.Exists(num => num == locToCheck))
                    {
                        return true;
                    }
                }
            }
            if (direction == 2)
            {
                for (int i = 0; i < size; i++)
                {
                    locToCheck = location + i;
                    if (shipLocNumbers.Exists(num => num == locToCheck))
                    {
                        return true;
                    }
                }
            }
            if (direction == 3)
            {
                for (int i = 0; i < size; i++)
                {
                    locToCheck = location + 10 * i;
                    if (shipLocNumbers.Exists(num => num == locToCheck))
                    {
                        return true;
                    }
                }
            }
            if (direction == 4)
            {
                for (int i = 0; i < size; i++)
                {
                    locToCheck = location - i;
                    if (shipLocNumbers.Exists(num => num == locToCheck))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public List<int> GenerateShipsByAi()
        {
            int shipsize = 0;
            int boardSize = 100;
            List<int> shipLocNumbers = new List<int>();

            while (true)
            {
                shipsize++;
                if (shipsize == 6)
                {
                    return shipLocNumbers;
                }
                int randLocValue = random.Next(boardSize);
                int direction = random.Next(1, 5);

                while (shipLocNumbers.Exists(num => num == randLocValue) ||
                    ShipDetector(direction, randLocValue, shipsize, shipLocNumbers)
                        || WallDetector(direction, randLocValue, shipsize))
                {
                    randLocValue = random.Next(boardSize);
                    direction = random.Next(1, 5);
                }
                shipLocNumbers.Add(randLocValue);

                if (shipLocNumbers.Count == 1)
                    continue;
                if (shipsize <= 5)
                {

                    if (direction == 1)
                    {
                        for (int i = 1; i < shipsize; i++)
                        {
                            shipLocNumbers.Add(randLocValue - 10 * i);
                        }
                    }
                    if (direction == 2)
                    {
                        for (int i = 1; i < shipsize; i++)
                        {
                            shipLocNumbers.Add(randLocValue + i);
                        }
                    }
                    if (direction == 3)
                    {
                        for (int i = 1; i < shipsize; i++)
                        {
                            shipLocNumbers.Add(randLocValue + 10 * i);
                        }
                    }
                    if (direction == 4)
                    {
                        for (int i = 1; i < shipsize; i++)
                        {
                            shipLocNumbers.Add(randLocValue - i);
                        }
                    }
                }
            }
        }
    }
}
