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

namespace Torpedo
{
    /// <summary>
    /// Interaction logic for TwoPlayerGameResult.xaml
    /// </summary>
    public partial class TwoPlayerGameResult : Window
    {
        public TwoPlayerGameResult()
        {
            InitializeComponent();
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
