using Box2DX;
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
	public class BoxShape
	{
		public Body Body { get; protected set; }

		public BoxShape(float diameterX, float diameterY, PhysicsParams p)
		{
			BodyDef def = new BodyDef();
			def.Position.Set(p.X, p.Y);
			def.IsSleeping = p.IsSleeping;
			def.Angle = p.Angle;
			def.AngularDamping = p.AngularDamping;
			def.AngularVelocity = p.AngularVelocity;
			def.IsBullet = p.CanSleep;
			def.FixedRotation = p.FixedRotation;
			def.LinearDamping = p.LinearDamping;
			def.LinearVelocity = new Vec2(p.LinearVelocityX, p.LinearVelocityY);
			Body = PhysConfig.world.CreateBody(def);
			PolygonDef pdef = new PolygonDef();
			pdef.SetAsBox(diameterX, diameterY);
			if (!p.Static)
			{
				pdef.Density = p.Density == 0 ? 0.01f : p.Density;
				pdef.Friction = p.Friction;
				pdef.Restitution = p.Restitution;
			}
			Body.CreateFixture(pdef);
			if (!p.Static)
			{
				Body.SetMassFromShapes();
			}
		}
	}
}