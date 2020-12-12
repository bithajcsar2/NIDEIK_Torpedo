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
using System.Windows.Shapes;
using Torpedo.OnePlayerGame;

namespace Torpedo
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static string player1Name;
        public static string player2Name;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Start1PlayerGame(object sender, RoutedEventArgs e)
        {
            player1Name = player.Text;
            player2Name = "AI";
            OnePlayerGameSate onePlayerGameState = new OnePlayerGameSate();

            onePlayerGameState.Show();

            this.Close();
        }

        public void Start2PlayerGame(object sender, RoutedEventArgs e)
        {
            player1Name = firstPlayer.Text;
            player2Name = secondPlayer.Text;
            TwoPlayerGameView twoPlayerGameState = new TwoPlayerGameView();
            twoPlayerGameState.Show();
            this.Close();
        }

    }
}
