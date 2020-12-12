﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Torpedo.TwoPlayerGame;

namespace Torpedo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TwoPlayerGameView : Window, ITwoPlayerGameView
    {
        private ITwoPLayerGameModel Model;
        private ITwoPlayerGameController Controller;

        public void MakeShipPartHit(Button guessbtn, int player)
        {
            if (player == 2)
            {
                Button gridButton = P2Grid.Children.OfType<Button>().FirstOrDefault(btn => (int)guessbtn.GetValue(Grid.RowProperty) == (int)btn.GetValue(Grid.RowProperty)
                   && (int)guessbtn.GetValue(Grid.ColumnProperty) == (int)btn.GetValue(Grid.ColumnProperty));

                guessbtn.Background = Brushes.Red;
                gridButton.Background = Brushes.Red;

                guessbtn.Content = FindResource("Hit");
                gridButton.Content = FindResource("Hit");

            }
            if (player == 1)
            {
                Button gridButton = P1Grid.Children.OfType<Button>().FirstOrDefault(btn => (int)guessbtn.GetValue(Grid.RowProperty) == (int)btn.GetValue(Grid.RowProperty)
                   && (int)guessbtn.GetValue(Grid.ColumnProperty) == (int)btn.GetValue(Grid.ColumnProperty));


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
                button.Click -= new RoutedEventHandler(Controller.BtnEvent);
            }
        }


        public void ReEnableNotClickedGridButtons(Grid grid)
        {
            //statsWindow.NextStep(nextPlayer);

            foreach (Button button in grid.Children.OfType<Button>().Where(btn => btn.Content == null &&
                btn.Background.ToString() != new SolidColorBrush(Color.FromRgb(80, 154, 159)).ToString()))
            {
                button.Click += new RoutedEventHandler(Controller.BtnEvent);
            }
        }


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
                    button.Click += new RoutedEventHandler(Controller.BtnEvent);
                    gridToBuild.Children.Add(button);
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                }
            }
        }

        public Grid getGrid(string name)
        {
            throw new System.NotImplementedException();
        }

        public TwoPlayerGameView(ITwoPlayerGameController paramController, ITwoPLayerGameModel paramModel)
        {
            InitializeComponent();

            Controller = paramController;
            Model = paramModel;

            BuildGrid(P1GuessGrid);
            BuildGrid(P1Grid);
            BuildGrid(P2GuessGrid);
            BuildGrid(P2Grid);

            TwoPlayerStatsWindow statsWindow;
            statsWindow = new TwoPlayerStatsWindow(MainWindow.player1Name, MainWindow.player2Name);
            p1GuessGridLabel.Content = MainWindow.player1Name + "'s firing board";
            p1GridLabel.Content = MainWindow.player1Name + "'s board";
            p2GuessGridLabel.Content = MainWindow.player2Name + "'s firing board";
            p2GridLabel.Content = MainWindow.player2Name + "'s board";
        }
    }
}
