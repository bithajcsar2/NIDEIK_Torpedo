using System.Windows;
using System.Windows.Controls;
using Torpedo.GameResult;

namespace Torpedo
{
    /// <summary>
    /// Interaction logic for TwoPlayerGameResult.xaml
    /// </summary>
    public partial class GameResultView : Window, IGameResultsView
    {
        public IGameResultContoler contoler;
        public GameResultView()
        {
            InitializeComponent();
            contoler = new GameResultControler(this);
        }

        public DataGrid shareGrid { get { return Results; }
            set => Results=value; }

        public IGameResultContoler shareContoler { get => contoler; set => contoler=value; }

        public void show()
        {
            this.Show();
        }

        private void ExitGame(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
            this.Close();
        }
 
    }
}
