using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LD_29
{
	public static class Program
	{
        public static Launcher l = new Launcher();
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
        [STAThread]
		private static void Main()
		{
            l.ShowDialog();
            l.Close();
            new Bullet(0,0);
		}
	}
}