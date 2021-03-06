﻿using FarseerPhysics.Dynamics;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD_29
{
	public class BulletHandler
	{
		public List<Bullet> Bullets;

		public event HitEventHandler OnHit;

		private CircleShape shape;

		public BulletHandler()
		{
			shape = new CircleShape(4, 8);
			shape.FillColor = new Color(120, 154, 188);
			Bullets = new List<Bullet>();
		}

		public void Add(float x, float y, float rad)
		{
			Bullet b = new Bullet(x, y, rad);
			b.OnBreak += Break;
			b.OnHit += Hit;
			Bullets.Add(b);
		}

		public void Draw(RenderTarget window)
		{
			for (int i = Bullets.Count - 1; i >= 0; i--)
			{
				shape.Position = Offset(new Vector2f(Bullets[i].X, Bullets[i].Y));
				window.Draw(shape);
			}
		}

		private void Break(object bullet, EventArgs e)
		{
			Remove((Bullet)bullet);
		}

		private void Hit(object bullet, Fixture fix)
		{
			if (OnHit != null)
				OnHit(bullet, fix);
			Remove((Bullet)bullet);
		}

		public Vector2f Offset(Vector2f v)
		{
			return new Vector2f(v.X * 128 * Global.Scale, v.Y * 128 * Global.Scale) + Global.Offset - new Vector2f(32, 0);
		}

		public void Remove(Bullet b)
		{
			if (Bullets.Contains(b))
				Bullets.Remove(b);
		}
	}
}