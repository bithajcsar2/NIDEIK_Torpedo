using System.Windows;
using Torpedo.TwoPlayerGame;

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
            if (player.Text != "")
            {
                player1Name = player.Text;
                player2Name = "AI";
                ITwoPlayerGameView twoPlayerGameView = new TwoPlayerGameView(true);
                this.Close();
            }
        }

        public void Start2PlayerGame(object sender, RoutedEventArgs e)
        {
            if (firstPlayer.Text != "" && secondPlayer.Text != "")
            {
                player1Name = firstPlayer.Text;
                player2Name = secondPlayer.Text;
                ITwoPlayerGameView twoPlayerGameView = new TwoPlayerGameView(false);
                this.Close();
            }
        }

    }
}
