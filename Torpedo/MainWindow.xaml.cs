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

namespace Torpedo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int[] clickedCoordGuessP1 = new int[2];
        public int[] clickedCoordOwnP1 = new int[2];
        public int[] clickedCoordGuessP2 = new int[2];
        public int[] clickedCoordOwnP2 = new int[2];
        List<Button> shipCoordListP1 = new List<Button>();
        List<Ship> ShipsP1 = new List<Ship>()
        {
            new Ship(1),
            new Ship(2),
            new Ship(3),
            new Ship(4),
            new Ship(5)
        };
        List<Button> shipCoordListP2 = new List<Button>();
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
            var element = VisualTreeHelper.GetParent(clickedButton) as Grid;
            if (element.Name == guessGrid.Name)
            {
                clickedCoordGuessP1[0] = (int)clickedButton.GetValue(Grid.RowProperty);
                clickedCoordGuessP1[1] = (int)clickedButton.GetValue(Grid.ColumnProperty);
                checkHit(clickedButton);
            }
            if(element.Name == myGrid.Name)
            {
                clickedCoordOwnP1[0] = (int)clickedButton.GetValue(Grid.RowProperty);
                clickedCoordOwnP1[1] = (int)clickedButton.GetValue(Grid.ColumnProperty);
                shipCoordListP1.Add(clickedButton);
                buildShip(element);
            }
            if (element.Name == guessGridEnemy.Name)
            {
                clickedCoordGuessP2[0] = (int)clickedButton.GetValue(Grid.RowProperty);
                clickedCoordGuessP2[1] = (int)clickedButton.GetValue(Grid.ColumnProperty);
            }
            if (element.Name == enemyGrid.Name)
            {
                clickedCoordOwnP2[0] = (int)clickedButton.GetValue(Grid.RowProperty);
                clickedCoordOwnP2[1] = (int)clickedButton.GetValue(Grid.ColumnProperty);
                shipCoordListP2.Add(clickedButton);
                buildShip(element);
            }

          //  lastClicked.Content = "Row: "+clickedCoordGuessP1[0] + "  " + "Column: "+clickedCoordGuessP1[1]+" on "+element.Name;
        }

        public void checkHit(Button btnToColor)
        {
            foreach (Ship ship in ShipsP2)
            {
                foreach (int[] coords in ship.coordinates)
                {
                    if (coords[0] == clickedCoordGuessP1[0] && coords[1] == clickedCoordGuessP1[1])
                    {
                        lastClicked.Content = "hit";
                        ship.hits++;
                        btnToColor.Background = Brushes.Red;
                        if (ship.hits >= ship.Length)
                        {
                            lastClicked.Content = "ship is dead";
                            btnToColor.Background = Brushes.Black;
                        }
                        return;
                    }
                    else
                    {
                        btnToColor.Background = Brushes.Blue;
                        lastClicked.Content = " no hit";
                    }
                }
            }
        }

        public void buildShip(Grid gridToBuildOn)
        {
            if (gridToBuildOn.Name == myGrid.Name)
            {
                if (shipCoordListP1.Count == shipSizeP1)
                {
                    for (int i = 0; i < shipSizeP1; i++)
                    {
                        int coordx = (int)shipCoordListP1.ElementAt(i).GetValue(Grid.RowProperty);
                        int coordy = (int)shipCoordListP1.ElementAt(i).GetValue(Grid.ColumnProperty);
                        int[] coords = { coordx, coordy };
                        ShipsP1.ElementAt(shipSizeP1 - 1).coordinates.Add(coords);
                    }
                    shipSizeP1++;
                    if (shipCoordListP1.Count() == 5)
                    {
                        disableOwnFieldButtons(myGrid);
                    }
                    shipCoordListP1.Clear();
                }
            }
            if (gridToBuildOn.Name == enemyGrid.Name)
            {
                if (shipCoordListP2.Count == shipSizeP2)
                {
                    for (int i = 0; i < shipSizeP2; i++)
                    {
                        int coordx = (int)shipCoordListP2.ElementAt(i).GetValue(Grid.RowProperty);
                        int coordy = (int)shipCoordListP2.ElementAt(i).GetValue(Grid.ColumnProperty);
                        int[] coords = { coordx, coordy };
                        ShipsP2.ElementAt(shipSizeP2 - 1).coordinates.Add(coords);
                    }
                    shipSizeP2++;
                    if (shipCoordListP2.Count() == 5)
                    {
                        disableOwnFieldButtons(enemyGrid);
                    }
                    shipCoordListP2.Clear();
                }
            }
        }
        public void disableOwnFieldButtons(Grid ownGrid)
        {
            foreach (Button button in ownGrid.Children.OfType<Button>())
            {
                button.Click -= new RoutedEventHandler(btnEvent);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            BuildGrid(guessGrid);
            BuildGrid(myGrid);
            BuildGrid(guessGridEnemy);
            BuildGrid(enemyGrid);
            
        }
    }
}
