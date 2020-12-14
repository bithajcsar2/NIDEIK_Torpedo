using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Torpedo.TwoPlayerGame;

namespace Torpedo.OnePlayerGame
{
    class OnePlayerGameController : TwoPLayerGameController
    {
        AI ai = new AI();
        AI.PrevHitLevel hitlevel = new AI.PrevHitLevel();
        List<int> hitsByAi = new List<int>();
        public override void CheckHit(Button btnToCheck)
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
                foreach (Ship ship in Model.ShipsP1)
                {
                    int[] matchedCoords = ship.coordinates.FirstOrDefault(coords => (coords[0] == (int)btnToCheck.GetValue(Grid.RowProperty) &&
                     coords[1] == (int)btnToCheck.GetValue(Grid.ColumnProperty)));

                    if (matchedCoords != null)
                    {
                        Debug.WriteLine("P1's ship got hit");
                        ship.hits++;
                        hitlevel = AI.PrevHitLevel.Hit;
                        int coordOfHit = (((int)btnToCheck.GetValue(Grid.RowProperty) - 1) * 10) + ((int)btnToCheck.GetValue(Grid.ColumnProperty) - 1);
                        hitsByAi.Add(coordOfHit);
                        View.MakeShipPartHit(btnToCheck, 1);

                        statsWindow.IncP2HitCount();
                        if (ship.hits >= ship.Length)
                        {
                            Debug.WriteLine($"P1's {ship.Length} size ship is dead");
                            ship.isDead = true;
                            hitlevel = AI.PrevHitLevel.Sunk;
                            foreach (var cord in ship.coordinates)
                            {
                                hitsByAi.RemoveAll(hit => hit == (cord[0] - 1) * 10 + cord[1] - 1);
                            }


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
                hitlevel = AI.PrevHitLevel.NoHit;
            }
        }

        public override void UpdateGameState()
        {

            if (Model.ShipsP1.TrueForAll(ship => ship.isDead == true) || Model.ShipsP2.TrueForAll(ship => ship.isDead == true))
            {
                View.DisableGridButtons(View.P1Grid);
                View.DisableGridButtons(View.P1Grid);
                View.DisableGridButtons(View.P2GGrid);
                View.DisableGridButtons(View.P2GGrid);

                //Ide jön a fájlba írás!

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

                ai.InformAiAboutMove(hitlevel, ref hitsByAi);
                if (Model.Turn == true)
                {
                    var pressableBtns = View.P2GGrid.Children.OfType<Button>().Where(btn => btn.Content == null
                    && btn.Background.ToString() != new SolidColorBrush(Color.FromRgb(80, 154, 159)).ToString()).ToList();
                    List<int> pressableCoords = new List<int>();
                    foreach (var btn in pressableBtns)
                    {
                        int coord = ((int)btn.GetValue(Grid.RowProperty) - 1) * 10 + (int)btn.GetValue(Grid.ColumnProperty) - 1;
                        pressableCoords.Add(coord);
                    }
                    var coordOfBtnToPress = ai.MakeAiMove(pressableCoords);

                    var btnToPress = pressableBtns.Find(btnToFind => (((int)btnToFind.GetValue(Grid.RowProperty) - 1) * 10) + ((int)btnToFind.GetValue(Grid.ColumnProperty) - 1) == coordOfBtnToPress);

                    this.BtnEvent(btnToPress, new RoutedEventArgs(ButtonBase.ClickEvent));
                }
            }
        }

        public override void BuildShipsByAICords()
        {
            List<int> coordsOfBtnsToPress = ai.GenerateShipsByAi();
            List<Button> buttons = View.P2Grid.Children.OfType<Button>().ToList();
            foreach (var coord in coordsOfBtnsToPress)
            {
                Button btn = buttons.ElementAt(coord);
                this.BtnEvent(btn, new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        public OnePlayerGameController()
        {

        }

    }
}
