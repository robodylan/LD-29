using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD_29.Level
{
	public class JSONDescription
	{
		/// <summary>
		/// Human Readable Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Scale value level will be scaled. 0.5-2
		/// </summary>
		public float Scale { get; set; }

		/// <summary>
		/// Relative Path to Collission PNG
		/// </summary>
		public string CollisionMap { get; set; }

		/// <summary>
		/// Relative Path to Rendering PNG above the level
		/// </summary>
		public string TopLayerMap { get; set; }

		/// <summary>
		/// Relative Path to Rendering PNG below the level
		/// </summary>
		public string BottomLayerMap { get; set; }

		/// <summary>
		/// Entites
		/// </summary>
		public List<JSONEntity> Entities { get; set; }

		/// <summary>
		/// Start Position
		/// </summary>
		public JSONVector<int> Start { get; set; }

		/// <summary>
		/// End Position
		/// </summary>
		public JSONVector<int> Finish { get; set; }

		/// <summary>
		/// Has the level a scret exit?
		/// </summary>
		public bool HasSecretExit { get; set; }

		/// <summary>
		/// Position of second exit if level has a secret exit
		/// </summary>
		public JSONVector<int> SecretExit { get; set; }

		/// <summary>
		/// Next level when done
		/// </summary>
		public int NextLevel { get; set; }

		/// <summary>
		/// Next level when done through secret exit
		/// </summary>
		public int SecretNextLevel { get; set; }
	}
}