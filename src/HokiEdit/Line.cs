using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SpriteUtilities;
using FloatMath;

namespace HokiEdit {
	/// <summary>
	/// Summary description for Line.
	/// </summary>
	public class Line : SpriteLine,MapElement {
		private bool
			selected=false,
			deleted=false;	//true after the Delete event has been fired
		private Node
			from,				//Nodes the line is connected to
			to;
		private readonly Color
			selectColor,		//Color when selected
			deselectColor;		//Color when not selected
		private const float
			selectThickness=2,	//Thickness when selected
			deselectThickness=1,//Thickness when not selected
			selectArea=6;		//Distance from the line that selection will work at

		#region getset
		public Node From {
			get { return from; }
		}
		public Node To {
			get { return to; }
		}
		#endregion

		public event EventHandler Delete;

		public Line(Device device,Node from,Node to) : base(device) {
			//Copy end-nodes
			this.from=from;
			this.to=to;

			//Add reference to nodes' list to trigger deletion on node deletion
			from.AddIncidence(this);
			to.AddIncidence(this);

			//Capture Delete event
			Delete+=new EventHandler(OnDelete);

			//Define color(s)
			deselectColor=Color.Black;
			selectColor=Color.FromArgb(50,82,118);
		}

		/// <summary>
		/// Updates position and size based on node positions
		/// </summary>
		protected void updatePosition() {
			X=from.X+from.Width/2;
			Y=from.Y+from.Height/2;
			Width=to.X+to.Width/2-X;
			Height=to.Y+to.Height/2-Y;
		}

		#region draw overrides
		public override void Draw() {
			updatePosition();
			base.Draw ();
		}

		private void OnDelete(object sender, EventArgs e) {
			//Remove reference from attached nodes
			from.RemoveIncidence(this);
			to.RemoveIncidence(this);
		}

		public override void Draw(Matrix parentMatrix, Vector2 parentShift) {
			updatePosition();
			base.Draw (parentMatrix, parentShift);
		}
		#endregion

		#region MapElement Members

		public bool TriggerDelete() {
			if (deleted) return false;

			deleted=true;
			selected=false;
			Delete(this,new EventArgs());
			return true;
		}

		public void Select() {
			//Mark that this is now selected
			selected=true;
			Tint=selectColor;			//Change graphic
			Thickness=selectThickness;
		}

		public void Deselect() {
			selected=false;
			Tint=deselectColor;
			Thickness=deselectThickness;
		}

		public bool Selected() {
			return selected;
		}

		public void Move(Microsoft.DirectX.Vector2 v) {
			if (!from.Selected()) from.Move(v);
			if (!to.Selected()) to.Move(v);
		}

		public bool PtInShape(Microsoft.DirectX.Vector2 pt) {
			GlobalToLocal(ref pt);

			/* This algorithm works by finding the distance
			 * between pt and the closest point on the line.
			 * We find the straightest path from pt to the
			 * line by calculating the perpendicular line
			 * that crosses pt, and then find the point of
			 * intersection between those two. The distance
			 * between pt and the calculated point of
			 * intersection is the distance between the lines.
			 */

			//Calculate this line's slope
			bool inf=false;				//Whether the slope is infinite
			float m=Vector.Y/Vector.X;	//Get slope
			if (float.IsInfinity(m)) {
				inf=true;
				m=1000;	//Close enough to infinity for our purposes
			}

			//Caclulate perpendicular line that crosses through pt
			float mp=-1/m;						//Negative reciprocal slope
			if (float.IsInfinity(mp)) mp=1000;	//Close enough to infinity for our purposes
			float mb=pt.Y-mp*pt.X;				//y=mx+b, b=y-mx

			//Find intersection
			Vector2 c;		//Point of intersection
			c.X=mb/(m-mp);	//m1x+b1=m2x+b2, b1=0. m1x-m2x=b2-0, x(m1-m2)=b2, x=b2/(m1-m2)
			c.Y=m*c.X;		//y=mx (b=0), we know y is the same at this x on either line so just use the simpler equation

			//Check whether the intersection is on the segment (otherwise a click anywhere on the infinite line could be counted)
			if ((inf && ((Vector.Y>0 && (c.Y>Vector.Y || c.Y<0)) || (Vector.Y<0 && (c.Y<Vector.Y || c.Y>0)))) || (Vector.X>0 && (c.X>Vector.X || c.X<0)) || (Vector.X<0 && (c.X<Vector.X || c.X>0))) return false;

			//Find distance between intersection and pt
			return (FMath.Sqrt(FMath.Pow(pt.X-c.X,2)+FMath.Pow(pt.Y-c.Y,2))<selectArea);	//Diagonal distance formula
		}

		public bool InBox(SpriteBox box) {
			return (from.InBox(box) && to.InBox(box));
		}

		public string Compile() {
			// TODO:  Add Line.Compile implementation
			return "\n"+from.ID+","+to.ID;
		}

		#endregion
	}
}
