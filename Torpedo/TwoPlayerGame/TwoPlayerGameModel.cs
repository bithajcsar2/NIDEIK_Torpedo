using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Torpedo.TwoPlayerGame
{
    class TwoPlayerGameModel : ITwoPLayerGameModel
    {
        ITwoPlayerGameController twoPlayerGameController;
        ITwoPlayerGameView twoPlayerGameView;

        public TwoPlayerGameModel(ITwoPlayerGameController twoPlayerGameController, ITwoPlayerGameView twoPlayerGameView)
        {
            this.twoPlayerGameController = twoPlayerGameController;
            this.twoPlayerGameView = twoPlayerGameView;
        }




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

        string p1Name;
        public string P1Name { get { return p1Name; }
            set { p1Name = value; } }
        
        string p2Name;
        public string P2Name
        {
            get { return p2Name; }
            set { p2Name = value; }
        }
    }
}
