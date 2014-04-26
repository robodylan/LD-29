using LD_29.Level;

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
    class Game : IDisposable
    {


        public static string Title = "One Who Shall Not Be Named Yet";
        //Set Screen Boundries
        public static Rectangle r = Screen.PrimaryScreen.Bounds;
        //Create window for drawing
        public static RenderWindow window = new RenderWindow(new VideoMode((uint)(r.Width * 0.9f), (uint)(r.Height * 0.9f)), Title);
<<<<<<< HEAD

=======
        private string p;

        public Game(string p)
        {
            // TODO: Complete member initialization
            this.p = p;
        }
>>>>>>> 95c894c99654dd9f8028db02a6d01eb54cec67e9
        public void Start()
        {
            //Begin Main Loop
            while (window.IsOpen())
            {
                //Check For Window Events
                window.DispatchEvents();

                //Update all 
                Update();
                //Update window
                window.Display();
            }

        }

        //Method for drawing all assets
        public static void Update()
        {

        }
    }
}
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

		private Texture tex;
		private Sprite spr;

		private BoxShape character;

		private Level.Level testlevel;

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

		public Game()
		{

			Keys = new bool[256];
		}

		/// <summary>
		/// Load things
		/// </summary>
		public void Load()
		{
			testlevel = LevelLoader.LoadLevel("Level0/");
			testlevel.ComputePhysics();
			character = new BoxShape(1, 1, new PhysicsParams() { Static = false, Density = 1.0f, X = 6, Y = 80, IsSleeping = false });
			tex = new Texture("Content/block.png");
			spr = new Sprite(tex);
			spr.Scale = new Vector2f(0.15f, 0.15f);
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

			Load();

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
			PhysConfig.world.Step(1 / 60.0f);
			spr.Position = new Vector2f(character.Body.Position.X * 64 * 0.15f, character.Body.Position.Y * 64 * 0.15f);
		}

		/// <summary>
		/// Draw Everything
		/// </summary>
		public void Draw()
		{
			testlevel.Draw(window);
			window.Draw(spr);
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
