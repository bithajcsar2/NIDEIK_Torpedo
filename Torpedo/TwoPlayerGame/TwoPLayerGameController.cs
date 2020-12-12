using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public TwoPLayerGameController(ITwoPLayerGameModel twoPlayerGameModel, ITwoPlayerGameView twoPlayerGameView)
        {
            Model = twoPlayerGameModel;
            View = twoPlayerGameView;
            resultsWindow = new GameResult();
            statsWindow = new TwoPlayerStatsWindow(MainWindow.player1Name, MainWindow.player2Name);
        }
        public TwoPLayerGameController()
        {
            
            resultsWindow = new GameResult();
            statsWindow = new TwoPlayerStatsWindow(MainWindow.player1Name, MainWindow.player2Name);
        }

        public void BtnEvent(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            clickedButton.Background = new SolidColorBrush(Color.FromRgb(80, 154, 159));
            clickedButton.Click -= new RoutedEventHandler(BtnEvent);
            int _row = (int)clickedButton.GetValue(Grid.RowProperty);
            int _column = (int)clickedButton.GetValue(Grid.ColumnProperty);
            var btnsGrid = VisualTreeHelper.GetParent(clickedButton) as Grid;

            Debug.WriteLine(_row + " " + _column + "  " + btnsGrid.Name);

            if (btnsGrid.Name == View.P1Grid.Name)
            {
                Model.shipButtonListP1.Add(clickedButton);
                View.BuildShip(btnsGrid);
            }

            if (btnsGrid.Name == View.P2Grid.Name)
            {
                Model.shipButtonListP2.Add(clickedButton);
                View.BuildShip(btnsGrid);
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
                    resultsWindow.WriteJson(MainWindow.player2Name, MainWindow.player1Name, statsWindow.roundCounter, statsWindow.p1HitCount, statsWindow.p2HitCount, Model.ShipsP1, Model.ShipsP2);
                    resultsWindow.FillDataGridWithResult();

                    resultsWindow.Show();
                    statsWindow.Close();
                    View.CloseWindow();
                }
                else
                {
                    Debug.WriteLine("P1 won!");
                    resultsWindow.WriteJson(MainWindow.player1Name, MainWindow.player2Name, statsWindow.roundCounter, statsWindow.p1HitCount, statsWindow.p2HitCount, Model.ShipsP1, Model.ShipsP2);
                    resultsWindow.FillDataGridWithResult();

                    resultsWindow.Show();
                    statsWindow.Close();
                    View.CloseWindow();
                }
                return;
            }
            if (Model.ShipSizeP1 == 6 && Model.ShipSizeP2 == 6)
            {
                if (Model.turn)
                {
                    View.ReEnableNotClickedGridButtons(View.P1GGrid);
                    View.DisableGridButtons(View.P2GGrid);
                }
                else
                {
                    View.ReEnableNotClickedGridButtons(View.P2GGrid);
                    View.DisableGridButtons(View.P1GGrid);
                }
                Model.turn = !Model.turn;
            }
        }

        public virtual void CheckHit(Button btnToCheck)
        {
            statsWindow.IncRound();
            if (Model.NextPlayerName == MainWindow.player2Name)
            {
                Model.NextPlayerName = MainWindow.player1Name;
            }
            else if (Model.NextPlayerName == MainWindow.player1Name)
            {
                Model.NextPlayerName = MainWindow.player2Name;
            }
            statsWindow.NextStep(Model.NextPlayerName);

            if (!Model.turn)
            {
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
                Debug.WriteLine("No hit on P2's ships");
            }

            else
            {
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
                Debug.WriteLine("No hit on P1's ships");
            }
        }

        public void StartGameSate()
        {
            var rand = new Random();

            if (rand.Next(2) == 1)
                Model.turn = true;
            else
                Model.turn = false;

            if (Model.turn)
            {
                Model.NextPlayerName = MainWindow.player1Name;
            }
            else
            {
                Model.NextPlayerName = MainWindow.player2Name;
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

            if (Model.NextPlayerName == MainWindow.player1Name)
            {


                Debug.WriteLine("P2 won!");
                resultsWindow.WriteJson(MainWindow.player2Name, MainWindow.player1Name, statsWindow.roundCounter, statsWindow.p1HitCount, statsWindow.p2HitCount, Model.ShipsP1, Model.ShipsP2);
                resultsWindow.FillDataGridWithResult();

            }
            else if (Model.NextPlayerName == MainWindow.player2Name)
            {
                Debug.WriteLine("P1 won!");
                resultsWindow.WriteJson(MainWindow.player1Name, MainWindow.player2Name, statsWindow.roundCounter, statsWindow.p1HitCount, statsWindow.p2HitCount, Model.ShipsP1, Model.ShipsP2);
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
