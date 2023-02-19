using System;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SpriteUtilities;
using SpriteMenu;

namespace HokiEdit {
	/// <summary>
	/// Summary description for SpriteMarker.
	/// </summary>
	public class SpriteMarker : SpriteButton, MapElement {
		private int index;
		private float depth;

		private bool
			selected=false,
			deleted=false;	//true after the Delete event has been fired

		private SpriteText
			indexText,
			depthText;

		private Vector2 center;

		public event System.EventHandler Delete;

		public SpriteMarker(Device device,SpriteTexture tex,Sprite sprite,Font font,Form form) : base(device,null,null,new ButtonTexture(null,tex,null),form) {
			int halfHeight=(int)tex.Height/2;

			//Create texts
			indexText=new SpriteText(device,sprite,font,tex.Width,halfHeight);
			indexText.Format=DrawTextFormat.Center|DrawTextFormat.VerticalCenter;
			Add(indexText);

			depthText=new SpriteText(device,sprite,font,tex.Width,halfHeight);
			depthText.Y=tex.Height/2;
			depthText.Format=DrawTextFormat.Center|DrawTextFormat.VerticalCenter;
			Add(depthText);

			//Set default depth and index
			Index=0;
			Depth=1f;

			//Find the vector from the topleft corner to the center
			center=new Vector2(tex.Width/2,tex.Height/2);
		}

		/// <summary>
		/// Update the button graphic to represent the current state
		/// </summary>
		protected override void updateButtonFrame() {
			if (selected)	setButtonFrame(2);
			else			base.updateButtonFrame();
		}

		public float Depth {
			get { return depth; }
			set {
				depth=(float)Math.Round(Math.Max(0,Math.Min(1,value))*20f,0)/20f;

				string depthString=depth.ToString();
				depthText.Text=depthString.Length==1?depthString:depthString.Substring(1);
			}
		}

		public int Index {
			get { return index; }
			set {
				index=Math.Max(0,value);
				indexText.Text=""+index;
			}
		}

		#region MapElement Members

		public bool TriggerDelete() {
			if (!deleted) {
				deleted=true;
				Delete(this,new EventArgs());
				return true;
			}
			return false;
		}

		public void Select() {
			selected=true;
			updateButtonFrame();
		}

		public void Deselect() {
			selected=false;
			updateButtonFrame();
		}

		public bool Selected() {
			return selected;
		}

		public void Move(Vector2 v) {
			position+=v;
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
			return "\n"+Math.Round((X+center.X+xOffset)/4)+","+Math.Round((Y+center.Y+yOffset)/4)+","+index+","+depth;
		}

		#endregion
	}
}