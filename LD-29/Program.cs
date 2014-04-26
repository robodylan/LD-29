﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Box2DX;
using 
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
			using (Game game = new Game("One Who Shall Not Be Named Yet")) game.Start(/* Game Args */);
		}
	}
}