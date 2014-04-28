using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace LD_29.Level
{
	public static class ImageExt
	{
		public static void HandlePixels(this Bitmap i, Action<int, int, Color> a)
		{
			for (int x = 0; x < i.Width; x++)
			{
				for (int y = 0; y < i.Height; y++)
				{
					a(x, y, i.GetPixel(x, y));
				}
			}
		}
	}

	public class LevelLoader
	{
		public static int GetBlockID(Color c)
		{
			return 1;
		}

		public static Level LoadLevel(string path, EnemyHandler e)
		{
			JSONDescription d = JsonConvert.DeserializeObject<JSONDescription>(File.ReadAllText("Content/Levels/" + path + "level.json"));
			string name = d.Name;
			float scale = Math.Min(Math.Max(d.Scale, 0.5f), 2.0f);
			Position start = new Position() { X = d.Start.X, Y = d.Start.Y };
			Position finish = new Position() { X = d.Finish.X, Y = d.Finish.Y };
			Position secret = new Position() { X = d.SecretExit.X, Y = d.SecretExit.Y };
			bool hasSecret = d.HasSecretExit;
			int next = d.NextLevel;
			int snext = d.SecretNextLevel;

			List<Block> block = new List<Block>();
			List<Block> above = new List<Block>();
			List<Block> below = new List<Block>();

			Bitmap cmap = (Bitmap)Bitmap.FromFile("Content/Levels/" + path + "/" + d.CollisionMap);
			cmap.HandlePixels((x, y, color) =>
			{
				if (color.R == color.G && color.G == color.B && color.R == 0)
					block.Add(new Block() { Position = new Position() { X = x, Y = y }, ID = GetBlockID(color) });
			});

			Level l = new Level(cmap.Width, cmap.Height, block, above, below, start, finish, secret, hasSecret, scale, name, next, snext, e);
			cmap.HandlePixels((x, y, color) =>
			{
				if (color.R == 255 && color.G == 255 && color.B == 0)
					l.AddCoin(x, y, false);
				if (color.R == 0 && color.G == 255 && color.B == 255)
					l.AddCoin(x, y, true);
				if (color.R == 0 && color.G == 255 && color.B == 0)
					l.SpawnEntity(new Position() { X = x, Y = y });
			});

			return l;
		}
	}
}