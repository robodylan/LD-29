using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD_29
{
	public class Bullet
	{
		public float X { get { return body.Position.X; } }

		public float Y { get { return body.Position.Y; } }

		protected Body body;

		public event HitEventHandler OnHit;

		public event EventHandler OnBreak;

		public Bullet(float x, float y, float deg)
		{
			body = BodyFactory.CreateCircle(PhysConfig.world, 0.2f, 0.1f, new Vector2(x, y));
			body.IsBullet = true;
			body.IsStatic = false;
			body.CollisionCategories = Category.Cat16;
			body.CollidesWith = Category.Cat14 | Category.Cat15;
			body.OnCollision += body_OnCollision;
			body.LinearVelocity = new Vector2((float)Math.Sin(deg) * 10.0f, (float)Math.Cos(deg) * 10.0f);
			body.IgnoreGravity = true;
			body.GravityScale = 0;
			body.Restitution = 1.0f;
		}

		private bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
		{
			if (fixtureA.CollisionCategories == Category.Cat16)
			{
				if (fixtureB.CollisionCategories == Category.Cat15)
				{
					if (OnHit != null)
						OnHit(this, fixtureB);
				}
				else
				{
					if (OnBreak != null)
						OnBreak(this, EventArgs.Empty);
				}
			}
			return true;
		}
	}
}