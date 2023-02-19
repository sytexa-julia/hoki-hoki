using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SpriteUtilities;
using FloatMath;

namespace HokiEdit {
	/// <summary>
	/// Causes the heli to reverse rotation direction
	/// </summary>
	public class Spring : SpriteObject,MapElement {
		private int
			turns=0;	//Number of times rotated
		private Vector2
			center;		//Center point (used for PtInBox)
		public bool
			deleted=false,
			selected=false;

		public int Turns {
			get { return turns%8; }
		}

		/// <param name="tex">tex must not be null, and must have at least 2 frames</param>
		public Spring(Device device,SpriteTexture tex) : base(device,tex) {
			//Move the origin to the bottom center of the spring
			origin.X=tex.Width/2;
			origin.Y=tex.Height;

			//Find the central point
			center=new Vector2(0,-tex.Height/2);
		}

		public void Rotate() {
			Rotation+=FMath.PI/2;
			turns++;
		}

		#region MapElement Members

		public event System.EventHandler Delete;

		public bool TriggerDelete() {
			if (!deleted) {
				Delete(this,new EventArgs());
				deleted=true;
				return true;
			}
			return false;
		}

		public void Select() {
			selected=true;
			Frame=1;
		}

		public void Deselect() {
			selected=false;
			Frame=0;
		}

		public bool Selected() {
			return selected;
		}

		public void Move(Vector2 v) {
			X+=v.X;
			Y+=v.Y;
		}

		public bool PtInShape(Vector2 pt) {
			//Get the pt in local coord space
			GlobalToLocal(ref pt);

			//Test whether it's in the textured region
			return (pt.Y>-tex.Height && pt.Y<0 && pt.X>-tex.Width/2 && pt.X<tex.Width/2);
		}

		public bool InBox(SpriteBox box) {
			//Copy the center point and convert it to the global coordinate space
			Vector2 centerCopy=center;
			LocalToGlobal(ref centerCopy);

			//Determine whether that point is in the box
			return box.PtInBox(centerCopy);
		}

		public string Compile() {
			return Compile(0,0);
		}

		public string Compile(float xOffset,float yOffset) {
			return "\n"+Math.Round(X-X%8+xOffset)/8+","+Math.Round(Y-Y%8+yOffset)/8+","+turns;
		}

		#endregion
	}
}
