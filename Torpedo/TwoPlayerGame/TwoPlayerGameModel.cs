using System.Collections.Generic;
using System.Windows.Controls;

namespace Torpedo.TwoPlayerGame
{
    class TwoPlayerGameModel : ITwoPLayerGameModel
    {

        protected List<Button> shipButtonListP1 = new List<Button>();
        protected List<Ship> ShipsP1 = new List<Ship>()
        {
            new Ship(1, "Destroyer"),
            new Ship(2, "Submarine"),
            new Ship(3, "Cruiser"),
            new Ship(4, "Battleship"),
            new Ship(5, "Carrier")
        };
        protected List<Button> shipButtonListP2 = new List<Button>();
        protected List<Ship> ShipsP2 = new List<Ship>()
        {
            new Ship(1, "Destroyer"),
            new Ship(2, "Submarine"),
            new Ship(3, "Cruiser"),
            new Ship(4, "Battleship"),
            new Ship(5, "Carrier")
        };

        public int shipSizeP1 = 1;
        public int shipSizeP2 = 1;

        protected bool turnHelper = true;
        bool ITwoPLayerGameModel.turn
        {
            get
            {
                return turnHelper;
            }
            set
            {
                turnHelper = value;
            }
        }

        string nextPlayerName;
        public string NextPlayerName
        {
            get { return nextPlayerName; }
            set { nextPlayerName = value; }
        }

        string p2Name;
        public string P2Name
        {
            get { return p2Name; }
            set { p2Name = value; }
        }

        List<Button> ITwoPLayerGameModel.shipButtonListP1 { get => shipButtonListP1; set => shipButtonListP1 = value; }
        List<Button> ITwoPLayerGameModel.shipButtonListP2 { get => shipButtonListP2; set => shipButtonListP2 = value; }
        List<Ship> ITwoPLayerGameModel.ShipsP1 { get => ShipsP1; set => ShipsP1 = value; }
        List<Ship> ITwoPLayerGameModel.ShipsP2 { get => ShipsP1; set => ShipsP1 = value; }
        public int ShipSizeP1 { get => shipSizeP1; set => shipSizeP1 = value; }
        public int ShipSizeP2 { get => shipSizeP2; set => shipSizeP2 = value; }
    }
}
