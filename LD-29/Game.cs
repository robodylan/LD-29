﻿using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using LD_29.Level;
using Microsoft.Xna.Framework;

//Import SFML
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		public int Score = 0;
		private Texture tex;
		private PhysicsSprite spr;
		private OffsetSprite raycpoint;
		private int Width, Height;

		public bool grappled { get; set; }

		private RopeJoint graplingJoint { get; set; }

		private Body grapBody = null;

		private CapsuleShape character;

		private Level.Level testlevel;

		private Player player;

		private bool OnGround = false;

		private Vector2 velocityOld;

		private Vertex[] line;

		private float rota = 0;

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
		/// Load things
		/// </summary>
		public void Load()
		{
			testlevel = LevelLoader.LoadLevel("Level" + Score + "/");
			Score += 1;
			testlevel.ComputePhysics();
			character = new CapsuleShape(0.01f, 0.75f, new PhysicsParams() { Static = false, Density = 20.0f, X = 6, Y = 56, IsSleeping = false, FixedRotation = true, Friction = 0.5f });
			tex = new Texture("Content/character.png");
			spr = new PhysicsSprite(character.Body, character.Width, character.Height);
			raycpoint = new OffsetSprite(10, 10);
			raycpoint.Texture = tex;
			raycpoint.Scale = new Vector2f(Global.Scale, Global.Scale) * 0.1f;
			spr.Texture = tex;
			Console.WriteLine(Global.Scale);
			spr.Scale = new Vector2f(Global.Scale * 3, Global.Scale * 3);
			spr.Offset = new Vector2f(32, -64 - 128) * Global.Scale;
			player = new Player();
			line = new Vertex[3];
			line[0] = new Vertex(new Vector2f(), SFML.Graphics.Color.White);
			line[1] = new Vertex(new Vector2f(), SFML.Graphics.Color.White);
			line[2] = new Vertex(new Vector2f(), SFML.Graphics.Color.White);
		}

		/// <summary>
		/// Create window for drawing
		/// </summary>
		public void Start()
		{
			window = new RenderWindow(new VideoMode((uint)(screen.Width * 0.9f), (uint)(screen.Height * 0.9f)), title);
			Width = (int)window.Size.X;
			Height = (int)window.Size.Y;
			Global.GameResolution = new Vector2f(Width, Height);

			window.KeyPressed += window_KeyPressed;
			window.KeyReleased += window_KeyReleased;
			window.MouseButtonPressed += window_MouseButtonPressed;
			window.MouseButtonReleased += window_MouseButtonReleased;
			window.Closed += window_Closed;

			window.SetFramerateLimit(60);

			Load();

			// Begin Main Loop
			while (window.IsOpen())
			{
				// Update window, handle events
				window.DispatchEvents();

				// Update all
				Update();

				window.Clear(new SFML.Graphics.Color(25, 165, 190));

				// Draw Content
				Draw();

				// Bring drawn to user
				window.Display();
			}
		}

		private void window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
		{
			if (grappled)
			{
				grapBody.Dispose();
				graplingJoint.Enabled = false;
				grappled = false;
			}
		}

		private void window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
		{
			if (!grappled)
			{
				grappled = Graple();
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
			Global.Offset = -new Vector2f(character.Body.Position.X * spr.Texture.Size.X * Global.Scale - Width * 0.5f, character.Body.Position.Y * spr.Texture.Size.Y * Global.Scale - Height * 0.5f);

			OnGround = RayCastDistance(10, 0) <= 0.18f || RayCastDistance(10, 0.3f) <= 0.3f || RayCastDistance(10, 5.9f) <= 0.3f;

			if (OnGround)
			{
				if (IsKeyDown(Keyboard.Key.D))
				{
					character.Body.LinearVelocity += new Vector2(0.3f, 0.0f);
				}
				if (IsKeyDown(Keyboard.Key.A))
				{
					character.Body.LinearVelocity -= new Vector2(0.3f, 0.0f);
				}
				if (IsKeyDown(Keyboard.Key.Space))
				{
					character.Body.LinearVelocity = new Vector2(character.Body.LinearVelocity.X, -5.0f);
				}
			}
			else
			{
				if (IsKeyDown(Keyboard.Key.D))
				{
					character.Body.LinearVelocity += new Vector2(0.05f, 0.0f);
				}
				if (IsKeyDown(Keyboard.Key.A))
				{
					character.Body.LinearVelocity -= new Vector2(0.05f, 0.0f);
				}
			}
			if (character.Body.LinearVelocity == velocityOld) OnGround = true;
			velocityOld = character.Body.LinearVelocity;
			rota += 0.001f;
		}

		/// <summary>
		/// Draw Everything
		/// </summary>
		public void Draw()
		{
			Vector2 old = new Vector2();
			bool blue = false;
			for (int i = 0; i <= 360; i += 6)
			{
				//Vector2 r = RayCast(50, -1.57079632679f);
				Vector2 r = RayCast(50, i * 0.0174532925f + rota);
				if (old.X == 0 && old.Y == 0)
				{
					old = r;
					continue;
				}
				raycpoint.Position = to2f(r);
				Vector2f off = new Vector2f(32, 0);
				line[0].Position = new Vector2f(character.Body.Position.X * 128 * Global.Scale, (character.Body.Position.Y) * 128 * Global.Scale) + Global.Offset - off;
				line[1].Position = Offset(to2f(r)) - off;
				line[2].Position = Offset(to2f(old)) - off;
				SFML.Graphics.Color c = blue ? new SFML.Graphics.Color(0, 60, 70) : new SFML.Graphics.Color(0, 90, 100);
				line[0].Color = c;
				line[1].Color = c;
				line[2].Color = c;
				window.Draw(line, PrimitiveType.Triangles);
				blue = !blue;
				old = r;
			}
		}

		public Vector2f to2f(Vector2 v)
		{
			return new Vector2f(v.X, v.Y);
		}

		public Vector2f Offset(Vector2f v)
		{
			return new Vector2f(v.X * 128 * Global.Scale, v.Y * 128 * Global.Scale) + Global.Offset;
		}

		public Vector2 RayCast(float maxRayLength, float rotation)
		{
			RayCastInput input = new RayCastInput();
			input.Point1 = character.Body.Position;
			input.Point2 = input.Point1 + maxRayLength * new Vector2((float)Math.Sin(rotation), (float)Math.Cos(rotation));
			input.MaxFraction = 1;
			float closestFraction = 1;
			foreach (Body b in PhysConfig.world.BodyList)
			{
				foreach (Fixture f in b.FixtureList)
				{
					RayCastOutput output;
					if (!f.RayCast(out output, ref input, 0))
						continue;
					if (output.Fraction < closestFraction)
					{
						closestFraction = output.Fraction;
					}
				}
			}
			return input.Point1 + closestFraction * (input.Point2 - input.Point1) * 1.5f;
		}

		public bool Graple()
		{
			Vector2f pos1 = Offset(to2f(character.Body.Position));
			Vector2i pos2 = Mouse.GetPosition(window) + new Vector2i(32, 0);
			float rad = (float)Math.Atan2(pos2.X - pos1.X, pos2.Y - pos1.Y);
			Vector2 ray = RayCast(10, rad);
			float len = RayCastDistance(10, rad);
			if (len < 900)
			{
				grapBody = BodyFactory.CreateCircle(PhysConfig.world, 0.1f, 0.0f, ray);
				grapBody.IsStatic = true;
				graplingJoint = new RopeJoint(character.Body, grapBody, new Vector2(), new Vector2());
				graplingJoint.MaxLength = len * 16;
				PhysConfig.world.AddJoint(graplingJoint);
				return true;
			}
			return false;
		}

		public float RayCastDistance(float maxRayLength, float rotation)
		{
			RayCastInput input = new RayCastInput();
			input.Point1 = character.Body.Position;
			input.Point2 = input.Point1 + maxRayLength * new Vector2((float)Math.Sin(rotation), (float)Math.Cos(rotation));
			input.MaxFraction = 1;
			float closestFraction = 1;
			foreach (Body b in PhysConfig.world.BodyList)
			{
				foreach (Fixture f in b.FixtureList)
				{
					RayCastOutput output;
					if (!f.RayCast(out output, ref input, 0))
						continue;
					if (output.Fraction < closestFraction)
					{
						closestFraction = output.Fraction;
					}
				}
			}
			return closestFraction;
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