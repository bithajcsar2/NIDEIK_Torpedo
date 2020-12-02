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
        AI.Hitlevel hitlevel = new AI.Hitlevel();
        Ship shipDestroyedByAI;
        List<Button> hitsByAi = new List<Button>();


        public override void checkHit(Button btnToCheck)
        {
            statsWindow.incRound();
            if (nextPlayer == MainWindow.player2Name)
            {
                nextPlayer = MainWindow.player1Name;
            }
            else if (nextPlayer == MainWindow.player1Name)
            {
                nextPlayer = MainWindow.player2Name;
            }
            statsWindow.nextStep(nextPlayer);

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

                        makeShipPartHit(btnToCheck, 2);

                        statsWindow.incP1HitCount();
                        if (ship.hits >= ship.Length)
                        {
                            Debug.WriteLine($"P2's {ship.Length} size ship is dead");
                            ship.isDead = true;
                            statsWindow.listP2ShipStats(ShipsP2);
                            makeShipLookDead(ship, 2);
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
                        hitlevel = AI.Hitlevel.Hit;
                        hitsByAi.Add(btnToCheck);
                        makeShipPartHit(btnToCheck, 1);
                        shipDestroyedByAI = null;

                       statsWindow.incP2HitCount();
                        if (ship.hits >= ship.Length)
                        {
                            Debug.WriteLine($"P1's {ship.Length} size ship is dead");
                            ship.isDead = true;
                            hitlevel = AI.Hitlevel.Sunk;
                            shipDestroyedByAI = ship;
                            foreach (var cord in ship.coordinates)
                            {
                                hitsByAi.RemoveAll(hit => (int)hit.GetValue(Grid.RowProperty) == cord[0] && (int)hit.GetValue(Grid.ColumnProperty) == cord[1]);
                            }


                            statsWindow.listP1ShipStats(ShipsP1);
                            makeShipLookDead(ship, 1);
                        }
                        return;
                    }
                }
                Debug.WriteLine("No hit on P1's ships");
                shipDestroyedByAI = null;
               hitlevel = AI.Hitlevel.NoHit;
            }
        }

        public override void updateGameState()
        {

            if (ShipsP1.TrueForAll(ship => ship.isDead == true) || ShipsP2.TrueForAll(ship => ship.isDead == true))
            {
                disableGridButtons(P1GuessGrid);
                disableGridButtons(P2GuessGrid);
                disableGridButtons(P1Grid);
                disableGridButtons(P2Grid);

                //Ide jön a fájlba írás!

                if (ShipsP1.TrueForAll(ship => ship.isDead == true))
                {
                    Debug.WriteLine("P2 won!");
                    resultsWindow.WriteJson(MainWindow.player2Name, MainWindow.player1Name, statsWindow.roundCounter, statsWindow.p1HitCount, statsWindow.p2HitCount, ShipsP1, ShipsP2);
                    resultsWindow.FillDataGridWithResult();

                    resultsWindow.Show();
                    statsWindow.Close();
                    //this.Close();
                }
                else
                {
                    Debug.WriteLine("P1 won!");
                    resultsWindow.WriteJson(MainWindow.player1Name, MainWindow.player2Name, statsWindow.roundCounter, statsWindow.p1HitCount, statsWindow.p2HitCount, ShipsP1, ShipsP2);
                    resultsWindow.FillDataGridWithResult();

                    resultsWindow.Show();
                    statsWindow.Close();
                    //this.Close();
                }
                return;
            }
            if (shipSizeP1 == 6 && shipSizeP2 == 6)
            {
                if (turn)
                {
                    reEnableNotClickedGridButtons(P1GuessGrid);
                    disableGridButtons(P2GuessGrid);                    
                }
                else
                {
                    reEnableNotClickedGridButtons(P2GuessGrid);
                    disableGridButtons(P1GuessGrid);                    
                }
                ai.InformAiAboutMove(hitlevel, shipDestroyedByAI, ref hitsByAi);
                turn = !turn;
                if (turn == true)
                {
                    this.btnEvent(ai.MakeAiMove(P2GuessGrid.Children.OfType<Button>().Where(btn => btn.Content == null&&
                    btn.Background.ToString() !=  new SolidColorBrush(Color.FromRgb(80, 154, 159)).ToString()).ToList()), new RoutedEventArgs(ButtonBase.ClickEvent));
                }
            }
        }

        public void BuildShipsByAICords(List<Button> btnsToPress)
        {
            foreach (var btn in btnsToPress)
            {
                this.btnEvent(btn, new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        public OnePlayerGameSate()
        {
            /*P2Grid.Visibility = System.Windows.Visibility.Hidden;
            P2GuessGrid.Visibility = System.Windows.Visibility.Hidden;
            p2GridLabel.Visibility = Visibility.Hidden;
            p2GuessGridLabel.Visibility = System.Windows.Visibility.Hidden;*/
            this.KeyDown += new KeyEventHandler(Window_KeyDown);

            List<Button> buttons = this.P2Grid.Children.OfType<Button>().ToList();
            BuildShipsByAICords(ai.GenerateShipsByAi(buttons));
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
