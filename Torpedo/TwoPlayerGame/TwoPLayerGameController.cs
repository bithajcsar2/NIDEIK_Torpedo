using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Torpedo.TwoPlayerGame
{
    class TwoPLayerGameController
    {
        private ITwoPLayerGameModel Model;
        private ITwoPlayerGameView View;

        public TwoPLayerGameController(ITwoPLayerGameModel twoPlayerGameModel, ITwoPlayerGameView twoPlayerGameView)
        {
            Model = twoPlayerGameModel;
            View = twoPlayerGameView;
            StartGameSate();

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

            if (btnsGrid.Name == P1Grid.Name)
            {
                shipButtonListP1.Add(clickedButton);
                BuildShip(btnsGrid);
            }

            if (btnsGrid.Name == P2Grid.Name)
            {
                shipButtonListP2.Add(clickedButton);
                BuildShip(btnsGrid);
            }

            if (btnsGrid.Name == P1GuessGrid.Name)
            {
                CheckHit(clickedButton);
            }

            if (btnsGrid.Name == P2GuessGrid.Name)
            {
                CheckHit(clickedButton);
            }
            UpdateGameState();
        }

        public virtual void UpdateGameState()
        {

            if (ShipsP1.TrueForAll(ship => ship.isDead == true) || ShipsP2.TrueForAll(ship => ship.isDead == true))
            {
                DisableGridButtons(P1GuessGrid);
                DisableGridButtons(P2GuessGrid);
                DisableGridButtons(P1Grid);
                DisableGridButtons(P2Grid);


                if (ShipsP1.TrueForAll(ship => ship.isDead == true))
                {
                    Debug.WriteLine("P2 won!");
                    resultsWindow.WriteJson(MainWindow.player2Name, MainWindow.player1Name, statsWindow.roundCounter, statsWindow.p1HitCount, statsWindow.p2HitCount, ShipsP1, ShipsP2);
                    resultsWindow.FillDataGridWithResult();

                    resultsWindow.Show();
                    statsWindow.Close();
                    this.Close();
                }
                else
                {
                    Debug.WriteLine("P1 won!");
                    resultsWindow.WriteJson(MainWindow.player1Name, MainWindow.player2Name, statsWindow.roundCounter, statsWindow.p1HitCount, statsWindow.p2HitCount, ShipsP1, ShipsP2);
                    resultsWindow.FillDataGridWithResult();

                    resultsWindow.Show();
                    statsWindow.Close();
                    this.Close();
                }
                return;
            }
            if (shipSizeP1 == 6 && shipSizeP2 == 6)
            {
                if (turn)
                {
                    ReEnableNotClickedGridButtons(P1GuessGrid);
                    DisableGridButtons(P2GuessGrid);
                }
                else
                {
                    ReEnableNotClickedGridButtons(P2GuessGrid);
                    DisableGridButtons(P1GuessGrid);
                }
                turn = !turn;
            }
        }

        public virtual void CheckHit(Button btnToCheck)
        {
            statsWindow.IncRound();
            if (nextPlayer == MainWindow.player2Name)
            {
                nextPlayer = MainWindow.player1Name;
            }
            else if (nextPlayer == MainWindow.player1Name)
            {
                nextPlayer = MainWindow.player2Name;
            }
            statsWindow.NextStep(nextPlayer);

            if (!turn)
            {
                foreach (Ship ship in ShipsP2)
                {
                    int[] matchedCoords = ship.coordinates.FirstOrDefault(coords => (coords[0] == (int)btnToCheck.GetValue(Grid.RowProperty) &&
                     coords[1] == (int)btnToCheck.GetValue(Grid.ColumnProperty)));

                    if (matchedCoords != null)
                    {
                        Debug.WriteLine("P2's ship got hit");
                        ship.hits++;

                        MakeShipPartHit(btnToCheck, 2);

                        statsWindow.IncP1HitCount();
                        if (ship.hits >= ship.Length)
                        {
                            Debug.WriteLine($"P2's {ship.Length} size ship is dead");
                            ship.isDead = true;
                            statsWindow.ListP2ShipStats(ShipsP2);
                            MakeShipLookDead(ship, 2);
                        }
                        return;
                    }
                }
                Debug.WriteLine("No hit on P2's ships");
            }

            else
            {
                foreach (Ship ship in ShipsP1)
                {
                    int[] matchedCoords = ship.coordinates.FirstOrDefault(coords => (coords[0] == (int)btnToCheck.GetValue(Grid.RowProperty) &&
                     coords[1] == (int)btnToCheck.GetValue(Grid.ColumnProperty)));

                    if (matchedCoords != null)
                    {
                        Debug.WriteLine("P1's ship got hit");
                        ship.hits++;

                        MakeShipPartHit(btnToCheck, 1);

                        statsWindow.IncP2HitCount();
                        if (ship.hits >= ship.Length)
                        {
                            Debug.WriteLine($"P1's {ship.Length} size ship is dead");
                            ship.isDead = true;
                            statsWindow.ListP1ShipStats(ShipsP1);
                            MakeShipLookDead(ship, 1);
                        }
                        return;
                    }
                }
                Debug.WriteLine("No hit on P1's ships");
            }
        }
        private void PlayerGiveUp(object sender, RoutedEventArgs e)
        {
            if (nextPlayer == MainWindow.player1Name)
            {
                DisableGridButtons(P1GuessGrid);
                DisableGridButtons(P2GuessGrid);
                DisableGridButtons(P1Grid);
                DisableGridButtons(P2Grid);

                Debug.WriteLine("P2 won!");
                resultsWindow.WriteJson(MainWindow.player2Name, MainWindow.player1Name, statsWindow.roundCounter, statsWindow.p1HitCount, statsWindow.p2HitCount, ShipsP1, ShipsP2);
                resultsWindow.FillDataGridWithResult();

                resultsWindow.Show();
                statsWindow.Close();
                this.Close();
            }
            else if (nextPlayer == MainWindow.player2Name)
            {
                DisableGridButtons(P1GuessGrid);
                DisableGridButtons(P2GuessGrid);
                DisableGridButtons(P1Grid);
                DisableGridButtons(P2Grid);

                Debug.WriteLine("P1 won!");
                resultsWindow.WriteJson(MainWindow.player1Name, MainWindow.player2Name, statsWindow.roundCounter, statsWindow.p1HitCount, statsWindow.p2HitCount, ShipsP1, ShipsP2);
                resultsWindow.FillDataGridWithResult();

                resultsWindow.Show();
                statsWindow.Close();
                this.Close();
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
                nextPlayer = MainWindow.player1Name;
            }
            else
            {
                nextPlayer = MainWindow.player2Name;
            }
            View.DisableGridButtons(P1GuessGrid);
            View.DisableGridButtons(P2GuessGrid);
        }
    }

}
