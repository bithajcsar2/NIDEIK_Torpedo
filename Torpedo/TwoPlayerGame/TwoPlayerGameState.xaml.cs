using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Xml.XPath;

namespace Torpedo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TwoPlayerGameState : Window
    {
        protected TwoPlayerStatsWindow statsWindow;
        protected GameResult resultsWindow = new GameResult();

        protected String nextPlayer;

        protected bool turn = true;
        protected List<Button> shipButtonListP1 = new List<Button>();
        protected List<Ship> ShipsP1 = new List<Ship>()
        {
            new Ship(1, "Destroyer"),
            new Ship(2, "Submarine"),
            new Ship(3, "Cruiser"),
            new Ship(4, "Battleship"),
            new Ship(5, "Carrier")
        };
        protected List<Button> shipButtonListP2 = new List<Button>();
        protected List<Ship> ShipsP2 = new List<Ship>()
        {
            new Ship(1, "Destroyer"),
            new Ship(2, "Submarine"),
            new Ship(3, "Cruiser"),
            new Ship(4, "Battleship"),
            new Ship(5, "Carrier")
        };

        public int shipSizeP1 = 1;
        public int shipSizeP2 = 1;

        public void BuildGrid(Grid gridToBuild)
        {
            gridToBuild.RowDefinitions.Add(new RowDefinition());
            gridToBuild.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 1; i < 11; i++)
            {
                gridToBuild.RowDefinitions.Add(new RowDefinition());
                gridToBuild.ColumnDefinitions.Add(new ColumnDefinition());
                Label horLabel = new Label
                {
                    Content = (char)('A' + i - 1),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                gridToBuild.Children.Add(horLabel);
                Grid.SetRow(horLabel, 0);
                Grid.SetColumn(horLabel, i);

                Label verLabel = new Label
                {
                    Content = i,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                gridToBuild.Children.Add(verLabel);
                Grid.SetRow(verLabel, i);
                Grid.SetColumn(verLabel, 0);
            }
            for (int i = 1; i < 11; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    Button button = new Button();
                    button.Click += new RoutedEventHandler(BtnEvent);
                    gridToBuild.Children.Add(button);
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                }
            }
        }
        public void BtnEvent(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            clickedButton.Background = new SolidColorBrush(Color.FromRgb(80, 154, 159));
            clickedButton.Click -= new RoutedEventHandler(BtnEvent);
            int _row = (int)clickedButton.GetValue(Grid.RowProperty);
            int _column = (int)clickedButton.GetValue(Grid.ColumnProperty);
            var btnsGrid = VisualTreeHelper.GetParent(clickedButton) as Grid;

            Debug.WriteLine(_row + " " + _column+"  "+btnsGrid.Name);

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


                if(ShipsP1.TrueForAll(ship => ship.isDead == true))
                {
                    Debug.WriteLine("P2 won!");
                    resultsWindow.WriteJson(MainWindow.player2Name, MainWindow.player1Name,statsWindow.roundCounter, statsWindow.p1HitCount, statsWindow.p2HitCount, ShipsP1, ShipsP2);
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
            if(nextPlayer == MainWindow.player2Name)
            {
                nextPlayer = MainWindow.player1Name;
            }
             else if(nextPlayer == MainWindow.player1Name)
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

        public void MakeShipPartHit(Button guessbtn, int player)
        {
            if (player == 2)
            {
                Button gridButton = P2Grid.Children.OfType<Button>().FirstOrDefault( btn => (int)guessbtn.GetValue(Grid.RowProperty)==(int)btn.GetValue(Grid.RowProperty)
                    && (int)guessbtn.GetValue(Grid.ColumnProperty)==(int)btn.GetValue(Grid.ColumnProperty));

                guessbtn.Background = Brushes.Red;
                gridButton.Background = Brushes.Red;

                guessbtn.Content = FindResource("Hit");
                gridButton.Content = FindResource("Hit");

            }
            if (player == 1)
            {
                Button gridButton = P1Grid.Children.OfType<Button>().FirstOrDefault( btn => (int)guessbtn.GetValue(Grid.RowProperty)==(int)btn.GetValue(Grid.RowProperty)
                    && (int)guessbtn.GetValue(Grid.ColumnProperty)==(int)btn.GetValue(Grid.ColumnProperty));


                guessbtn.Background = Brushes.Red;
                gridButton.Background = Brushes.Red;
                guessbtn.Content = FindResource("Hit");
                gridButton.Content = FindResource("Hit");
            }
        }
    

        public void MakeShipLookDead(Ship ship, int player)
        {
            if (player == 2)
            {
                IEnumerable<Button> buttonsOnGrid = P2Grid.Children.OfType<Button>();
                IEnumerable<Button> buttonsOnGuessGrid = P1GuessGrid.Children.OfType<Button>();

                foreach (var coords in ship.coordinates)
                {
                    Button btnToColor = buttonsOnGrid.FirstOrDefault(button => ((int)button.GetValue(Grid.RowProperty) == coords[0] && (int)button.GetValue(Grid.ColumnProperty) == coords[1]));
                    btnToColor.Background = Brushes.Black;
                    btnToColor.Content = FindResource("Dead");

                    btnToColor = buttonsOnGuessGrid.FirstOrDefault(button => ((int)button.GetValue(Grid.RowProperty) == coords[0] && (int)button.GetValue(Grid.ColumnProperty) == coords[1]));
                    btnToColor.Background = Brushes.Black;
                    btnToColor.Content = FindResource("Dead");
                }

            }
            if (player == 1)
            {
                IEnumerable<Button> buttonsOnGrid = P1Grid.Children.OfType<Button>();
                IEnumerable<Button> buttonsOnGuessGrid = P2GuessGrid.Children.OfType<Button>();

                foreach (var coords in ship.coordinates)
                {
                    Button btnToColor = buttonsOnGrid.FirstOrDefault(button => ((int)button.GetValue(Grid.RowProperty) == coords[0] && (int)button.GetValue(Grid.ColumnProperty) == coords[1]));
                    btnToColor.Background = Brushes.Black;
                    btnToColor.Content = FindResource("Dead");

                    btnToColor = buttonsOnGuessGrid.FirstOrDefault(button => ((int)button.GetValue(Grid.RowProperty) == coords[0] && (int)button.GetValue(Grid.ColumnProperty) == coords[1]));
                    btnToColor.Background = Brushes.Black; 
                    btnToColor.Content = FindResource("Dead");

                }
            }
        }

        public void BuildShip(Grid gridToBuildOn)
        {
            if (gridToBuildOn.Name == P1Grid.Name)
            {
                if (shipButtonListP1.Count == shipSizeP1)
                {
                    for (int i = 0; i < shipSizeP1; i++)
                    {
                        int coordx = (int)shipButtonListP1.ElementAt(i).GetValue(Grid.RowProperty);
                        int coordy = (int)shipButtonListP1.ElementAt(i).GetValue(Grid.ColumnProperty);
                        int[] coords = { coordx, coordy };
                        ShipsP1.ElementAt(shipSizeP1 - 1).coordinates.Add(coords);
                    }
                    shipSizeP1++;
                    if (shipButtonListP1.Count() == 5)
                    {
                        DisableGridButtons(P1Grid);
                    }
                    shipButtonListP1.Clear();
                }
            }
            if (gridToBuildOn.Name == P2Grid.Name)
            {
                if (shipButtonListP2.Count == shipSizeP2)
                {
                    for (int i = 0; i < shipSizeP2; i++)
                    {
                        int coordx = (int)shipButtonListP2.ElementAt(i).GetValue(Grid.RowProperty);
                        int coordy = (int)shipButtonListP2.ElementAt(i).GetValue(Grid.ColumnProperty);
                        int[] coords = { coordx, coordy };
                        ShipsP2.ElementAt(shipSizeP2 - 1).coordinates.Add(coords);
                    }
                    shipSizeP2++;
                    if (shipButtonListP2.Count() == 5)
                    {
                        DisableGridButtons(P2Grid);
                    }
                    shipButtonListP2.Clear();
                }
            }          
        }
        public void DisableGridButtons(Grid grid)
        {
            foreach (Button button in grid.Children.OfType<Button>())
            {
                button.Click -= new RoutedEventHandler(BtnEvent);
            }
        }


        public void ReEnableNotClickedGridButtons(Grid grid)
        {
            statsWindow.NextStep(nextPlayer);

            foreach (Button button in grid.Children.OfType<Button>().Where(btn => btn.Content == null &&
                btn.Background.ToString() != new SolidColorBrush(Color.FromRgb(80, 154, 159)).ToString()))
            {
                button.Click += new RoutedEventHandler(BtnEvent);
            }
        }

        public void StartGameSate()
        {
            var rand = new Random();

            if (rand.Next(2) == 1)
                turn = true;
            else
                turn = false;

            if (turn)
            {
                nextPlayer = MainWindow.player1Name;
            }
            else
            {
                nextPlayer = MainWindow.player2Name;
            }
            DisableGridButtons(P1GuessGrid);
            DisableGridButtons(P2GuessGrid);
            statsWindow.Show();
        }


        public TwoPlayerGameState()
        {
            InitializeComponent();
            BuildGrid(P1GuessGrid);
            BuildGrid(P1Grid);
            BuildGrid(P2GuessGrid);
            BuildGrid(P2Grid);

            statsWindow = new TwoPlayerStatsWindow(MainWindow.player1Name, MainWindow.player2Name);
            p1GuessGridLabel.Content = MainWindow.player1Name + "'s firing board";
            p1GridLabel.Content = MainWindow.player1Name + "'s board";
            p2GuessGridLabel.Content = MainWindow.player2Name + "'s firing board";
            p2GridLabel.Content = MainWindow.player2Name + "'s board";
            StartGameSate();
        }

        private void Player1GiveUp(object sender, RoutedEventArgs e)
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
        }

        private void Player2GiveUp(object sender, RoutedEventArgs e)
        {
            if (nextPlayer == MainWindow.player2Name)
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
    }
}
