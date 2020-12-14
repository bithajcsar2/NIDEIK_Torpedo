using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo.GameResult
{
    public interface IGameResultContoler
    {
        void FillDataGridWithResult();
        void WriteJson(string winner, string loser, int numOfRounds, int p1HitCount, int p2HitCount, List<Ship> p1Ships, List<Ship> p2Ships);
        List<GameResultModel> ReadJson();
    }
}
