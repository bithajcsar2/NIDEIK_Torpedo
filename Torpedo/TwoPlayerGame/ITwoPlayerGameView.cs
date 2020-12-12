using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Torpedo.TwoPlayerGame
{
    interface ITwoPlayerGameView
    {

        void ReEnableNotClickedGridButtons(Grid grid);
        void DisableGridButtons(Grid grid);

        Grid getGrid(string name);
    }
}
