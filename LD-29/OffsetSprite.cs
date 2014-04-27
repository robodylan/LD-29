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
	public class OffsetSprite : Sprite
	{
		public Vector2f Offset;
		public float Width, Height;

		public OffsetSprite(float width, float height)
			: base()
		{
			Offset = new Vector2f(0, 0);
			Width = width;
			Height = height;
		}

		public void DrawTransformed(RenderTarget r, RenderStates s)
		{
			base.Position = new Vector2f(Position.X * Texture.Size.X * Global.Scale, Position.Y * Texture.Size.Y * Global.Scale) + Global.Offset + Offset;

			Origin = new Vector2f(Texture.Size.X * Width * 0.5f, 0);

			if (Position.X < 0 || Position.Y < 0 || Position.X > Width + Global.GameResolution.X || Position.Y > Height + Global.GameResolution.Y)
				return;
			base.Draw(r, s);
		}
	}
}