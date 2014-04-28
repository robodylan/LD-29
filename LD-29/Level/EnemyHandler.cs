using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD_29.Level
{
	public class EnemyHandler
	{
		public List<Enemy> Enemies;

		public event EventHandler OnHit;

		public event EventHandler OnDead;

		private Texture slimeTex;

		private OffsetSprite sprite;

		public EnemyHandler()
		{
			Enemies = new List<Enemy>();
			slimeTex = new Texture("Content/Slime.png");
			sprite = new OffsetSprite(128, 128);
			sprite.Texture = slimeTex;
			sprite.Scale = new Vector2f(0.5f, 0.5f) * Global.Scale;
			sprite.Offset = new Vector2f(0, 64);
		}

		public void Add(float x, float y)
		{
			Enemy b = new Enemy(x, y);
			b.OnDead += Dead;
			b.OnAttack += Hit;
			Enemies.Add(b);
		}

		public void Draw(RenderTarget window)
		{
			for (int i = Enemies.Count - 1; i >= 0; i--)
			{
				sprite.Position = Offset(new Vector2f(Enemies[i].X, Enemies[i].Y));
				Console.WriteLine(sprite.Position);
				window.Draw(sprite);
			}
		}

		private void Dead(object enemy, EventArgs e)
		{
			OnDead(enemy, e);
			Remove((Enemy)enemy);
		}

		private void Hit(object enemy, EventArgs e)
		{
			OnHit(enemy, e);
		}

		public Vector2f Offset(Vector2f v)
		{
			return new Vector2f(v.X * 128 * Global.Scale, v.Y * 128 * Global.Scale) + Global.Offset - new Vector2f(32, 0);
		}

		public void Remove(Enemy b)
		{
			if (Enemies.Contains(b))
				Enemies.Remove(b);
		}
	}
}