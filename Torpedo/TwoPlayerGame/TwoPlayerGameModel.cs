﻿using System.Collections.Generic;
using System.Windows.Controls;

namespace Torpedo.TwoPlayerGame
{
    class TwoPlayerGameModel : ITwoPLayerGameModel
    {
        protected List<Button> shipButtonListP1 = new List<Button>();
        protected List<int[]> shipsCoordsListP1 = new List<int[]>();
        protected List<Ship> ShipsP1 = new List<Ship>()
        {
            new Ship(1, "Destroyer"),
            new Ship(2, "Submarine"),
            new Ship(3, "Cruiser"),
            new Ship(4, "Battleship"),
            new Ship(5, "Carrier")
        };
        protected List<Button> shipButtonListP2 = new List<Button>();
        protected List<int[]> shipsCoordsListP2 = new List<int[]>(); 
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

        protected bool turn = true;

        public TwoPlayerGameModel(string p1Name, string p2Name)
        {
            P2Name = p2Name;
            P1Name = p1Name;
        }

        public bool Turn
        {
            get => turn;
            set => turn = value;
        }

        public string NextPlayerName { get; set; }

        public string P2Name { get; set; }
        public string P1Name { get; set; }

        List<Button> ITwoPLayerGameModel.shipButtonListP1 { get => shipButtonListP1; set => shipButtonListP1 = value; }
        List<Button> ITwoPLayerGameModel.shipButtonListP2 { get => shipButtonListP2; set => shipButtonListP2 = value; }
        List<Ship> ITwoPLayerGameModel.ShipsP1 { get => ShipsP1; set => ShipsP1 = value; }
        List<Ship> ITwoPLayerGameModel.ShipsP2 { get => ShipsP2; set => ShipsP2 = value; }
        public int ShipSizeP1 { get => shipSizeP1; set => shipSizeP1 = value; }
        public int ShipSizeP2 { get => shipSizeP2; set => shipSizeP2 = value; }
        public int UserSelectedDirection { get; set; }
        List<int[]> ITwoPLayerGameModel.shipsCoordsListP1 { get=> shipsCoordsListP1; set=> shipsCoordsListP1=value; }
        List<int[]> ITwoPLayerGameModel.shipsCoordsListP2 { get => shipsCoordsListP2; set => shipsCoordsListP2 = value; }
    }
}