using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Torpedo.OnePlayerGame;
using Torpedo.TwoPlayerGame;
namespace Torpedo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TwoPlayerGameView : Window, ITwoPlayerGameView
    {
        private ITwoPLayerGameModel Model = new TwoPlayerGameModel(MainWindow.player1Name, MainWindow.player2Name);
        private ITwoPlayerGameController Controller;
        Grid ITwoPlayerGameView.P1Grid { get => P1Grid; set => P1Grid = value; }
        public Grid P1GGrid { get => P1GuessGrid; set => P1GuessGrid = value; }
        Grid ITwoPlayerGameView.P2Grid { get => P2Grid; set => P2Grid = value; }
        public Grid P2GGrid { get => P2GuessGrid; set => P2GuessGrid = value; }
        public Label P2GridLabel { get => p2GridLabel; set => p2GridLabel = value; }
        public Label P2GuessGridLabel { get => p2GuessGridLabel; set => p2GuessGridLabel = value; }

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


        public void DisableGridButtons(Grid grid)
        {
            foreach (Button button in grid.Children.OfType<Button>())
            {
                button.Click -= new RoutedEventHandler(Controller.BtnEvent);
            }
        }


        public void ReEnableNotClickedGridButtons(Grid grid)
        {

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

        public void CloseWindow()
        {
            this.Close();
        }

        public TwoPlayerGameView(bool AI)
        {
            InitializeComponent();

            if (!AI)
                Controller = new TwoPLayerGameController();
            else
                Controller = new OnePlayerGameController();

            WireUp(Controller, Model);

            BuildGrid(P1GuessGrid);
            BuildGrid(P1Grid);
            BuildGrid(P2GuessGrid);
            BuildGrid(P2Grid);

            p1GuessGridLabel.Content = Model.P1Name + "'s firing board";
            p1GridLabel.Content = Model.P1Name + "'s board";
            p2GuessGridLabel.Content = Model.P2Name + "'s firing board";
            p2GridLabel.Content = Model.P2Name + "'s board";

            this.KeyDown += new KeyEventHandler(Window_KeyDown);


            if (AI)
                Controller.BuildShipsByAICords();

            Controller.StartGameSate();
            this.Show();

        }
        public void WireUp(ITwoPlayerGameController paramControl, ITwoPLayerGameModel paramModel)
        {
            Model = paramModel;
            Controller = paramControl;
            Controller.SetModel(Model);
            Controller.SetView(this);
        }


        public void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                Debug.WriteLine("user is pressed Ctrl+S");
                P2Grid.Visibility = Visibility.Visible;
                P2GGrid.Visibility = Visibility.Visible;
                P2GridLabel.Visibility = Visibility.Visible;
                P2GuessGridLabel.Visibility = Visibility.Visible;
            }
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.H)
            {
                Debug.WriteLine("user is pressed Ctrl+H");
                P2Grid.Visibility = Visibility.Hidden;
                P2GGrid.Visibility = Visibility.Hidden;
                P2GridLabel.Visibility = Visibility.Hidden;
                P2GuessGridLabel.Visibility = Visibility.Hidden;
            }
            if (e.Key == Key.W)
            {
                Model.UserSelectedDirection = 1;
            }
            if (e.Key == Key.S)
            {
                Model.UserSelectedDirection = 3;
            }

            if (e.Key == Key.A)
            {
                Model.UserSelectedDirection = 4;
            }

            if (e.Key == Key.D)
            {
                Model.UserSelectedDirection = 2;
            }

        }

        private void RequestGiveUp(object sender, RoutedEventArgs e)
        {
            Controller.PlayerGiveUp();
        }


    }
}
