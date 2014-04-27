using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD_29
{
    class Bullet
    {
        public Bullet(int x, int y)
        {
            this.x = x;
            this.y = y;

            while (Game.window.IsOpen())
            {
                this.x + 1;
            }
        }
    }
}
