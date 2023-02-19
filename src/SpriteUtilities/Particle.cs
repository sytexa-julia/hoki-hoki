using System;
using Microsoft.DirectX;

namespace SpriteUtilities {
	/// <summary>
	/// Particle with position, velocity, acceleration data for motion and alpha/fade rate data for lifetime control
	/// </summary>
	public struct Particle : Updateable {
		public Vector2
			Position,
			Velocity,
			Acceleration;
		public Vector3
			Color,
			ColorChange;
		public float
			Alpha,
			Fade;

		public Particle(Vector2 position,Vector2 velocity,Vector2 acceleration,Vector3 color,Vector3 colorChange,float fade) {
			Alpha=byte.MaxValue;
			Position=position;
			Velocity=velocity;
			Acceleration=acceleration;
			Color=color;
			ColorChange=colorChange;
			Fade=fade;
		}

		#region Updateable Members
		public void Update(float elapsedTime) {
			Position+=Vector2.Scale(Velocity,elapsedTime);
			Velocity+=Vector2.Scale(Acceleration,elapsedTime);
			Color+=Vector3.Scale(ColorChange,elapsedTime);
			Alpha-=Fade*elapsedTime;
			System.Diagnostics.Trace.WriteLine("P "+Position.X+","+Position.Y);
		}
		#endregion
	}
}