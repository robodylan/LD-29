using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace LD_29
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
<<<<<<< HEAD
            using (Game game = new Game("aadsd")) game.Start(/* Game Args */);
=======
            Game game = new Game(""); 
            game.Start(/* Game Args */);
>>>>>>> 95c894c99654dd9f8028db02a6d01eb54cec67e9
		}
	}
}