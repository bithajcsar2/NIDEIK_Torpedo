using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;


namespace Torpedo.OnePlayerGame
{
    class OnePlayerGameSate : TwoPlayerGameState
    {
        public OnePlayerGameSate()
        {
            /*P2Grid.Visibility = System.Windows.Visibility.Hidden;
            P2GuessGrid.Visibility = System.Windows.Visibility.Hidden;
            p2GridLabel.Visibility = Visibility.Hidden;
            p2GuessGridLabel.Visibility = System.Windows.Visibility.Hidden;*/
            this.KeyDown += new KeyEventHandler(Window_KeyDown);
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                Debug.WriteLine("user is pressed Ctrl+S");
                P2Grid.Visibility = System.Windows.Visibility.Visible;
                P2GuessGrid.Visibility = System.Windows.Visibility.Visible;
                p2GridLabel.Visibility = Visibility.Visible;
                p2GuessGridLabel.Visibility = System.Windows.Visibility.Visible;
            }
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.H)
            {
                Debug.WriteLine("user is pressed Ctrl+H");
                P2Grid.Visibility = Visibility.Hidden;
                P2GuessGrid.Visibility = System.Windows.Visibility.Hidden;
                p2GridLabel.Visibility = Visibility.Hidden;
                p2GuessGridLabel.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
