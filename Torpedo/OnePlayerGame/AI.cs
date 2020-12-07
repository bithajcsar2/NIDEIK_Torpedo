using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;

namespace Torpedo.OnePlayerGame
{
    public class AI
    {
        Random random = new Random();
        int dir = 0;
        Button clickedBtn;
        bool[] canMoveInDirs = { true, true, true, true };
        int radiusFromFirstHit = 1;
        List<Button> hitsByAi;
        public enum PrevHitLevel
        {
            NoHit,
            Hit,
            Sunk
        }

        PrevHitLevel hitlevel = new PrevHitLevel();

        public void InformAiAboutMove(PrevHitLevel passedHitLevel, ref List<Button> hitsByAi)
        {
            hitlevel = passedHitLevel;
            this.hitsByAi = hitsByAi;
        }

        public Button MakeAiMove(List<Button> clickableBtns)
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
                        if (clickableBtns.FirstOrDefault(btn => (int)btn.GetValue(Grid.RowProperty) == (int)clickedBtn.GetValue(Grid.RowProperty) - radiusFromFirstHit
                         && (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedBtn.GetValue(Grid.ColumnProperty)) != null)
                        {
                            clickedBtn = clickableBtns.FirstOrDefault(btn => (int)btn.GetValue(Grid.RowProperty) == (int)clickedBtn.GetValue(Grid.RowProperty) - radiusFromFirstHit
                                && (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedBtn.GetValue(Grid.ColumnProperty));
                            canMoveInDirs[dir - 1] = true;
                            return clickedBtn;
                        }
                        else
                        {
                            dir = random.Next(1, 5);
                            canMoveInDirs[dir - 1] = false;
                            continue;
                        }
                    }
                    if (dir == 2)
                    {
                        if (clickableBtns.FirstOrDefault(btn => (int)btn.GetValue(Grid.RowProperty) == (int)clickedBtn.GetValue(Grid.RowProperty)
                        && (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedBtn.GetValue(Grid.ColumnProperty) + radiusFromFirstHit) != null)
                        {
                            clickedBtn = clickableBtns.FirstOrDefault(btn => (int)btn.GetValue(Grid.RowProperty) == (int)clickedBtn.GetValue(Grid.RowProperty)
                                && (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedBtn.GetValue(Grid.ColumnProperty) + radiusFromFirstHit);
                            canMoveInDirs[dir - 1] = true;
                            return clickedBtn;
                        }
                        else
                        {
                            dir = random.Next(1, 5);
                            canMoveInDirs[dir - 1] = false;
                            continue;
                        }
                    }
                    if (dir == 3)
                    {
                        if (clickableBtns.FirstOrDefault(btn => (int)btn.GetValue(Grid.RowProperty) == (int)clickedBtn.GetValue(Grid.RowProperty) + radiusFromFirstHit
                         && (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedBtn.GetValue(Grid.ColumnProperty)) != null)
                        {
                            clickedBtn = clickableBtns.FirstOrDefault(btn => (int)btn.GetValue(Grid.RowProperty) == (int)clickedBtn.GetValue(Grid.RowProperty) + radiusFromFirstHit
                                && (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedBtn.GetValue(Grid.ColumnProperty));
                            canMoveInDirs[dir - 1] = true;
                            return clickedBtn;

                        }
                        else
                        {
                            dir = random.Next(1, 5);
                            canMoveInDirs[dir - 1] = false;
                            continue;
                        }
                    }
                    if (dir == 4)
                    {
                        if (clickableBtns.FirstOrDefault(btn => (int)btn.GetValue(Grid.RowProperty) == (int)clickedBtn.GetValue(Grid.RowProperty)
                        && (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedBtn.GetValue(Grid.ColumnProperty) - radiusFromFirstHit) != null)
                        {
                            clickedBtn = clickableBtns.FirstOrDefault(btn => (int)btn.GetValue(Grid.RowProperty) == (int)clickedBtn.GetValue(Grid.RowProperty)
                                && (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedBtn.GetValue(Grid.ColumnProperty) - radiusFromFirstHit);
                            canMoveInDirs[dir - 1] = true;
                            return clickedBtn;
                        }
                        else
                        {
                            dir = random.Next(1, 5);
                            canMoveInDirs[dir - 1] = false;
                            continue;
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
                    clickedBtn = hitsByAi.ElementAt(0);
                    MakeAiMove(clickableBtns);
                }

                else
                {
                    dir = random.Next(1, 5);
                    random.Next(clickableBtns.Count);
                    clickedBtn = clickableBtns.ElementAt(random.Next(clickableBtns.Count));
                    return clickedBtn;
                }

            }

            /*if (Hitlevel.NoHit==hitlevel)
            {
                if (hitsByAi.Count>0)
                {
                    dir = random.Next(1, 5);
                    clickedBtn = hitsByAi.ElementAt(0);
                    hitlevel = Hitlevel.Hit;
                    MakeAiMove(clickableBtns);
                }
                else
                {
                    dir = random.Next(1, 5);
                    random.Next(clickableBtns.Count);
                    clickedBtn = clickableBtns.ElementAt(random.Next(clickableBtns.Count));
                    return clickedBtn;
                }
            }*/
            return clickedBtn;
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
            if(direction == 4)
            {
                if (location / 10 == (location - size) / 10 && location-size>=0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool ShipDetector(int direction, int location, int size, List<int> shipLocNumbers)
        {

            int locToCheck=0;
            if (direction == 1)
            {
                for (int i = 0; i < size; i++)
                {
                    locToCheck = location - 10 * i;
                    if(shipLocNumbers.Exists(num => num == locToCheck))
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
                
                while (shipLocNumbers.Exists(num => num==randLocValue) ||
                    ShipDetector(direction, randLocValue, shipsize, shipLocNumbers)
                        || WallDetector(direction, randLocValue, shipsize))
                {
                    randLocValue = random.Next(boardSize);
                    direction = random.Next(1, 5);
                }
                shipLocNumbers.Add(randLocValue);

                //Button shipFirstBtn = passedBtns.ElementAt(randLocValue);
                //passedBtns.RemoveAt(randValue);
                //onePlayerGameState.btnEvent(btn, new RoutedEventArgs(ButtonBase.ClickEvent));
                //btnsToPress.Add(shipFirstBtn);
                if (shipLocNumbers.Count == 1)
                    continue;
                if (shipsize<=5)
                {

                    if (direction == 1)
                    {
                        for (int i = 1; i < shipsize; i++)
                        {
                            //Button btnOthers = passedBtns.ElementAt(randLocValue-10*i);
                            //onePlayerGameState.btnEvent(btnOthers, new RoutedEventArgs(ButtonBase.ClickEvent));
                            //btnsToPress.Add(btnOthers);
                            shipLocNumbers.Add(randLocValue - 10 * i);

                        }
                    }
                    if (direction == 2)
                    {
                        for (int i = 1; i < shipsize; i++)
                        {
                            //Button btnOthers = passedBtns.ElementAt(randLocValue+i);
                            //onePlayerGameState.btnEvent(btnOthers, new RoutedEventArgs(ButtonBase.ClickEvent));
                           // btnsToPress.Add(btnOthers);
                            shipLocNumbers.Add(randLocValue + i);

                        }
                    }
                    if (direction == 3)
                    {
                        for (int i = 1; i < shipsize; i++)
                        {
                            //Button btnOthers = passedBtns.ElementAt(randLocValue + 10 * i);
                            //onePlayerGameState.btnEvent(btnOthers, new RoutedEventArgs(ButtonBase.ClickEvent));
                            //btnsToPress.Add(btnOthers);
                            shipLocNumbers.Add(randLocValue + 10 * i);

                        }
                    }
                    if (direction == 4)
                    {
                        for (int i = 1; i < shipsize; i++)
                        {
                            //Button btnOthers = passedBtns.ElementAt(randLocValue -  i);
                            //onePlayerGameState.btnEvent(btnOthers, new RoutedEventArgs(ButtonBase.ClickEvent));
                            //btnsToPress.Add(btnOthers);
                            shipLocNumbers.Add(randLocValue - i);

                        }
                    }
                }
            }            
        }
    }
}
