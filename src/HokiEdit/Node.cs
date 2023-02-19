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

namespace HokiEdit {
	/// <summary>
	/// A junction between walls.
	/// </summary>
	public class Node : SpriteButton,MapElement {
		private bool
			selected=false,
			deleted=false;	//true after the Delete event has been fired
		private ArrayList incidences;
		private int id;
		private Vector2 nextMove;	//Move once updated

		public event EventHandler Delete;

		public int ID {
			get { return id; }
			set { id=value; }
		}

		/// <param name="tex">A four-frame texture: default, mouseover, pressed down, and selected</param>
		/// <param name="frm">A Windows form (used to capture mouse events)</param>
		public Node(Device device,SpriteTexture tex,Form form) : base(device,null,null,new ButtonTexture(null,tex,null),form) {
			//Default state
			selected=false;

			//init line and triangle lists
			incidences=new ArrayList();

			//Capture delete event
			Delete+=new EventHandler(OnDelete);
		}

		/// <summary>
		/// Update the button graphic to represent the current state
		/// </summary>
		protected override void updateButtonFrame() {
			if (selected)	setButtonFrame(3);
			else			base.updateButtonFrame();
		}

		/// <summary>
		/// Applies whatever move the node has stored. Avoids moving the same node multiple times for one event
		/// </summary>
		public void Update() {
			X+=nextMove.X;
			Y+=nextMove.Y;
			nextMove=Vector2.Empty;
		}

		/// <summary>
		/// Clean up before deletion
		/// </summary>
		/// <param name="sender"></param>
		private void OnDelete(object sender, EventArgs e) {
			while (incidences.Count!=0) ((MapElement)incidences[0]).TriggerDelete();
		}
		
		#region incidence lists
		/// <summary>
		/// Add an incident object. It will be deleted when this node is deleted
		/// </summary>
		internal void AddIncidence(MapElement obj) {
			incidences.Add(obj);
		}

		/// <summary>
		/// Remove an incident object
		/// </summary>
		internal void RemoveIncidence(MapElement obj) {
			incidences.Remove(obj);
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
			selected=true;

			//Update the graphic
			updateButtonFrame();
		}

		public void Deselect() {
			selected=false;

			//Update the graphic
			updateButtonFrame();
		}

		public bool Selected() {
			return selected;
		}

		public void Move(Microsoft.DirectX.Vector2 v) {
			nextMove=v;
		}

		public bool PtInShape(Microsoft.DirectX.Vector2 pt) {
			GlobalToLocal(ref pt);
			return ptInBounds(pt);
		}

		public bool InBox(SpriteBox box) {
			return box.PtInBox(new Vector2(position.X+parent.Parent.X,position.Y+parent.Parent.Y));
		}

		public string Compile() {
			return Compile(0,0);
		}

		/// <summary>
		/// Compiles the node with an adjusted position
		/// </summary>
		/// <param name="xOffset">Amount to add to X</param>
		/// <param name="yOffset">Amount to add to Y</param>
		public string Compile(int xOffset,int yOffset) {
			return "\n"+Math.Round((X+xOffset)/8)+","+Math.Round((Y+yOffset)/8);
		}

		#endregion
	}
}
