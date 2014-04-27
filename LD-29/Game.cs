﻿using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
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

		private Texture tex;
		private PhysicsSprite spr;
		private OffsetSprite raycpoint;
		private int Width, Height;

		private CapsuleShape character;

		private Level.Level testlevel;

		private Player player;

		private bool OnGround = false;

		private Vector2 velocityOld;

		private Vertex[] line;

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
			testlevel = LevelLoader.LoadLevel("Level0/");
			testlevel.ComputePhysics();
			character = new CapsuleShape(0.01f, 0.75f, new PhysicsParams() { Static = false, Density = 1.0f, X = 6, Y = 56, IsSleeping = false, FixedRotation = true, Friction = 0.5f });
			tex = new Texture("Content/character.png");
			spr = new PhysicsSprite(character.Body, character.Width, character.Height);
			raycpoint = new OffsetSprite(10, 10);
			raycpoint.Texture = tex;
			raycpoint.Scale = new Vector2f(Global.Scale, Global.Scale) * 0.1f;
			character.Body.OnSeparation += Body_OnSeparation;
			character.Body.OnCollision += Body_OnCollision;
			spr.Texture = tex;
			Console.WriteLine(Global.Scale);
			spr.Scale = new Vector2f(Global.Scale * 3, Global.Scale * 3);
			spr.Offset = new Vector2f(32, -64 - 128) * Global.Scale;
			player = new Player();
			line = new Vertex[2];
			line[0] = new Vertex(new Vector2f(), SFML.Graphics.Color.White);
			line[1] = new Vertex(new Vector2f(), SFML.Graphics.Color.White);
		}

		private bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
		{
			OnGround = true;
			return true;
		}

		public void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB)
		{
			OnGround = false;
		}

		/// <summary>
		/// Create window for drawing
		/// </summary>
		public void Start()
		{
			window = new RenderWindow(new VideoMode((uint)(screen.Width * 0.9f), (uint)(screen.Height * 0.9f)), title);
			Width = (int)window.Size.X;
			Height = (int)window.Size.Y;

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
			Global.Offset = -new Vector2f(character.Body.Position.X * spr.Texture.Size.X * Global.Scale - Width * 0.5f, character.Body.Position.Y * spr.Texture.Size.Y * Global.Scale - Height * 0.5f);

			if (OnGround)
			{
				if (IsKeyDown(Keyboard.Key.D))
				{
					character.Body.LinearVelocity += new Vector2(1.0f, 0.0f);
				}
				if (IsKeyDown(Keyboard.Key.A))
				{
					character.Body.LinearVelocity -= new Vector2(1.0f, 0.0f);
				}
				if (IsKeyDown(Keyboard.Key.Space))
				{
					Console.WriteLine(character.Body.LinearVelocity.Y);
					if (character.Body.LinearVelocity.Y == 0)
						character.Body.LinearVelocity += new Vector2(0.0f, -10.0f);
				}
			}
			else
			{
				if (IsKeyDown(Keyboard.Key.D))
				{
					character.Body.LinearVelocity += new Vector2(0.1f, 0.0f);
				}
				if (IsKeyDown(Keyboard.Key.A))
				{
					character.Body.LinearVelocity -= new Vector2(0.1f, 0.0f);
				}
			}
			if (character.Body.LinearVelocity == velocityOld) OnGround = true;
			velocityOld = character.Body.LinearVelocity;
		}

		/// <summary>
		/// Draw Everything
		/// </summary>
		public void Draw()
		{
			testlevel.Draw(window);
			spr.DrawTransformed(window, RenderStates.Default);
			for (int i = 0; i < 360; i += 10)
			{
				//Vector2 r = RayCast(50, -1.57079632679f);
				Vector2 r = RayCast(50, i * 0.0174532925f);
				raycpoint.Position = new Vector2f(r.X, r.Y);
				raycpoint.DrawTransformed(window, RenderStates.Default);
				line[0].Position = new Vector2f(character.Body.Position.X * 128 * Global.Scale, (character.Body.Position.Y) * 128 * Global.Scale) + Global.Offset;
				line[1].Position = raycpoint.Position;
				Console.WriteLine(line[0].Position);
				Console.WriteLine(line[1].Position);
				window.Draw(line, PrimitiveType.Lines);
			}
		}

		public Vector2f to2f(Vector2 v)
		{
			return new Vector2f(v.X, v.Y);
		}

		public Vector2f Offset(Vector2f v)
		{
			return new Vector2f(v.X * Global.Scale, v.Y * Global.Scale) + Global.Offset;
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
			return input.Point1 + closestFraction * (input.Point2 - input.Point1);
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