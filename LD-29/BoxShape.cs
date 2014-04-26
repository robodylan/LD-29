﻿using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common.PhysicsLogic;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
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
			Body = BodyFactory.CreateRectangle(PhysConfig.world, diameterX, diameterY, p.Density);
			Body.Position = new Vector2(p.X, p.Y);
			Body.IsBullet = !p.CanSleep;
			Body.Awake = !p.IsSleeping;
			Body.Rotation = p.Angle;
			Body.AngularDamping = p.AngularDamping;
			Body.AngularVelocity = p.AngularVelocity;
			Body.FixedRotation = p.FixedRotation;
			Body.LinearDamping = p.LinearDamping;
			Body.LinearVelocity = new Vector2(p.LinearVelocityX, p.LinearVelocityY);
			Body.Friction = p.Friction;
			Body.Restitution = p.Restitution;
			Body.IsStatic = p.Static;
		}
	}
}