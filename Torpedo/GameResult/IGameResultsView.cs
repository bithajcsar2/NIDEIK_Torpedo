using System.Windows.Controls;

namespace Torpedo.GameResult
{
    interface IGameResultsView
    {
        DataGrid shareGrid { get; set; }

        IGameResultContoler shareContoler { get; set; }

        void show();
    }
}
