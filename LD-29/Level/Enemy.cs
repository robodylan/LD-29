using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD_29.Level
{
	public class Enemy
	{
		public float X { get { return body.Position.X; } }

		public float Y { get { return body.Position.Y; } }

		protected Body body;

		public event EventHandler OnDead;

		public event EventHandler OnAttack;

		public Enemy(float x, float y)
		{
			body = BodyFactory.CreateCircle(PhysConfig.world, 0.2f, 0.1f, new Vector2(x, y));
			body.IsStatic = false;
			body.CollisionCategories = Category.Cat15;
			body.CollidesWith = Category.Cat2 | Category.Cat16 | Category.Cat14;
			body.OnCollision += body_OnCollision;
			body.FixedRotation = true;
		}

		private bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
		{
			if (fixtureA.CollisionCategories == Category.Cat15)
			{
				if (fixtureB.CollisionCategories == Category.Cat2)
				{
					if (OnAttack != null)
						OnAttack(this, EventArgs.Empty);
				}
				else if (fixtureB.CollisionCategories == Category.Cat16)
				{
					if (OnDead != null)
						OnDead(this, EventArgs.Empty);
				}
			}
			return true;
		}
	}
}