using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using D3D=Microsoft.DirectX.Direct3D;
using FloatMath;

namespace SpriteUtilities {
	/// <summary>
	/// A line segment transformed and drawn as a SpriteObject
	/// </summary>
	public class SpriteLine : TransformedObject {
		//D3D vars
		private D3D.Line line;	//D3DX line for drawing

		//Properties
		protected Vector2
			vector;		//Vector that represents the line
		protected float
			angle,		//Angle from 0rad
			magnitude;	//Length

		#region getset

		/// <summary>
		/// Vector representing the line
		/// </summary>
		public Vector2 Vector {
			get { return vector; }
			set {
				vector=value;
				updateAngle();
			}
		}

		/// <summary>
		/// X-component of the line vector
		/// </summary>
		public float Width {
			get { return vector.X; }
			set {
				vector.X=value;
				updateAngle();
			}
		}

		/// <summary>
		/// Y-component of the line vector
		/// </summary>
		public float Height {
			get { return vector.Y; }
			set {
				vector.Y=value;
				updateAngle();
			}
		}

		public float Angle {
			get { return angle; }
			set {
				angle=value;
				updateVector();
			}
		}

		public float Length {
			get { return magnitude; }
			set {
				magnitude=value;
				updateVector();
			}
		}

		/// <summary>
		/// Line midpoint
		/// </summary>
		public Vector2 Midpoint {
			get { return new Vector2(vector.X/2,vector.Y/2); }
		}

		/// <summary>
		/// Line's thickness (in pixels)
		/// </summary>
		virtual public float Thickness {
			get { return line.Width; }
			set { line.Width=value; }
		}

		/// <summary>
		/// Whether the line is antialiased
		/// </summary>
		virtual public bool Antialias {
			get { return line.Antialias; }
			set { line.Antialias=value; }
		}

		#endregion

		public SpriteLine(Device device) : base(device) {
			line=new D3D.Line(device);
			vector=new Vector2();

			Tint=System.Drawing.Color.Black;	//Black by default
		}

		/// <summary>
		/// Calculate angle and magnitude from vector
		/// </summary>
		protected void updateAngle() {
			angle=FMath.Atan2(vector.Y,vector.X);
			magnitude=FMath.Sqrt(FMath.Pow(Vector.X,2)+FMath.Pow(Vector.Y,2));
		}

		/// <summary>
		/// Calculate vector from angle and magnitude
		/// </summary>
		protected void updateVector() {
			vector.X=FMath.Cos(angle)*magnitude;
			vector.Y=FMath.Sin(angle)*magnitude;
		}

		/// <summary>
		/// Calculate the line in screen coordinates and draw it
		/// </summary>
		/// <param name="trans">Absolute transformation matrix</param>
		protected override void deviceDraw(Matrix trans) {
			//Create new Vector2s and transform them
			Vector2 P1=Vector2.Empty,P2=new Vector2(vector.X,vector.Y);
			P1.TransformCoordinate(trans);
			P2.TransformCoordinate(trans);

			//Draw the line
			line.Draw(new Vector2[]{P1,P2},Tint);
		}
	}
}
