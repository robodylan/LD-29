using FarseerPhysics.Collision;
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
using System.Linq;
using System.Text;
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

		private Sprite glowDisplay;

		private Font defaultFont;

		public bool grappled { get; set; }

		private RopeJoint graplingJoint { get; set; }

		private Body grapBody = null;

		private CapsuleShape character;

		private BulletHandler bullets;

		private Level.Level testlevel;

		private Player player;

		private bool OnGround = false;

		private Vector2 velocityOld;

		private RenderTexture glowMap;
		private RenderTexture glowMap2;
		private RenderTexture post;

		private Music music;

		private bool shooting;

		private Vertex[] line;

		private float rota = 0;

		private int canShoot = 0;

		/// <summary>
		/// Internal title
		/// </summary>
		private string title;

		private RenderStates blurShaderHorizontal;

		private Shader blurHorizontal;

		private RenderStates blurShaderVertical;

		private Shader blurVertical;

		private RenderStates finalShader;

		private Shader final;

		private EnemyHandler enemies;

		public bool[] Keys;

		/// <summary>
		/// Boolean wether the window can close if you hit X
		/// </summary>
		public bool canClose;

		/// <summary>
		/// Set Screen Boundries
		/// </summary>
		private System.Drawing.Rectangle screen;

		/// <summary>
		/// Handle to a window
		/// </summary>
		public RenderWindow window;

		private float vol;

		private bool Sound;

		public Game(string Title, bool sound, float volume)
		{
			screen = Screen.PrimaryScreen.Bounds;
			title = Title;
			Keys = new bool[256];
			Sound = sound;
			vol = volume;
		}

		/// <summary>
		/// Load things
		/// </summary>
		public void Load()
		{
			// Create Post Process
			glowMap = new RenderTexture(window.Size.X, window.Size.Y);
			glowMap2 = new RenderTexture(window.Size.X, window.Size.Y);
			post = new RenderTexture(window.Size.X, window.Size.Y);
			glowDisplay = new Sprite();
			glowDisplay.TextureRect = new IntRect(0, 0, (int)window.Size.X, (int)window.Size.Y);

			blurHorizontal = new Shader("Content/default.vs", "Content/blurh.fs");
			blurHorizontal.SetParameter("texelSize", new Vector2f(1 / (float)window.Size.X, 1 / (float)window.Size.Y));

			blurShaderHorizontal = new RenderStates(RenderStates.Default);
			blurShaderHorizontal.Shader = blurHorizontal;

			blurVertical = new Shader("Content/default.vs", "Content/blurv.fs");
			blurVertical.SetParameter("texelSize", new Vector2f(1 / (float)window.Size.X, 1 / (float)window.Size.Y));

			blurShaderVertical = new RenderStates(RenderStates.Default);
			blurShaderVertical.Shader = blurVertical;

			final = new Shader("Content/default.vs", "Content/final.fs");
			final.SetParameter("texelSize", new Vector2f(1 / (float)window.Size.X, 1 / (float)window.Size.Y));

			finalShader = new RenderStates(RenderStates.Default);
			finalShader.Shader = final;

			// Load Level
			enemies = new EnemyHandler();
			enemies.OnHit += (s, e) => { Console.WriteLine("Au"); };
			enemies.OnDead += (s, e) => { Console.WriteLine("Ded"); };

			testlevel = LevelLoader.LoadLevel("Level" + Score + "/", enemies);
			Score += 1;

			// Load Physics
			testlevel.ComputePhysics();

			// Load Character
			character = new CapsuleShape(0.01f, 0.2f, new PhysicsParams() { Static = false, Density = 20.0f, X = 6, Y = 56, IsSleeping = false, FixedRotation = true, Friction = 0.5f });
			character.Body.CollisionCategories = Category.Cat2;
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
			defaultFont = new Font("Content/consolas.ttf");

			line = new Vertex[3];

			line[0] = new Vertex(new Vector2f(), Color.White);
			line[1] = new Vertex(new Vector2f(), Color.White);
			line[2] = new Vertex(new Vector2f(), Color.White);

			if (Sound)
			{
				// Load Music
				music = new Music("Content/BackgroundMusic.wav");
				music.Loop = true;
				music.Volume = vol;
				music.Play();
			}

			// Bullets
			shooting = false;
			bullets = new BulletHandler();
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
			window.MouseWheelMoved += window_MouseWheelMoved;
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

				window.Clear(Color.Black);
				post.Clear(Color.Black);
				glowMap.Clear(Color.Black);
				glowMap2.Clear(Color.Black);

				// Draw Content
				Draw();

				// Bring drawn to user
				window.Display();
			}
		}

		private void window_MouseWheelMoved(object sender, MouseWheelEventArgs e)
		{
			if (grappled)
			{
				graplingJoint.MaxLength += e.Delta * 0.1f;
			}
		}

		private void window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
		{
			if (grappled && e.Button == Mouse.Button.Right)
			{
				grapBody.Dispose();
				graplingJoint.Enabled = false;
				grappled = false;
			}
			if (e.Button == Mouse.Button.Left)
			{
				shooting = false;
			}
		}

		private void window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
		{
			if (!grappled && e.Button == Mouse.Button.Right)
			{
				grappled = Graple();
			}
			if (e.Button == Mouse.Button.Left)
			{
				shooting = true;
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

			OnGround = RayCastDistance(10, 0) <= 0.07f || RayCastDistance(10, 0.1f) <= 0.1f || RayCastDistance(10, 6.1f) <= 0.1f;

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
					character.Body.LinearVelocity = new Vector2(character.Body.LinearVelocity.X, -8.0f);
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
				if (IsKeyDown(Keyboard.Key.Space))
				{
					character.Body.LinearVelocity += new Vector2(0, -0.01f);
				}
			}

			if (!OnGround && character.Body.LinearVelocity.X == 0 && character.Body.LinearVelocity.Y == 0)
				character.Body.LinearVelocity += new Vector2(0, -1.0f);
			if (character.Body.LinearVelocity == velocityOld) OnGround = true;
			velocityOld = character.Body.LinearVelocity;

			//rota += 0.001f;

			canShoot--;
			canShoot = canShoot < 0 ? 0 : canShoot;
			if (shooting)
			{
				if (canShoot == 0)
				{
					Shoot();
					canShoot = 10;
				}
			}
            //Display Score
            window.Draw(new Text("Hello",defaultFont));
		}

		public void Shoot()
		{
			Vector2f pos1 = Offset(to2f(character.Body.Position));
			Vector2i pos2 = Mouse.GetPosition(window) + new Vector2i(32, 0);
			float rad = (float)Math.Atan2(pos2.X - pos1.X, pos2.Y - pos1.Y);
			bullets.Add(character.Body.Position.X, character.Body.Position.Y, rad);
		}

		/// <summary>
		/// Draw Everything
		/// </summary>
		public Sprite Char = new Sprite(new Texture("Content/Player.png"), new IntRect(0, 0, 64, 64));

		public void Draw()
		{
			Vector2 old = new Vector2();
			for (int i = 0; i <= 360; i += 12)
			{
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
				line[0].Color = new Color(0, 60, 70);
				line[1].Color = new Color(0, 30, 35);
				post.Draw(line, PrimitiveType.Lines);
				line[0].Color = Color.White;
				line[1].Color = Color.White;
				line[2].Color = Color.White;
				line[2].Position = Offset(to2f(old)) - off;
				glowMap.Draw(line, PrimitiveType.Triangles);
				old = r;
			}

			testlevel.Draw(post);

			if (grappled)
			{
				line[0].Position = new Vector2f(character.Body.Position.X * 128 * Global.Scale, (character.Body.Position.Y) * 128 * Global.Scale) + Global.Offset - new Vector2f(32, 0);
				line[1].Position = Offset(to2f(grapBody.Position)) - new Vector2f(32, 0);
				line[0].Color = new Color(100, 100, 100);
				line[1].Color = new Color(100, 100, 100);
				post.Draw(line, PrimitiveType.Lines);
			}

			enemies.Draw(post);

			//Main Character;
			Char.Position = new Vector2f(Player.CameraX + window.Size.X / 2 - 64, Player.CameraY + window.Size.Y / 2 - 32);
			post.Draw(Char);

			bullets.Draw(post);
			glowDisplay.Texture = glowMap.Texture;
			glowMap2.Draw(glowDisplay, blurShaderHorizontal);
			glowDisplay.Texture = glowMap2.Texture;
			glowMap.Draw(glowDisplay, blurShaderVertical);
			glowDisplay.Texture = glowMap.Texture;
			finalShader.Shader.SetParameter("glow", glowDisplay.Texture);
			finalShader.Shader.SetParameter("texture", post.Texture);
			window.Draw(glowDisplay, finalShader);
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
					if (f.CollisionCategories == Category.Cat16 || f.CollisionCategories == Category.Cat25)
						continue;
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

		public bool Graple()
		{
			Vector2f pos1 = Offset(to2f(character.Body.Position));
			Vector2i pos2 = Mouse.GetPosition(window) + new Vector2i(32, 0);
			float rad = (float)Math.Atan2(pos2.X - pos1.X, pos2.Y - pos1.Y);
			Vector2 ray = RayCast(25, rad);
			float len = RayCastDistance(25, rad);
			if (len < 1)
			{
				grapBody = BodyFactory.CreateCircle(PhysConfig.world, 0.1f, 0.0f, ray);
				grapBody.IsStatic = true;
				graplingJoint = new RopeJoint(character.Body, grapBody, new Vector2(), new Vector2());
				graplingJoint.MaxLength = len * 24.8f;
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
					if (f.CollisionCategories == Category.Cat16 || f.CollisionCategories == Category.Cat25)
						continue;
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