using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LD_29
{
	public struct PhysicsParams
	{
		[DefaultValue(0)]
		public float X { get; set; }

		[DefaultValue(0)]
		public float Y { get; set; }

		[DefaultValue(0)]
		public float Angle { get; set; }

		[DefaultValue(0)]
		public float AngularDamping { get; set; }

		[DefaultValue(0)]
		public float AngularVelocity { get; set; }

		[DefaultValue(false)]
		public bool FixedRotation { get; set; }

		[DefaultValue(true)]
		public bool CanSleep { get; set; }

		[DefaultValue(false)]
		public bool IsSleeping { get; set; }

		[DefaultValue(0)]
		public float LinearDamping { get; set; }

		[DefaultValue(0)]
		public float LinearVelocityX { get; set; }

		[DefaultValue(0)]
		public float LinearVelocityY { get; set; }

		[DefaultValue(true)]
		public bool Static { get; set; }

		[DefaultValue(0)]
		public float Density { get; set; }

		[DefaultValue(0)]
		public float Friction { get; set; }

		[DefaultValue(0)]
		public float Restitution { get; set; }

		[DefaultValue(false)]
		public bool Ghost { get; set; }
	}
}