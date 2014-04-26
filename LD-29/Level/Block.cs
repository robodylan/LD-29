using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD_29.Level
{
	public class Block
	{
		public static int Coin = 0;
		public static int Stone = 1;
		public static int Dirt = 2;
		public static int Ladder = 3;
		public static int Torch = 4;
		public static int Platform = 5;

		public Position Position { get; set; }

		public int ID { get; set; }
	}
}