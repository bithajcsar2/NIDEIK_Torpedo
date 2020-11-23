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

namespace Torpedo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool turn = false;
        List<Button> shipButtonListP1 = new List<Button>();
        List<Ship> ShipsP1 = new List<Ship>()
        {
            new Ship(1),
            new Ship(2),
            new Ship(3),
            new Ship(4),
            new Ship(5)
        };
        List<Button> shipButtonListP2 = new List<Button>();
        List<Ship> ShipsP2 = new List<Ship>()
        {
            new Ship(1),
            new Ship(2),
            new Ship(3),
            new Ship(4),
            new Ship(5)
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
                Label horLabel = new Label();
                horLabel.Content = (char)('A' + i - 1);
                gridToBuild.Children.Add(horLabel);
                Grid.SetRow(horLabel, 0);
                Grid.SetColumn(horLabel, i);

                Label verLabel = new Label();
                verLabel.Content = i;
                gridToBuild.Children.Add(verLabel);
                Grid.SetRow(verLabel, i);
                Grid.SetColumn(verLabel, 0);
            }
            for (int i = 1; i < 11; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    Button button = new Button();
                    button.Click += new RoutedEventHandler(btnEvent);
                    gridToBuild.Children.Add(button);
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                }
            }
        }
        void btnEvent(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            clickedButton.Background = Brushes.Blue;
            clickedButton.Click -= new RoutedEventHandler(btnEvent);
            int _row = (int)clickedButton.GetValue(Grid.RowProperty);
            int _column = (int)clickedButton.GetValue(Grid.ColumnProperty);
            var btnsGrid = VisualTreeHelper.GetParent(clickedButton) as Grid;

            Debug.WriteLine(_row + " " + _column+"  "+btnsGrid.Name);

            if (btnsGrid.Name == P1Grid.Name)
            {
                shipButtonListP1.Add(clickedButton);
                buildShip(btnsGrid);
            }

            if (btnsGrid.Name == P2Grid.Name)
            {
                shipButtonListP2.Add(clickedButton);
                buildShip(btnsGrid);
            }

            if (btnsGrid.Name == P1GuessGrid.Name)
            {
                checkHit(clickedButton);
            }

            if (btnsGrid.Name == P2GuessGrid.Name)
            {
                checkHit(clickedButton);
            }
            updateGameState();
        }

        public void updateGameState()
        {
            turn = !turn;
            if (ShipsP1.TrueForAll(ship => ship.isDead == true) || ShipsP2.TrueForAll(ship => ship.isDead == true))
            {
                disableGridButtons(P1GuessGrid);
                disableGridButtons(P2GuessGrid);
                disableGridButtons(P1Grid);
                disableGridButtons(P2Grid);

                //Ide jön a fájlba írás!

                if(ShipsP1.TrueForAll(ship => ship.isDead == true))
                {
                    Debug.WriteLine("P2 won!");
                }
                else
                {
                    Debug.WriteLine("P1 won!");
                }
                return;
            }
            if (shipSizeP1 == 6 && shipSizeP2 == 6)
            {
                if (turn)
                {
                    reEnableGridButtons(P1GuessGrid);
                    disableGridButtons(P2GuessGrid);
                }
                else
                {
                    reEnableGridButtons(P2GuessGrid);
                    disableGridButtons(P1GuessGrid);
                }              
            }
        }

        public void checkHit(Button btnToCheck)
        {
            if (turn)
            {
                foreach (Ship ship in ShipsP2)
                {
                    int[] matchedCoords = ship.coordinates.FirstOrDefault(coords => (coords[0] == (int)btnToCheck.GetValue(Grid.RowProperty) &&
                     coords[1] == (int)btnToCheck.GetValue(Grid.ColumnProperty)));

                    if (matchedCoords != null)
                    {
                        Debug.WriteLine("P2s ship got hit");
                        ship.hits++;
                        btnToCheck.Background = Brushes.Red;
                        if (ship.hits >= ship.Length)
                        {
                            Debug.WriteLine($"P2s {ship.Length} size ship is dead");
                            ship.isDead = true;
                            makeShipLookDead(ship, 2);
                        }
                        return;
                    }
                }
                Debug.WriteLine("No hit on P2s ships");
            }

            else
            {
                foreach (Ship ship in ShipsP1)
                {
                    int[] matchedCoords = ship.coordinates.FirstOrDefault(coords => (coords[0] == (int)btnToCheck.GetValue(Grid.RowProperty) &&
                     coords[1] == (int)btnToCheck.GetValue(Grid.ColumnProperty)));

                    if (matchedCoords != null)
                    {
                        Debug.WriteLine("P1s ship got hit");
                        ship.hits++;
                        btnToCheck.Background = Brushes.Red;
                        if (ship.hits >= ship.Length)
                        {
                            Debug.WriteLine($"P1s {ship.Length} size ship is dead");
                            ship.isDead = true;
                            makeShipLookDead(ship, 1);
                        }
                        return;
                    }
                }
                Debug.WriteLine("No hit on P1s ships");
            }
        }

        private void makeShipLookDead(Ship ship, int player)
        {
            if (player == 2)
            {
                IEnumerable<Button> buttonsOnGrid = P2Grid.Children.OfType<Button>();
                IEnumerable<Button> buttonsOnGuessGrid = P1GuessGrid.Children.OfType<Button>();

                foreach (var coords in ship.coordinates)
                {
                    Button btnToColor = buttonsOnGrid.FirstOrDefault(button => ((int)button.GetValue(Grid.RowProperty) == coords[0] && (int)button.GetValue(Grid.ColumnProperty) == coords[1]));
                    btnToColor.Background = Brushes.Black;

                    btnToColor = buttonsOnGuessGrid.FirstOrDefault(button => ((int)button.GetValue(Grid.RowProperty) == coords[0] && (int)button.GetValue(Grid.ColumnProperty) == coords[1]));
                    btnToColor.Background = Brushes.Black; ;
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

                    btnToColor = buttonsOnGuessGrid.FirstOrDefault(button => ((int)button.GetValue(Grid.RowProperty) == coords[0] && (int)button.GetValue(Grid.ColumnProperty) == coords[1]));
                    btnToColor.Background = Brushes.Black; ;
                }
            }
        }

        public void buildShip(Grid gridToBuildOn)
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
                        disableGridButtons(P1Grid);
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
                        disableGridButtons(P2Grid);
                    }
                    shipButtonListP2.Clear();
                }
            }          
        }
        public void disableGridButtons(Grid grid)
        {
            foreach (Button button in grid.Children.OfType<Button>())
            {
                button.Click -= new RoutedEventHandler(btnEvent);
            }
        }

        public void reEnableGridButtons(Grid grid)
        {
            foreach (Button button in grid.Children.OfType<Button>().Where<Button>(btn => (btn.Background != Brushes.Black && btn.Background!= Brushes.Red && btn.Background != Brushes.Blue)))
            {
                button.Click += new RoutedEventHandler(btnEvent);
            }
        }

        public void startGameSate()
        {
            disableGridButtons(P1GuessGrid);
            disableGridButtons(P2GuessGrid);
        }

        public MainWindow()
        {
            InitializeComponent();
            BuildGrid(P1GuessGrid);
            BuildGrid(P1Grid);
            BuildGrid(P2GuessGrid);
            BuildGrid(P2Grid);

            startGameSate();        
        }
    }
}
