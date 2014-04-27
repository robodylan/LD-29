using FarseerPhysics.Dynamics;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD_29
{
	public class PhysicsSprite : Sprite
	{
		private Body body;
		public Vector2f Offset;
		public float Width, Height;

		public PhysicsSprite(Body b, float width, float height)
			: base()
		{
			body = b;
			Offset = new Vector2f(0, 0);
			Width = width;
			Height = height;
		}

		public void DrawTransformed(RenderTarget r, RenderStates s)
		{
			base.Position = new Vector2f(body.Position.X * Texture.Size.X * Global.Scale, (body.Position.Y) * Texture.Size.Y * Global.Scale) + Global.Offset + Offset;
			base.Rotation = body.Rotation * 57.2957804904f;

			Origin = new Vector2f(Texture.Size.X * Width * 0.5f, 0);
			base.Draw(r, s);
		}
	}
}