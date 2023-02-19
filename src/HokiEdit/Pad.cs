using System;
using System.Collections;
using System.Drawing;
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
	/// A special square area on the map
	/// </summary>
	public class Pad : SpriteObject, MapElement {
		private PadType
			type;			//The pad's function
		private const int 
			size=191,		//Length of one side
			lineSpace=16;	//Pixels between diagonal lines
		private ArrayList
			objects;		//Hold box and lines so highlight color can be applied on selection
		private bool
			selected,		//Whether the object is currently selected
			deleted;
		private SpriteBox
			box;
		private SpriteText
			text;			//Text block in the middle of the pad

		#region getset
		public PadType Type {
			get { return type; }
			set {
				type=value;
				updateText();
			}
		}

		public override float Width {
			get { return size; }
		}
		
		public override float Height {
			get { return size; }
		}

		#endregion

		public Pad(Device device,Sprite sprite,Microsoft.DirectX.Direct3D.Font font,PadType type) : base(device,null) {
			//Store pad type
			this.type=type;

			//Create SpriteObject ArrayList
			objects=new ArrayList();

			//White background
			SpriteLine bg=new SpriteLine(device);
			bg.Width=bg.Thickness=size;
			bg.Tint=Color.White;
			bg.Y=size/2.0f;
			Add(bg);

			//Outer box
			box=new SpriteBox(device);
			box.Width=box.Height=size;
			Add(box);
			objects.Add(box);

			SpriteLine line;
			int doubleSize=2*size;
			int dist;	//Temporarily store the distance of a line from the topright or bottomleft corner, whatever is nearer
			for (int i=lineSpace;i<=doubleSize-doubleSize%lineSpace;i+=lineSpace) {
				line=new SpriteLine(device);
				if (i<size) {
					line.Y=i;					//Rest on the left edge
					dist=i;
				} else {
					line.Y=size-1;				//Rest on the bottom edge
					line.X=i-size;
					dist=Math.Abs(i-2*size);
				}
				line.Rotation=-FMath.PI/4;		//Rotate 45deg above +x
				line.Length=FMath.Sqrt(2)*dist;	//The line is the hypotenuse of a 45-45-90 triangle with short leg lengths i

				//Add to draw and storage lists
				Add(line);
				objects.Add(line);
			}

			//Add a white rectangle for text (sort of a hack using a line, but ok)
			SpriteLine white=new SpriteLine(device);
			white.Width=0.8f*size;
			white.Thickness=20;
			white.X=(size-white.Width)/2;
			white.Y=size/2.0f;
			white.Tint=Color.White;
			Add(white);

			//Add text
			text=new SpriteText(device,sprite,font,(int)white.Width,(int)white.Thickness);
			text.Format=DrawTextFormat.VerticalCenter|DrawTextFormat.Center;
			text.X=(int)white.X;
			text.Y=(int)(white.Y-white.Thickness/2);
			Add(text);

			//Set the text
			updateText();
		}

		private void updateText() {
			switch (type) {
				case PadType.End:
					text.Text="End pad";
					break;
				case PadType.Start:
					text.Text="Start pad";
					break;
				case PadType.Heal:
					text.Text="Heal pad";
					break;
			}
		}

		#region MapElement Members
		public event System.EventHandler Delete;

		public bool TriggerDelete() {
			if (deleted) return false;

			deleted=true;
			selected=false;
			Delete(this,new EventArgs());
			return true;
		}

		public void Select() {
			selected=true;
			foreach (TransformedObject o in objects) o.Tint=Color.FromArgb(50,82,118);
			box.Thickness=2;
		}

		public void Deselect() {
			selected=false;
			foreach (TransformedObject o in objects) o.Tint=Color.Black;
			box.Thickness=1;
		}

		public bool Selected() {
			return selected;
		}

		public void Move(Microsoft.DirectX.Vector2 v) {
			X+=v.X;
			Y+=v.Y;
		}

		public bool PtInShape(Microsoft.DirectX.Vector2 pt) {
			GlobalToLocal(ref pt);
			return (pt.X<size && pt.X>0 && pt.Y<size && pt.Y>0);
		}

		public bool InBox(SpriteBox box) {
			return box.PtInBox(new Vector2(position.X+parent.Parent.X+Width/2,position.Y+parent.Parent.Y+Height/2));
		}

		public string Compile() {
			return Compile(0,0);
		}

		/// <summary>
		/// Compiles the pad with an adjusted position
		/// </summary>
		/// <param name="xOffset">Amount to add to X</param>
		/// <param name="yOffset">Amount to add to Y</param>
		public string Compile(int xOffset,int yOffset) {
			return "\n"+Math.Round((X+xOffset)/8)+","+Math.Round((Y+yOffset)/8)+","+(int)type;
		}

		#endregion
	}

	public enum PadType {
		Start,
		End,
		Heal
	}
}
