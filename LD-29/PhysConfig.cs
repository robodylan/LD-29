﻿using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD_29
{
	public class PhysConfig
	{
		public static World world;

		static PhysConfig()
		{
			world = new World(new Vector2(0, 20));
		}
	}
}