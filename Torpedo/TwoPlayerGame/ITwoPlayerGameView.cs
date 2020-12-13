using System.Windows.Controls;
using System.Windows.Input;

namespace Torpedo.TwoPlayerGame
{
    public interface ITwoPlayerGameView
    {

        void ReEnableNotClickedGridButtons(Grid grid);
        void DisableGridButtons(Grid grid);

        Grid P1Grid { get; set; }
        Grid P1GGrid { get; set; }
        Grid P2Grid { get; set; }
        Grid P2GGrid { get; set; }
        void MakeShipPartHit(Button guessbtn, int player);
        void MakeShipLookDead(Ship ship, int player);
        Label P2GridLabel { get; set; }
        Label P2GuessGridLabel { get; set; }

        void Window_KeyDown(object sender, KeyEventArgs e);

        void CloseWindow();
    }
}
