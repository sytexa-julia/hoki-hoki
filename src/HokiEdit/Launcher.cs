using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SpriteUtilities;
using FloatMath;

namespace HokiEdit {
	/// <summary>
	/// One part of a two-launcher pair in which one launches mines, and the other catches them.
	/// </summary>
	public class Launcher : SpriteObject,MapElement {
		private int
			turns;		//Number of 45° rotations
		private Segment
			path;		//Segment on which the projectile moves/this may be positioned
		private Launcher
			other;		//Launcher connected with this one
		private bool
			sender;		//Whether this launches (if false, it means the other launches and this receives)
		private bool
			selected,	//Whether currently selected
			deleted;	//Whether already deleted
		private SpriteArrow
			dirArrow;	//Shows direction of launch
		private float
			trueX,		//Real coordinates (as opposed to the displayed ones)
			trueY,
			frequency,	//Frequency of launches, decimal% (0=never, 1=as fast as possible without mine overlap)
			offset;		//Launch timing offset, decimal % of the time between launches

		public override float X {
			get { return trueX; }
			set {
				trueX=value;
				base.X=coord(trueX);
			}
		}

		public override float Y {
			get { return trueY; }
			set {
				trueY=value;
				base.Y=coord(trueY);
			}
		}

		public float DrawX {
			get { return coord(trueX); }
		}

		public float DrawY {
			get { return coord(trueY); }
		}

		public bool Sender {
			get { return sender; }
			set { sender=value; }
		}

		public int Turns {
			get { return turns; }
			set {
				turns=value%8;
				Rotation=value*FMath.PI/4;
				getPath();
				X=trueX;
				Y=trueY;
			}
		}

		public float Frequency {
			get { return frequency; }
			set { frequency=value; }
		}
		
		public float Offset {
			get { return offset; }
			set { offset=value; }
		}

		public Launcher Other {
			get { return other; }
			set {
				other=value;
				other.other=this;
			}
		}

		/// <param name="tex">Cannot be null, must have at least 2 frames</param>
		public Launcher(Device device,SpriteTexture tex) : base(device,tex) {
			//Orient the origin at the center
			origin.X=tex.Width/2;
			origin.Y=tex.Height/2;

			//Set default frequency and offset
			frequency=0.2f;
			offset=0.0f;

			//Determine the initial path
			getPath();
		}

		/// <summary>
		/// Updates the path based on rotation
		/// </summary>
		private void getPath() {
			path=new Segment(base.position, new Vector2(FMath.Cos(Rotation),FMath.Sin(Rotation)));
		}

		/// <summary>
		/// Get a drawn coordinate from a coordinate on the 8x8square grid
		/// </summary>
		private float coord(float screenCoord) {
			return screenCoord+(turns%2)*4;
		}

		/// <summary>
		/// Convert offset and frequency to integers for compiling
		/// </summary>
		private int percent(float decimalPercent) {
			return (int)(decimalPercent*100);
		}

		public void Rotate() {
			Turns++;
		}

		public void SetPos(float x,float y) {
			//Find a perpendicular segment (use it as an infinite line)
			float ang=path.Angle+FMath.PI/2;
			Segment perp=new Segment(new Vector2(x,y),new Vector2(x+FMath.Cos(ang),y+FMath.Sin(ang)),null);

			//Get the intersection with the path
			Vector2 intersection;
			Segment.Intersection(path,perp,out intersection);

			//Get the distance from the sender in square units
			float dist=FMath.Distance(intersection,other.position);
			if (turns%2==0)
				dist-=dist%8;
			else
				dist-=dist%(8*FMath.Sqrt(2));

			//Project position from the sender
			X=other.X+FMath.Cos(other.rotation)*dist;
			Y=other.Y+FMath.Sin(other.rotation)*dist;
		}

		#region MapElement Members

		public event System.EventHandler Delete;

		public bool TriggerDelete() {
			if (!deleted) {
				deleted=true;
				Delete(this,new EventArgs());
				if (!other.Selected()) other.TriggerDelete();
				return true;
			}
			return false;
		}

		public void Select() {
			if (!selected) {
				selected=true;
				other.Select();
				Frame=1;

				if (sender) {
					dirArrow=new SpriteArrow(device,Vector2.Empty,new Vector2(FMath.Distance(position,other.position),0));
					dirArrow.Thickness=2;
					dirArrow.Tint=System.Drawing.Color.FromArgb(50,82,118);
					Add(dirArrow);
				}
			}
		}

		public void Deselect() {
			if (selected) {
				selected=false;
				Frame=0;
				other.Deselect();
				if (sender)
					Remove(dirArrow);
			}
		}

		public bool Selected() {
			return selected;
		}

		public void Move(Vector2 v) {
			X+=v.X;
			Y+=v.Y;
		}

		public bool PtInShape(Vector2 pt) {
			GlobalToLocal(ref pt);
			return (pt.X>-tex.Width/2 && pt.X<tex.Width/2 && pt.Y>-tex.Width/2 && pt.Y<tex.Width/2);
		}

		public bool InBox(SpriteBox box) {
			//Get position in screen coordinates
			Vector2 pos=position;
			parent.LocalToGlobal(ref pos);
			return box.PtInBox(pos);
		}

		public string Compile() {
			return Compile(0,0);
		}

		public string Compile(float xOffset,float yOffset) {
			String outString="\n"+(int)(trueX+xOffset)/8+","+(int)(trueY+yOffset)/8+","+turns;
			if (sender) outString+=","+percent(frequency)+","+percent(offset);
			return outString;
		}

		#endregion
	}
}
