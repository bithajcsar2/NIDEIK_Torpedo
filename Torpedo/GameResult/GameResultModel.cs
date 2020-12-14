using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo.GameResult
{
    public class GameResultModel
    {
        public string Winner { get; set; }
        public string Loser { get; set; }
        public int NumOfRounds { get; set; }
        public int P1HitCount { get; set; }
        public int P2HitCount { get; set; }
        public string P1ShipsSats { get; set; }
        public string P2ShipsSats { get; set; }
    }
}
