using System;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SpriteUtilities;

namespace SpriteMenu {
	/// <summary>
	/// A button widget derived from SpriteObject
	/// </summary>
	public class SpriteButton : SpriteObject,Lockable {
		#region vars
		protected bool
			locked,	//Whether currently locked
			hover,	//Whether the mouse is hovering over the button
			down;	//Whether the button is currently depressed
		protected Form
			frm;	//Windows Form this button is in
		protected SpriteText
			text;	//SpriteObject to draw text with
		protected SpriteObject		//Various parts of the button
			left,	//Left end
			middle,	//Middle (stretched)
			right;	//Right end
		#endregion

		#region events
		public event MouseEventHandler Press;
		public event MouseEventHandler Release;
		public event MouseEventHandler ReleaseOutside;
		public event MouseEventHandler RollOver;
		public event MouseEventHandler RollOut;
		#endregion

		#region getset
		public string Text {
			get { if (text!=null) return text.Text; else return ""; }
			set { if (text!=null) text.Text=value; }
		}

		public Font LabelFont {
			get { if (text!=null) return text.DrawFont; else return null; }
			set { if (text!=null) text.DrawFont=value; }
		}

		public override float Width {
			get { return left.Width+middle.Width+right.Width; }
			set {
				middle.Width=Math.Max(0,value-right.Width-left.Width);	//Set the middle width, must be >=0
				right.X=left.Width+middle.Width;						//Set the right end's position
			}
		}

		public override float Height {
			get { return Math.Max(Math.Max(left.Height,middle.Height),right.Height); }
			set { base.Height = value; }
		}

		#endregion

		#region constructor
		public SpriteButton(Device device,Sprite sprite,Font font,ButtonTexture tex,Form frm) : base(device,null) {
			//Store the form reference
			this.frm=frm;

			if (tex!=null) {
				//Create button segments using a texture if provided
				left=new SpriteObject(device,tex.Left);
				middle=new SpriteObject(device,tex.Middle);
				right=new SpriteObject(device,tex.Right);
			} else {
				//If no texture is provided, create dummy objects to prevent exceptions
				left=new SpriteObject(device,null);
				middle=new SpriteObject(device,null);
				right=new SpriteObject(device,null);
			}

			//Add segments to the draw list
			Add(left);
			Add(middle);
			Add(right);
			
			//If a sprite is provided, create text, add it, and format it
			if (sprite!=null && font!=null) {
				text=new SpriteText(device,sprite,font,(int)middle.Width,(int)middle.Height);
				text.Format=DrawTextFormat.VerticalCenter|DrawTextFormat.NoClip;
				text.X=left.Width;	//Text never moves, so its position can be set here
				text.Y=2;			//Always seems a little bit too high
				Add(text); //Add it to the draw list
			}

			//Set unchanging positions
			middle.X=left.Width;

			//Capture from mouse events
			frm.MouseMove+=new MouseEventHandler(onMouseMove);
			frm.MouseDown+=new MouseEventHandler(onMouseDown);
			frm.MouseUp+=new MouseEventHandler(onMouseUp);

			//Capture own events (these handlers set the frame)
			Press+=new MouseEventHandler(onPress);
			Release+=new MouseEventHandler(onRelease);
			ReleaseOutside+=new MouseEventHandler(onReleaseOutside);
			RollOver+=new MouseEventHandler(onRollOver);
			RollOut+=new MouseEventHandler(onRollOut);

			//Set default state
			hover=down=false;
		}
		#endregion

		#region collision detection
		/// <summary>
		/// Tests whether a mouse event occurred while the mouse was over this button
		/// </summary>
		protected bool mouseInBounds(MouseEventArgs e) {
			Vector2 m=new Vector2(e.X,e.Y);	//Put the mouse coords in a vector
			GlobalToLocal(ref m);			//Transform them to local space
			return ptInBounds(m);
		}

		/// <summary>
		/// Tests whether a point in local coordinate space is over the button
		/// </summary>
		/// <param name="pt">A point in local coordinate space</param>
		protected bool ptInBounds(Vector2 pt) {
			return (pt.X>0 && pt.X<Width && pt.Y>0 && pt.Y<Height);
		}
		#endregion

		#region button frame
		/// <summary>
		/// Helper method for the event handlers. Sets the frame of the three button graphics.
		/// </summary>
		virtual protected void setButtonFrame(int frame) {
			left.Frame=frame;
			middle.Frame=frame;
			right.Frame=frame;
		}

		/// <summary>
		/// Sets the proper frame based on state
		/// </summary>
		virtual protected void updateButtonFrame() {
			if (hover) {
				if (down)	setButtonFrame(2);
				else		setButtonFrame(1);
			} else			setButtonFrame(0);
		}
		#endregion

		#region mouse event handlers
		virtual protected void onMouseMove(object sender,MouseEventArgs e) {
			if (locked) return;	//Ignore user input while locked

			//If the mouse is over the button...
			if (mouseInBounds(e)) {
				if (hover==false) {
					hover=true;
					RollOver(this,e);
				}
			} else if (hover==true) {
				hover=false;
				RollOut(this,e);
			}
		}

		virtual protected void onMouseDown(object sender,MouseEventArgs e) {
			if (locked) return;	//Ignore user input while locked
			if (hover) {
				Press(this,e);
			}
		}

		virtual protected void onMouseUp(object sender, MouseEventArgs e) {
			if (locked) return;	//Ignore user input while locked
			if (down) {
				if (hover) Release(this,e);
				else ReleaseOutside(this,e);
			}
		}
		#endregion

		#region button event handlers
		virtual protected void onPress(object sender, MouseEventArgs e) {
			down=true;
			updateButtonFrame();
		}

		virtual protected void onRelease(object sender, MouseEventArgs e) {
			down=false;
			updateButtonFrame();
		}

		virtual protected void onReleaseOutside(object sender, MouseEventArgs e) {
			down=false;
			updateButtonFrame();
		}

		virtual protected void onRollOver(object sender, MouseEventArgs e) {
			updateButtonFrame();
		}

		virtual protected void onRollOut(object sender, MouseEventArgs e) {
			updateButtonFrame();
		}
		#endregion

		#region Lockable Members
		virtual public void Lock() {
			locked=true;
		}
		virtual public void Unlock() {
			locked=false;
		}
		public bool Locked() {
			return locked;
		}
		#endregion
	}
}