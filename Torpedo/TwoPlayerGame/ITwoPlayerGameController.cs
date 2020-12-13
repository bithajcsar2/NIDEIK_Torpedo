using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Torpedo.TwoPlayerGame
{
    public interface ITwoPlayerGameController
    {
        void BtnEvent(object sender, EventArgs e);
        void PlayerGiveUp();

        void SetModel(ITwoPLayerGameModel paramModel);
        void SetView(ITwoPlayerGameView paramView);

        void StartGameSate();
        void BuildShipsByAICords();
        void FillShipCoords(Grid gridToBuildOn);
    }
}
