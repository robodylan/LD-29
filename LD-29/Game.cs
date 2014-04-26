//Import SFML
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LD_29
{
	public class Game : IDisposable
	{
		/// <summary>
		/// Title of the window
		/// </summary>
		public string Title
		{
			get
			{
				return title;
			}

			set
			{
				window.SetTitle(value);
				title = value;
			}
		}

		/// <summary>
		/// Internal title
		/// </summary>
		private string title;

		public bool[] Keys;

		/// <summary>
		/// Boolean wether the window can close if you hit X
		/// </summary>
		public bool canClose;

		/// <summary>
		/// Set Screen Boundries
		/// </summary>
		private Rectangle screen;

		/// <summary>
		/// Handle to a window
		/// </summary>
		public RenderWindow window;

		public Game(string Title)
		{
			screen = Screen.PrimaryScreen.Bounds;
			title = Title;
			Keys = new bool[256];
		}

		/// <summary>
		/// Create window for drawing
		/// </summary>
		public void Start()
		{
			window = new RenderWindow(new VideoMode((uint)(screen.Width * 0.9f), (uint)(screen.Height * 0.9f)), title);

			window.KeyPressed += window_KeyPressed;
			window.KeyReleased += window_KeyReleased;
			window.Closed += window_Closed;

			// Begin Main Loop
			while (window.IsOpen())
			{
				// Update window, handle events
				window.DispatchEvents();

				// Update all
				Update();

				window.Clear(SFML.Graphics.Color.Black);

				// Draw Content
				Draw();

				// Bring drawn to user
				window.Display();
			}
		}

		private void window_Closed(object sender, EventArgs e)
		{
#if !DEBUG
			canClose = MessageBox.Show("Do you really want to close the game?", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.Yes;
			if (canClose)
#endif
			window.Close();
		}

		private void window_KeyReleased(object sender, SFML.Window.KeyEventArgs e)
		{
			if ((int)e.Code > -1 && (int)e.Code < 256)
				Keys[(int)e.Code] = false;
		}

		private void window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
		{
			if ((int)e.Code > -1 && (int)e.Code < 256)
				Keys[(int)e.Code] = true;
		}

		/// <summary>
		/// Update Game Logic
		/// </summary>
		public void Update()
		{
		}

		/// <summary>
		/// Draw Everything
		/// </summary>
		public void Draw()
		{
		}

		public bool IsKeyDown(Keyboard.Key key)
		{
			if ((int)key > -1 && (int)key < 256)
				return Keys[(int)key];
			return false;
		}

		/// <summary>
		/// Unload Content
		/// </summary>
		public void Dispose()
		{
		}
	}
}