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
        string P1Name { get; set; }
        string P2Name { get; set; }

        bool turn { get; set; }
    }
}
