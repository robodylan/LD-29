﻿using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LD_29.Level
{
	public class Level
	{
		private Texture block;
		private Sprite blockSprite;
		private int width, height;
		private List<Block> blocks, topLayer, bottomLayer, physBlocks;
		private Position start, finish, secret;
		private bool hasSecret;
		private float scale;
		private string name;
		private int next, snext;
		private Stopwatch sw;

		public Level(int width, int height, List<Block> blocks, List<Block> topLayer, List<Block> bottomLayer, Position start, Position finish, Position secret, bool hasSecret, float scale, string name, int next, int snext)
		{
			this.width = width;
			this.height = height;
			this.blocks = blocks;
			this.physBlocks = new List<Block>();
			this.topLayer = topLayer;
			this.bottomLayer = bottomLayer;
			this.start = start;
			this.finish = finish;
			this.secret = secret;
			this.hasSecret = hasSecret;
			this.name = name;
			this.next = next;
			this.snext = snext;
			scale *= 0.5f;
			this.scale = scale;
			block = new Texture("Content/block.png");
			block.Smooth = true;
			blockSprite = new Sprite();
			blockSprite.Texture = block;
			blockSprite.Scale = new SFML.Window.Vector2f(scale, scale);
			sw = new Stopwatch();
		}

		public void SimplifyPhysics()
		{
			Block[,] blocks = new Block[width, height];
			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
					blocks[x, y] = new Block() { ID = -1 };
			foreach (Block b in this.blocks)
				blocks[b.Position.X, b.Position.Y] = b;
			int oldCount = this.blocks.Count;
			physBlocks.Clear();
			Block[,] outline = new Block[width, height];
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (blocks[x, y].ID != -1)
					{
						// -- Remove all hidden blocks --
						// Check near blocks
						bool top, bottom, left, right;
						top = blocks[x, System.Math.Max(System.Math.Min(height - 1, y - 1), 0)].ID != -1;
						bottom = blocks[x, System.Math.Max(System.Math.Min(height - 1, y + 1), 0)].ID != -1;
						left = blocks[System.Math.Max(System.Math.Min(width - 1, x - 1), 0), y].ID != -1;
						right = blocks[System.Math.Max(System.Math.Min(width - 1, x + 1), 0), y].ID != -1;

						// Set outline array
						if (!top || !bottom || !left || !right) outline[x, y] = blocks[x, y];
						else outline[x, y] = new Block() { ID = -1, Position = new Position() { X = x, Y = y } };
					}
					else outline[x, y] = new Block() { ID = -1, Position = new Position() { X = x, Y = y } };
				}
			}
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (outline[x, y].ID != -1) physBlocks.Add(outline[x, y]);
				}
			}
			int tcount = physBlocks.Count;
			Console.WriteLine("Minified(" + tcount + "/" + oldCount + "): " + System.Math.Round(tcount / (float)oldCount * 100, 2) + "%");
		}

		public void StartTime()
		{
			sw.Start();
		}

		public TimeSpan StopTime()
		{
			sw.Start();
			return sw.Elapsed;
		}

		public void ComputePhysics()
		{
			Console.WriteLine("Computing Physics");
			Console.WriteLine("Simplifying Level...");
			StartTime();
			SimplifyPhysics();
			Console.WriteLine("Done in " + StopTime().ToString() + "!");
			Console.WriteLine("Adding Blocks to Physics world");
			StartTime();

			foreach (Block b in physBlocks)
			{
				new BoxShape(0.5f, 0.5f, new PhysicsParams() { Static = true, X = b.Position.X, Y = b.Position.Y, Ghost = b.ID == Block.Coin });
			}
			Console.WriteLine("Done in " + StopTime().ToString() + "!");
		}

		public void SpawnEntity(int type, Position p)
		{
		}

		public void AddEntitySpawner(int type, Position p, Position defaultVelocity)
		{
		}

		public void Draw(RenderWindow r)
		{
			foreach (Block b in blocks)
			{
				blockSprite.Position = new SFML.Window.Vector2f(b.Position.X * scale * block.Size.X, b.Position.Y * scale * block.Size.Y);
				r.Draw(blockSprite);
			}
		}
	}
}