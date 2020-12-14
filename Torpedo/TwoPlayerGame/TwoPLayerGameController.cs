using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Torpedo.TwoPlayerGame
{
    class TwoPLayerGameController : ITwoPlayerGameController
    {
        protected ITwoPLayerGameModel Model;
        protected ITwoPlayerGameView View;
        protected GameResult resultsWindow;
        protected TwoPlayerStatsWindow statsWindow;
        ManualResetEvent man = new ManualResetEvent(false);
        public TwoPLayerGameController(ITwoPLayerGameModel twoPlayerGameModel, ITwoPlayerGameView twoPlayerGameView)
        {
            Model = twoPlayerGameModel;
            View = twoPlayerGameView;

        }
        public TwoPLayerGameController()
        {

        }

        public void GridVisibilitySet(bool isPlayerOne)
        {
            if (isPlayerOne)
            {
                View.P1Grid.Visibility = Visibility.Visible;
                View.P1GGrid.Visibility = Visibility.Visible;

                View.P2Grid.Visibility = Visibility.Hidden;
                View.P2GGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                View.P1Grid.Visibility = Visibility.Hidden;
                View.P1GGrid.Visibility = Visibility.Hidden;

                View.P2Grid.Visibility = Visibility.Visible;
                View.P2GGrid.Visibility = Visibility.Visible;
            }
        }


        public void FillShipCoords(Grid gridToBuildOn)
        {
            int indexHelper = 0;
            if (gridToBuildOn.Name == View.P1Grid.Name)
            {
                GridVisibilitySet(true);

                for (int i = Model.ShipSizeP1; i > 0; i--)
                {
                    indexHelper += i;
                }

                if (Model.shipButtonListP1.Count == indexHelper)
                {
                    for (int i = 0; i < Model.ShipSizeP1; i++)
                    {
                        int index = indexHelper - Model.ShipSizeP1 + i;
                        int coordx = (int)Model.shipButtonListP1.ElementAt(index).GetValue(Grid.RowProperty);
                        int coordy = (int)Model.shipButtonListP1.ElementAt(index).GetValue(Grid.ColumnProperty);
                        int[] coords = { coordx, coordy };
                        Model.ShipsP1.ElementAt(Model.ShipSizeP1 - 1).coordinates.Add(coords);
                    }
                    Model.ShipSizeP1++;
                    if (Model.shipButtonListP1.Count() == 15)
                    {
                        View.DisableGridButtons(View.P1Grid);
                        if (Model.P2Name == "AI")
                        {
                            View.P2Grid.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            GridVisibilitySet(false);
                        }
                    }
                }

            }
            if (gridToBuildOn.Name == View.P2Grid.Name)
            {
                if (Model.P2Name == "AI")
                {
                    View.P2Grid.Visibility = Visibility.Hidden;
                }
                else
                {
                    GridVisibilitySet(false);
                }

                for (int i = Model.ShipSizeP2; i > 0; i--)
                {
                    indexHelper += i;
                }

                if (Model.shipButtonListP2.Count == indexHelper)
                {
                    for (int i = 0; i < Model.ShipSizeP2; i++)
                    {
                        int index = indexHelper - Model.ShipSizeP2 + i;
                        int coordx = (int)Model.shipButtonListP2.ElementAt(index).GetValue(Grid.RowProperty);
                        int coordy = (int)Model.shipButtonListP2.ElementAt(index).GetValue(Grid.ColumnProperty);
                        int[] coords = { coordx, coordy };
                        Model.ShipsP2.ElementAt(Model.ShipSizeP2 - 1).coordinates.Add(coords);
                    }
                    Model.ShipSizeP2++;
                    if (Model.shipButtonListP2.Count() == 15)
                    {
                        View.DisableGridButtons(View.P2Grid);

                        GridVisibilitySet(true);
                    }
                }

            }
        }

        public bool CanGenerateShipCoordsByPlayerSelection(int direction, Button clickedBtn, int shipSize, List<int[]> shipLocNumbers)
        {
            int locForWallDet = ((int)clickedBtn.GetValue(Grid.RowProperty) - 1) * 10
                + ((int)clickedBtn.GetValue(Grid.ColumnProperty) - 1);


            int[] lockForShipDet = { 0, 0 };
            lockForShipDet[0] = (int)clickedBtn.GetValue(Grid.RowProperty);
            lockForShipDet[1] = (int)clickedBtn.GetValue(Grid.ColumnProperty);


            if (shipLocNumbers.Count != 1 && shipLocNumbers.Count != 2 && shipLocNumbers.Count != 4 && shipLocNumbers.Count != 7 && shipLocNumbers.Count != 11)
            {
                int prevLockForNonNormal = (shipLocNumbers.ElementAt(shipLocNumbers.Count - 2)[0] - 1) * 10 +
                        shipLocNumbers.ElementAt(shipLocNumbers.Count - 2)[1] - 1;

                if (NonNormalLookingShipDetector(direction, locForWallDet, prevLockForNonNormal))
                    return false;
            }

            if (WallDetector(direction, locForWallDet, shipSize))
                return false;
            if (ShipDetector(direction, lockForShipDet, shipSize, shipLocNumbers))
                return false;


            return true;
        }

        public List<Button> GenerateShipCoordsByPlayerSelection(Button clickedButton, int size, int direction, Grid grid)
        {
            List<Button> toPress = new List<Button>();
            if (direction == 1)
            {
                for (int i = 1; i < size; i++)
                {
                    var button = grid.Children.OfType<Button>().Where(btn =>
                     (int)btn.GetValue(Grid.RowProperty) == (int)clickedButton.GetValue(Grid.RowProperty) - i &&
                     (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedButton.GetValue(Grid.ColumnProperty));
                    toPress.Add(button.First());

                }
            }
            if (direction == 2)
            {
                for (int i = 1; i < size; i++)
                {
                    toPress.Add((Button)grid.Children.OfType<Button>().Where(btn =>
                     (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedButton.GetValue(Grid.ColumnProperty) + i &&
                     (int)btn.GetValue(Grid.RowProperty) == (int)clickedButton.GetValue(Grid.RowProperty)).First());
                }
            }
            if (direction == 3)
            {
                for (int i = 1; i < size; i++)
                {
                    toPress.Add((Button)grid.Children.OfType<Button>().Where(btn =>
                     (int)btn.GetValue(Grid.RowProperty) == (int)clickedButton.GetValue(Grid.RowProperty) + i &&
                     (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedButton.GetValue(Grid.ColumnProperty)).First());
                }
            }
            if (direction == 4)
            {
                for (int i = 1; i < size; i++)
                {
                    toPress.Add((Button)grid.Children.OfType<Button>().Where(btn =>
                      (int)btn.GetValue(Grid.ColumnProperty) == (int)clickedButton.GetValue(Grid.ColumnProperty) - i &&
                     (int)btn.GetValue(Grid.RowProperty) == (int)clickedButton.GetValue(Grid.RowProperty)).First());

                }
            }
            return toPress;
        }

        public bool NonNormalLookingShipDetector(int direction, int currentLocation, int previousLocation)
        {

            if (direction == 1)
            {
                if (currentLocation == previousLocation - 10)
                    return false;
            }
            if (direction == 2)
            {
                if (currentLocation == previousLocation + 1)
                {
                    return false;
                }
            }
            if (direction == 3)
            {
                if (currentLocation == previousLocation + 10)
                    return false;
            }
            if (direction == 4)
            {
                if (currentLocation == previousLocation - 1)
                {
                    return false;
                }
            }
            return true;
        }

        public bool WallDetector(int direction, int location, int size)
        {
            size--;
            if (direction == 1)
            {
                if (location - 10 * size >= 0)
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
                if (location + 10 * size <= 99)
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

        public bool ShipDetector(int direction, int[] location, int size, List<int[]> shipLocNumbers)
        {

            int[] locToCheck = location;
            if (direction == 1)
            {
                for (int i = 1; i < size; i++)
                {
                    locToCheck[0] -= 1;
                    if (shipLocNumbers.Exists(num => num[0] == locToCheck[0] && num[1] == locToCheck[1]))
                    {
                        return true;
                    }
                }
            }
            if (direction == 2)
            {
                for (int i = 1; i < size; i++)
                {
                    locToCheck[1] += 1;
                    if (shipLocNumbers.Exists(num => num[0] == locToCheck[0] && num[1] == locToCheck[1]))
                    {
                        return true;
                    }
                }
            }
            if (direction == 3)
            {
                for (int i = 1; i < size; i++)
                {
                    locToCheck[0] += 1;
                    if (shipLocNumbers.Exists(num => num[0] == locToCheck[0] && num[1] == locToCheck[1]))
                    {
                        return true;
                    }
                }
            }
            if (direction == 4)
            {
                for (int i = 1; i < size; i++)
                {
                    locToCheck[1] -= 1;
                    if (shipLocNumbers.Exists(num => num[0] == locToCheck[0] && num[1] == locToCheck[1]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }




        public void BtnEvent(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            var originalBGColor = clickedButton.Background;
            clickedButton.Background = new SolidColorBrush(Color.FromRgb(80, 154, 159));
            clickedButton.Click -= new RoutedEventHandler(BtnEvent);
            int _row = (int)clickedButton.GetValue(Grid.RowProperty);
            int _column = (int)clickedButton.GetValue(Grid.ColumnProperty);
            var btnsGrid = VisualTreeHelper.GetParent(clickedButton) as Grid;

            Debug.WriteLine(_row + " " + _column + "  " + btnsGrid.Name);

            if (btnsGrid.Name == View.P1Grid.Name)
            {
                Model.shipButtonListP1.Add(clickedButton);

                int coordx = (int)clickedButton.GetValue(Grid.RowProperty);
                int coordy = (int)clickedButton.GetValue(Grid.ColumnProperty);
                int[] coords = { coordx, coordy };
                Model.shipsCoordsListP1.Add(coords);

                if (CanGenerateShipCoordsByPlayerSelection(Model.UserSelectedDirection, clickedButton,
                    Model.ShipSizeP1, Model.shipsCoordsListP1) == false)
                {
                    Model.shipButtonListP1.RemoveAt(Model.shipButtonListP1.Count - 1);
                    Model.shipsCoordsListP1.RemoveAt(Model.shipsCoordsListP1.Count - 1);
                    clickedButton.Background = originalBGColor;
                    clickedButton.Click += new RoutedEventHandler(BtnEvent);
                }
                else
                {
                    var toPress = GenerateShipCoordsByPlayerSelection(clickedButton, Model.ShipSizeP1, Model.UserSelectedDirection, btnsGrid);
                    foreach (var button in toPress)
                    {
                        button.Background = new SolidColorBrush(Color.FromRgb(80, 154, 159));
                        button.Click -= new RoutedEventHandler(BtnEvent);
                        Model.shipButtonListP1.Add(button);
                        coordx = (int)button.GetValue(Grid.RowProperty);
                        coordy = (int)button.GetValue(Grid.ColumnProperty);
                        int[] crds = { coordx, coordy };
                        Model.shipsCoordsListP1.Add(crds);
                    }
                    FillShipCoords(btnsGrid);
                }
            }

            if (btnsGrid.Name == View.P2Grid.Name)
            {
                Model.shipButtonListP2.Add(clickedButton);

                if (Model.P2Name != "AI")
                {
                    int coordx = (int)clickedButton.GetValue(Grid.RowProperty);
                    int coordy = (int)clickedButton.GetValue(Grid.ColumnProperty);
                    int[] coords = { coordx, coordy };
                    Model.shipsCoordsListP2.Add(coords);

                    if (CanGenerateShipCoordsByPlayerSelection(Model.UserSelectedDirection, clickedButton,
                    Model.ShipSizeP2, Model.shipsCoordsListP2) == false)
                    {
                        Model.shipButtonListP2.RemoveAt(Model.shipButtonListP2.Count - 1);
                        Model.shipsCoordsListP2.RemoveAt(Model.shipsCoordsListP2.Count - 1);
                        clickedButton.Background = originalBGColor;
                        clickedButton.Click += new RoutedEventHandler(BtnEvent);
                    }
                    else
                    {
                        var toPress = GenerateShipCoordsByPlayerSelection(clickedButton, Model.ShipSizeP2, Model.UserSelectedDirection, btnsGrid);
                        foreach (var button in toPress)
                        {
                            button.Background = new SolidColorBrush(Color.FromRgb(80, 154, 159));
                            button.Click -= new RoutedEventHandler(BtnEvent);
                            Model.shipButtonListP2.Add(button);
                            coordx = (int)button.GetValue(Grid.RowProperty);
                            coordy = (int)button.GetValue(Grid.ColumnProperty);
                            int[] crds = { coordx, coordy };
                            Model.shipsCoordsListP2.Add(crds);
                        }
                        FillShipCoords(btnsGrid);

                    }
                }
                else
                    FillShipCoords(btnsGrid);
            }

            if (btnsGrid.Name == View.P1GGrid.Name)
            {
                CheckHit(clickedButton);
                statsWindow.NextStep(Model.NextPlayerName);

            }

            if (btnsGrid.Name == View.P2GGrid.Name)
            {
                CheckHit(clickedButton);
                statsWindow.NextStep(Model.NextPlayerName);

            }
            UpdateGameState();
        }

        public virtual void UpdateGameState()
        {

            if (Model.ShipsP1.TrueForAll(ship => ship.isDead == true) || Model.ShipsP2.TrueForAll(ship => ship.isDead == true))
            {
                View.DisableGridButtons(View.P1Grid);
                View.DisableGridButtons(View.P1Grid);
                View.DisableGridButtons(View.P2GGrid);
                View.DisableGridButtons(View.P2GGrid);

                if (Model.ShipsP1.TrueForAll(ship => ship.isDead == true))
                {
                    Debug.WriteLine("P2 won!");
                    resultsWindow.WriteJson(Model.P2Name, Model.P1Name, statsWindow.roundCounter, statsWindow.p1HitCount, statsWindow.p2HitCount, Model.ShipsP1, Model.ShipsP2);
                    resultsWindow.FillDataGridWithResult();

                    resultsWindow.Show();
                    statsWindow.Close();
                    View.CloseWindow();
                }
                else
                {
                    Debug.WriteLine("P1 won!");
                    resultsWindow.WriteJson(Model.P1Name, Model.P2Name, statsWindow.roundCounter, statsWindow.p1HitCount, statsWindow.p2HitCount, Model.ShipsP1, Model.ShipsP2);
                    resultsWindow.FillDataGridWithResult();

                    resultsWindow.Show();
                    statsWindow.Close();
                    View.CloseWindow();
                }
                return;
            }
            if (Model.ShipSizeP1 == 6 && Model.ShipSizeP2 == 6)
            {
                if (Model.Turn)
                {
                    View.ReEnableNotClickedGridButtons(View.P1GGrid);
                    View.DisableGridButtons(View.P2GGrid);
                }
                else
                {
                    View.ReEnableNotClickedGridButtons(View.P2GGrid);
                    View.DisableGridButtons(View.P1GGrid);
                }
                Model.Turn = !Model.Turn;
            }
        }

        public virtual void CheckHit(Button btnToCheck)
        {
            statsWindow.IncRound();
            if (Model.NextPlayerName == Model.P2Name)
            {
                Model.NextPlayerName = Model.P1Name;
            }
            else if (Model.NextPlayerName == Model.P1Name)
            {
                Model.NextPlayerName = Model.P2Name;
            }
            statsWindow.NextStep(Model.NextPlayerName);

            if (!Model.Turn)
            {
                GridVisibilitySet(false);

                foreach (Ship ship in Model.ShipsP2)
                {
                    int[] matchedCoords = ship.coordinates.FirstOrDefault(coords => (coords[0] == (int)btnToCheck.GetValue(Grid.RowProperty) &&
                     coords[1] == (int)btnToCheck.GetValue(Grid.ColumnProperty)));

                    if (matchedCoords != null)
                    {
                        Debug.WriteLine("P2's ship got hit");
                        ship.hits++;

                        View.MakeShipPartHit(btnToCheck, 2);

                        statsWindow.IncP1HitCount();
                        if (ship.hits >= ship.Length)
                        {
                            Debug.WriteLine($"P2's {ship.Length} size ship is dead");
                            ship.isDead = true;
                            statsWindow.ListP2ShipStats(Model.ShipsP2);
                            View.MakeShipLookDead(ship, 2);
                        }
                        return;
                    }
                }

                Button buttonOnEnemyGrid = View.P2Grid.Children.OfType<Button>().Where(btn =>
                    (int)btn.GetValue(Grid.RowProperty) == (int)btnToCheck.GetValue(Grid.RowProperty) &&
                     (int)btn.GetValue(Grid.ColumnProperty) == (int)btnToCheck.GetValue(Grid.ColumnProperty)).First();
                buttonOnEnemyGrid.Background = new SolidColorBrush(Color.FromRgb(100, 100, 100));

                Debug.WriteLine("No hit on P2's ships");
            }

            else
            {
                if (Model.P2Name == "AI")
                {
                    View.P2Grid.Visibility = Visibility.Hidden;
                    View.P2GGrid.Visibility = Visibility.Hidden;
                }
                else
                {
                    GridVisibilitySet(true);
                }
                foreach (Ship ship in Model.ShipsP1)
                {
                    int[] matchedCoords = ship.coordinates.FirstOrDefault(coords => (coords[0] == (int)btnToCheck.GetValue(Grid.RowProperty) &&
                     coords[1] == (int)btnToCheck.GetValue(Grid.ColumnProperty)));

                    if (matchedCoords != null)
                    {
                        Debug.WriteLine("P1's ship got hit");
                        ship.hits++;

                        View.MakeShipPartHit(btnToCheck, 1);

                        statsWindow.IncP2HitCount();
                        if (ship.hits >= ship.Length)
                        {
                            Debug.WriteLine($"P1's {ship.Length} size ship is dead");
                            ship.isDead = true;
                            statsWindow.ListP1ShipStats(Model.ShipsP1);
                            View.MakeShipLookDead(ship, 1);
                        }
                        return;
                    }
                }

                Button buttonOnEnemyGrid = View.P1Grid.Children.OfType<Button>().Where(btn =>
                        (int)btn.GetValue(Grid.RowProperty) == (int)btnToCheck.GetValue(Grid.RowProperty) &&
                         (int)btn.GetValue(Grid.ColumnProperty) == (int)btnToCheck.GetValue(Grid.ColumnProperty)).First();
                buttonOnEnemyGrid.Background = new SolidColorBrush(Color.FromRgb(100, 100, 100));
                Debug.WriteLine("No hit on P1's ships");
            }
        }

        public void StartGameSate()
        {
            resultsWindow = new GameResult();
            statsWindow = new TwoPlayerStatsWindow(Model.P1Name, Model.P2Name);
            statsWindow.Show();
            var rand = new Random();

            if (rand.Next(2) == 1)
                Model.Turn = true;
            else
                Model.Turn = false;

            if (Model.Turn)
            {
                Model.NextPlayerName = Model.P1Name;
                GridVisibilitySet(true);
            }
            else if (Model.P2Name == "AI")
            {
                View.P1Grid.Visibility = Visibility.Visible;
                View.P2Grid.Visibility = Visibility.Hidden;
                View.P2GGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                Model.NextPlayerName = Model.P2Name;
                GridVisibilitySet(false);

            }


            View.DisableGridButtons(View.P1GGrid);
            View.DisableGridButtons(View.P2GGrid);
        }

        public void PlayerGiveUp()
        {
            View.DisableGridButtons(View.P1Grid);
            View.DisableGridButtons(View.P1Grid);
            View.DisableGridButtons(View.P2GGrid);
            View.DisableGridButtons(View.P2GGrid);

            if (Model.NextPlayerName == Model.P1Name)
            {


                Debug.WriteLine("P2 won!");
                resultsWindow.WriteJson(Model.P2Name, Model.P1Name, statsWindow.roundCounter, statsWindow.p1HitCount, statsWindow.p2HitCount, Model.ShipsP1, Model.ShipsP2);
                resultsWindow.FillDataGridWithResult();

            }
            else if (Model.NextPlayerName == Model.P2Name)
            {
                Debug.WriteLine("P1 won!");
                resultsWindow.WriteJson(Model.P1Name, Model.P2Name, statsWindow.roundCounter, statsWindow.p1HitCount, statsWindow.p2HitCount, Model.ShipsP1, Model.ShipsP2);
                resultsWindow.FillDataGridWithResult();


            }

            resultsWindow.Show();
            statsWindow.Close();
            View.CloseWindow();
        }

        public void SetModel(ITwoPLayerGameModel paramModel)
        {
            this.Model = paramModel;
        }

        public void SetView(ITwoPlayerGameView paramView)
        {
            this.View = paramView;
        }

        public virtual void BuildShipsByAICords()
        {
            throw new NotImplementedException();
        }
    }

}
