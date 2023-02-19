using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SpriteUtilities;
using SpriteMenu;
using FloatMath;

namespace HokiEdit {
	/// <summary>
	/// A triangle connected to three nodes
	/// </summary>
	public class Triangle : SpriteObject,MapElement {
		private bool
			selected=false,
			deleted=false;	//true after the Delete event has been fired
		private Node[] nodes;
		private SpriteTriangle tri;

		public Triangle(Device device,Texture tex,Node a,Node b,Node c) : base(device,null) {
			//Store nodes
			nodes=new Node[3] {a,b,c};
			foreach (Node n in nodes) n.AddIncidence(this);	//Give the nodes a reference to this

			//Make triangle
			tri=new SpriteTriangle(device,tex);
			updatePosition();
			Add(tri);

			//Hook delete event
			Delete+=new EventHandler(OnDelete);
		}

		public override void Draw(Matrix parentMatrix, Vector2 parentShift) {
			updatePosition();
			base.Draw (parentMatrix, parentShift);
		}

		protected void updatePosition() {
			for (int i=0;i<nodes.Length;i++)
				tri.SetPosition(i,new Vector2(nodes[i].X+nodes[i].Width/2,nodes[i].Y+nodes[i].Height/2));
		}

		#region MapElement Members

		public event System.EventHandler Delete;

		public bool TriggerDelete() {
			//Don't re-delete
			if (deleted) return false;

			deleted=true;
			selected=false;
			Delete(this,new EventArgs());
			return true;
		}

		public void Select() {
			selected=true;
			Alpha=160;
		}

		public void Deselect() {
			selected=false;
			Alpha=100;
		}

		public bool Selected() {
			return selected;
		}

		public void Move(Microsoft.DirectX.Vector2 v) {
			foreach (Node n in nodes) if (!n.Selected()) n.Move(v);
		}

		public bool PtInShape(Microsoft.DirectX.Vector2 pt) {
			/* This algorithm goes through each of the triangle's three lines
			 * (represented by a pair of points) and sees if the third point and
			 * the point we're testing are both on the same side of it. If that
			 * condition is true for each of the lines, then pt is inside the
			 * triangle (if you want to prove it to yourself, just draw a triangle
			 * with a dot inside and it'll make sense).
			 */

			GlobalToLocal(ref pt);

			//Get all verts
			Vector2[] verts=new Vector2[3];
			for (int i=0;i<3;i++) verts[i]=tri.GetPosition(i);

			//Test each side
			for (int i=0;i<3;i++) if (!sameSide(pt,verts[i%3],verts[(i+1)%3],verts[(i+2)%3])) return false;

			//If the flow gets here, each side's test passed
			return true;
		}

		public bool InBox(SpriteBox box) {
			foreach (Node n in nodes) if (!n.InBox(box)) return false;	//If any of the nodes is outside the box, return false
			return true;
		}

		/// <summary>
		/// Determines if p1 and p2 are both on the same side of the line defined by a and b.
		/// </summary>
		private bool sameSide(Vector2 p1,Vector2 p2,Vector2 a,Vector2 b) {
			float slope;

			if (a.X==b.X) slope=1000;		//Would be infinity; this is basically close enough.
			else slope=(b.Y-a.Y)/(b.X-a.X);	//Can calculate actual slope

			float intercept=a.Y-slope*a.X;

			return ((p1.Y>slope*p1.X+intercept)==(p2.Y>slope*p2.X+intercept));
		}

		public string Compile() {
			return "\n"+nodes[0].ID+","+nodes[1].ID+","+nodes[2].ID;
		}

		#endregion

		private void OnDelete(object sender, EventArgs e) {
			//Clear self from incidence lists
			foreach (Node n in nodes) n.RemoveIncidence(this);
		}
	}
}
