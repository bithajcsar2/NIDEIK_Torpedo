using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Torpedo.OnePlayerGame
{
    class OnePlayerGameSate : TwoPlayerGameState
    {
        AI ai = new AI();
        AI.PrevHitLevel hitlevel = new AI.PrevHitLevel();
        List<Button> hitsByAi = new List<Button>();


        public override void CheckHit(Button btnToCheck)
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
                        hitlevel = AI.PrevHitLevel.Hit;
                        hitsByAi.Add(btnToCheck);
                        MakeShipPartHit(btnToCheck, 1);

                       statsWindow.IncP2HitCount();
                        if (ship.hits >= ship.Length)
                        {
                            Debug.WriteLine($"P1's {ship.Length} size ship is dead");
                            ship.isDead = true;
                            hitlevel = AI.PrevHitLevel.Sunk;
                            foreach (var cord in ship.coordinates)
                            {
                                hitsByAi.RemoveAll(hit => (int)hit.GetValue(Grid.RowProperty) == cord[0] && (int)hit.GetValue(Grid.ColumnProperty) == cord[1]);
                            }


                            statsWindow.ListP1ShipStats(ShipsP1);
                            MakeShipLookDead(ship, 1);
                        }
                        return;
                    }
                }
                Debug.WriteLine("No hit on P1's ships");
               hitlevel = AI.PrevHitLevel.NoHit;
            }
        }

        public override void UpdateGameState()
        {

            if (ShipsP1.TrueForAll(ship => ship.isDead == true) || ShipsP2.TrueForAll(ship => ship.isDead == true))
            {
                DisableGridButtons(P1GuessGrid);
                DisableGridButtons(P2GuessGrid);
                DisableGridButtons(P1Grid);
                DisableGridButtons(P2Grid);

                //Ide jön a fájlba írás!

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
                ai.InformAiAboutMove(hitlevel, ref hitsByAi);
                turn = !turn;
                if (turn == true)
                {
                    this.BtnEvent(ai.MakeAiMove(P2GuessGrid.Children.OfType<Button>().Where(btn => btn.Content == null&&
                    btn.Background.ToString() !=  new SolidColorBrush(Color.FromRgb(80, 154, 159)).ToString()).ToList()), new RoutedEventArgs(ButtonBase.ClickEvent));
                }
            }
        }

        public void BuildShipsByAICords(List<int> coordsOfBtnsToPress)
        {
            List<Button> buttons = this.P2Grid.Children.OfType<Button>().ToList();
            foreach (var coord in coordsOfBtnsToPress)
            {
                Button btn = buttons.ElementAt(coord);
                this.BtnEvent(btn, new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        public OnePlayerGameSate()
        {
            /*P2Grid.Visibility = System.Windows.Visibility.Hidden;
            P2GuessGrid.Visibility = System.Windows.Visibility.Hidden;
            p2GridLabel.Visibility = Visibility.Hidden;
            p2GuessGridLabel.Visibility = System.Windows.Visibility.Hidden;*/
            this.KeyDown += new KeyEventHandler(Window_KeyDown);

            //List<Button> buttons = this.P2Grid.Children.OfType<Button>().ToList();
            BuildShipsByAICords(ai.GenerateShipsByAi());
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                Debug.WriteLine("user is pressed Ctrl+S");
                P2Grid.Visibility = System.Windows.Visibility.Visible;
                P2GuessGrid.Visibility = System.Windows.Visibility.Visible;
                p2GridLabel.Visibility = Visibility.Visible;
                p2GuessGridLabel.Visibility = System.Windows.Visibility.Visible;
            }
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.H)
            {
                Debug.WriteLine("user is pressed Ctrl+H");
                P2Grid.Visibility = Visibility.Hidden;
                P2GuessGrid.Visibility = System.Windows.Visibility.Hidden;
                p2GridLabel.Visibility = Visibility.Hidden;
                p2GuessGridLabel.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
