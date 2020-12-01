using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Torpedo.OnePlayerGame
{
    class AI
    {
        Random random = new Random();
        List<int> shipLocNumbers = new List<int>();
        int dir = 0;
        Button clickedBtn;
        Button firstHitOnShip;
        bool foundShipFlag = false;
        public enum Hitlevel
        {
            NoHit,
            Hit,
            Sunk
        }

        Hitlevel hitlevel = new Hitlevel();

        public void InformAiAboutMove(Hitlevel passedHitLevel)
        {
            hitlevel = passedHitLevel;
        }

        public Button MakeAiMove(List<Button> clickableBtns)
        {

            if (Hitlevel.Sunk==hitlevel)
            {
                dir = random.Next(1, 5);
                random.Next(clickableBtns.Count);
                clickedBtn = clickableBtns.ElementAt(random.Next(clickableBtns.Count));
                foundShipFlag = false;
                return clickedBtn;
            }
            if (Hitlevel.Hit == hitlevel)
            {
                if (foundShipFlag == false)
                {
                    firstHitOnShip = clickedBtn;
                }

                if (dir == 1)
                {
                    if(clickableBtns.FirstOrDefault(btn => (int)btn.GetValue(Grid.RowProperty) == (int)clickedBtn.GetValue(Grid.RowProperty) - 1
                    && (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedBtn.GetValue(Grid.ColumnProperty))!=null)
                    {
                        clickedBtn = clickableBtns.FirstOrDefault(btn => (int)btn.GetValue(Grid.RowProperty) == (int)clickedBtn.GetValue(Grid.RowProperty) - 1
                            && (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedBtn.GetValue(Grid.ColumnProperty) );
                    }
                    else
                    {
                        dir = random.Next(1, 5);
                        MakeAiMove(clickableBtns);
                    }
                }
                if (dir == 2)
                {
                    if (clickableBtns.FirstOrDefault(btn => (int)btn.GetValue(Grid.RowProperty) == (int)clickedBtn.GetValue(Grid.RowProperty)
                    && (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedBtn.GetValue(Grid.ColumnProperty) +1) != null)
                    {
                        clickedBtn = clickableBtns.FirstOrDefault(btn => (int)btn.GetValue(Grid.RowProperty) == (int)clickedBtn.GetValue(Grid.RowProperty)
                            && (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedBtn.GetValue(Grid.ColumnProperty) + 1);
                    }
                    else
                    {
                        dir = random.Next(1, 5);
                        MakeAiMove(clickableBtns);
                    }
                }
                if (dir == 3)
                {
                    if (clickableBtns.FirstOrDefault(btn => (int)btn.GetValue(Grid.RowProperty) == (int)clickedBtn.GetValue(Grid.RowProperty) + 1
                     && (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedBtn.GetValue(Grid.ColumnProperty) ) != null)
                    {
                        clickedBtn = clickableBtns.FirstOrDefault(btn => (int)btn.GetValue(Grid.RowProperty) == (int)clickedBtn.GetValue(Grid.RowProperty) + 1
                            && (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedBtn.GetValue(Grid.ColumnProperty) );
                    }
                    else
                    {
                        dir = random.Next(1, 5);
                        MakeAiMove(clickableBtns);
                    }
                }
                if (dir == 4)
                {
                    if (clickableBtns.FirstOrDefault(btn => (int)btn.GetValue(Grid.RowProperty) == (int)clickedBtn.GetValue(Grid.RowProperty)
                    && (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedBtn.GetValue(Grid.ColumnProperty)-1) != null)
                    {
                        clickedBtn = clickableBtns.FirstOrDefault(btn => (int)btn.GetValue(Grid.RowProperty) == (int)clickedBtn.GetValue(Grid.RowProperty) 
                            && (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedBtn.GetValue(Grid.ColumnProperty)-1);
                    }
                    else
                    {
                        dir = random.Next(1, 5);
                        MakeAiMove(clickableBtns);
                    }
                }
                foundShipFlag = true;
                return clickedBtn;
            }

            if(Hitlevel.NoHit==hitlevel)
            {
                if (foundShipFlag)
                {
                    dir = random.Next(1, 5);
                    clickedBtn = firstHitOnShip;
                    hitlevel = Hitlevel.Hit;
                    MakeAiMove(clickableBtns);
                }
                else
                {
                    dir = random.Next(1, 5);
                    random.Next(clickableBtns.Count);
                    clickedBtn = clickableBtns.ElementAt(random.Next(clickableBtns.Count));
                    foundShipFlag = false;
                    return clickedBtn;
                }
            }
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
            if(direction==4)
            {
                if (location / 10 == (location - size) / 10 && location-size>=0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool ShipDetector(int direction, int location, int size)
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
        public List<Button> GenerateShipsByAi(List<Button> passedBtns)
        {
            int shipsize = 0;
            List<Button> btnsToPress = new List<Button>();
            while (true)
            {
                shipsize++;
                if (shipsize == 6)
                {
                    return btnsToPress;
                }
                int randValue = random.Next(passedBtns.Count);
                int direction = random.Next(1, 5);
                
                while (shipLocNumbers.Exists(num => num==randValue) ||
                    ShipDetector(direction, randValue, shipsize)
                        || WallDetector(direction, randValue, shipsize))
                {
                    randValue = random.Next(passedBtns.Count);
                    direction = random.Next(1, 5);
                }
                shipLocNumbers.Add(randValue);

                Button btn = passedBtns.ElementAt(randValue);
                //passedBtns.RemoveAt(randValue);
                //onePlayerGameState.btnEvent(btn, new RoutedEventArgs(ButtonBase.ClickEvent));
                btnsToPress.Add(btn);
                if (shipLocNumbers.Count == 1)
                    continue;
                if (shipsize<=5)
                {

                    if (direction == 1)
                    {
                        for (int i = 1; i < shipsize; i++)
                        {
                            Button btnOthers = passedBtns.ElementAt(randValue-10*i);
                            //onePlayerGameState.btnEvent(btnOthers, new RoutedEventArgs(ButtonBase.ClickEvent));
                            btnsToPress.Add(btnOthers);
                            shipLocNumbers.Add(randValue - 10 * i);

                        }
                    }
                    if (direction == 2)
                    {
                        for (int i = 1; i < shipsize; i++)
                        {
                            Button btnOthers = passedBtns.ElementAt(randValue+i);
                            //onePlayerGameState.btnEvent(btnOthers, new RoutedEventArgs(ButtonBase.ClickEvent));
                            btnsToPress.Add(btnOthers);
                            shipLocNumbers.Add(randValue + i);

                        }
                    }
                    if (direction == 3)
                    {
                        for (int i = 1; i < shipsize; i++)
                        {
                            Button btnOthers = passedBtns.ElementAt(randValue + 10 * i);
                            //onePlayerGameState.btnEvent(btnOthers, new RoutedEventArgs(ButtonBase.ClickEvent));
                            btnsToPress.Add(btnOthers);
                            shipLocNumbers.Add(randValue + 10 * i);

                        }
                    }
                    if (direction == 4)
                    {
                        for (int i = 1; i < shipsize; i++)
                        {
                            Button btnOthers = passedBtns.ElementAt(randValue -  i);
                            //onePlayerGameState.btnEvent(btnOthers, new RoutedEventArgs(ButtonBase.ClickEvent));
                            btnsToPress.Add(btnOthers);
                            shipLocNumbers.Add(randValue - i);

                        }
                    }
                }
            }            
        }
    }
}
