using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo.GameResult
{
    public class GameResultModel
    {
        public string winner { get; set; }
        public string loser { get; set; }
        public int numOfRounds { get; set; }
        public int p1HitCount { get; set; }
        public int p2HitCount { get; set; }
        public string p1ShipsSats { get; set; }
        public string p2ShipsSats { get; set; }
    }
}
