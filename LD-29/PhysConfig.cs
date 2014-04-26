using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD_29
{
	public class PhysConfig
	{
		public static World world;

		static PhysConfig()
		{
			AABB worldAABB = new AABB();
			worldAABB.LowerBound.Set(-1000.0f);
			worldAABB.UpperBound.Set(1000.0f);
			world = new World(worldAABB, new Vec2(0, -9.7f), true);
		}
	}
}