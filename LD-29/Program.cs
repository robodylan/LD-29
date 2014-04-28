using System;
using System.Collections.Generic;
using System.Linq;
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
			Application.EnableVisualStyles();
			l.ShowDialog();
		}
	}
}