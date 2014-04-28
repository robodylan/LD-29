using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD_29.Level
{
	public class Coin
	{
		private Body body;
		public Position Position;
		public bool Ruby;

		public event EventHandler OnCollected;

		public Coin(int x, int y, bool ruby)
		{
			Position = new Position() { X = x, Y = y };
			body = BodyFactory.CreateCircle(PhysConfig.world, 0.6f, 1.0f);
			body.Position = new Vector2(x + 0.5f, y);
			body.CollisionCategories = Category.Cat25;

			body.BodyType = BodyType.Static;

			body.IsSensor = true;
			body.OnCollision += body_OnCollision;
			this.Ruby = ruby;
		}

		private bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
		{
			if (fixtureB.CollisionCategories == Category.Cat2)
			{
				if (Ruby) Global.Rubies++;
				else Global.Coins++;
				body.Dispose();
				if (OnCollected != null) OnCollected(this, EventArgs.Empty);
			}
			return false;
		}
	}
}