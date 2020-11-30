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
                for (int i = 0; i <= size; i++)
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
                for (int i = 0; i <= size; i++)
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
                for (int i = 0; i <= size; i++)
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
                for (int i = 0; i <= size; i++)
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
        public void BuildShipsByAi(List<Button> passedBtns, ref OnePlayerGameSate onePlayerGameState)
        {
            int shipsize = 0;
            while (onePlayerGameState.shipSizeP2!=6)
            {
                shipsize++;
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
                onePlayerGameState.btnEvent(btn, new RoutedEventArgs(ButtonBase.ClickEvent));
                if (shipLocNumbers.Count == 1)
                    continue;
                if (shipsize<=5)
                {

                    if (direction == 1)
                    {
                        for (int i = 1; i < shipsize; i++)
                        {
                            Button btnOthers = passedBtns.ElementAt(randValue-10*i);
                            onePlayerGameState.btnEvent(btnOthers, new RoutedEventArgs(ButtonBase.ClickEvent));
                            shipLocNumbers.Add(randValue - 10 * i);

                        }
                    }
                    if (direction == 2)
                    {
                        for (int i = 1; i < shipsize; i++)
                        {
                            Button btnOthers = passedBtns.ElementAt(randValue+i);
                            onePlayerGameState.btnEvent(btnOthers, new RoutedEventArgs(ButtonBase.ClickEvent));
                            shipLocNumbers.Add(randValue + i);

                        }
                    }
                    if (direction == 3)
                    {
                        for (int i = 1; i < shipsize; i++)
                        {
                            Button btnOthers = passedBtns.ElementAt(randValue + 10 * i);
                            onePlayerGameState.btnEvent(btnOthers, new RoutedEventArgs(ButtonBase.ClickEvent));
                            shipLocNumbers.Add(randValue + 10 * i);

                        }
                    }
                    if (direction == 4)
                    {
                        for (int i = 1; i < shipsize; i++)
                        {
                            Button btnOthers = passedBtns.ElementAt(randValue -  i);
                            onePlayerGameState.btnEvent(btnOthers, new RoutedEventArgs(ButtonBase.ClickEvent));
                            shipLocNumbers.Add(randValue - i);

                        }
                    }
                }
            }            
        }
    }
}
