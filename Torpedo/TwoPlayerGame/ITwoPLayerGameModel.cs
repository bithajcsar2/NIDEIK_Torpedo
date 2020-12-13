using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Torpedo.TwoPlayerGame
{
    public interface ITwoPLayerGameModel
    {
        string NextPlayerName { get; set; }

        bool Turn { get; set; }
        string P2Name { get; set; }
        string P1Name { get; set; }

        List<Button> shipButtonListP1 { get; set; }
        List<Button> shipButtonListP2 { get; set; }
        List<int[]> shipsCoordsListP1 { get; set; }
        List<int[]> shipsCoordsListP2 { get; set; }

        List<Ship> ShipsP1 { get; set; }
        List<Ship> ShipsP2 { get; set; }
        int ShipSizeP1 { get; set; }
        int ShipSizeP2 { get; set; }

        int UserSelectedDirection { get; set; }
    }
}
