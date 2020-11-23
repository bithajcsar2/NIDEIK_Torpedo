using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo
{
    class Ship
    {
        public int Length { get; set; }

        public List<int[]> coordinates = new List<int[]>();

        public int hits = 0;

        public bool isDead = false;

        public Ship(int length)
        {
            this.Length=length;
        }
    }
}
