using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo
{
    public class Ship
    {
        public int Length { get; set; }

        public List<int[]> coordinates = new List<int[]>();

        public int hits = 0;

        public string name;

        public bool isDead = false;

        public Ship(int length, string name)
        {
            this.Length=length;
            this.name = name;
        }
    }
}
