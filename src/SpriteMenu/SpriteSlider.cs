using System;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SpriteUtilities;
using FloatMath;

namespace SpriteMenu {
	/// <summary>
	/// Summary description for SpriteSlider.
	/// </summary>
	public class SpriteSlider : SpriteObject,Lockable {
		protected SpriteObject
			left,		//Left end of the slider
			right,		//Right end of the slider
			groove;		//Groove that the knob is moved along
		internal SpriteKnob
			knob;		//Knob that the user controls the slider with
		protected float
			slideArea,	//The size of the area that the knob can be in
			slideRatio;	//Position of the knob in the grove as a decimal percentage
		protected bool
			locked,		//Whether currently locked
			knobDown;	//Whether the knob is pressed down
		protected int
			knobOffset;	//Distance from the mouse to the knob on the knob's press

		public event EventHandler Change;

		#region getset

		public override float Width {
			get { return left.Width+groove.Width+right.Width; }
			set {
				groove.Width=Math.Max(value-left.Width-right.Width,0);	//Set the width of the groove
				right.X=left.Width+groove.Width;		//Position the right end
				slideArea=groove.Width-knob.Width;		//Calculate the area the knob can be in
				knob.X=left.Width+slideRatio*slideArea;	//Reposition the knob based on the new area using the old position ratio
			}
		}

		public override float Height {
			get { return Math.Max(left.Height,knob.Height)*YScale; }
			set { base.Height=value; }
		}

		public float Value {
			get { return slideRatio; }
			set {
				value=FMath.Clamp(value,0,1);	//Clamp the value in range
				slideRatio=value;
				knob.X=value*slideArea+left.Width;
				updateEnds();
				Change(this,new EventArgs());
			}
		}

		#endregion

		public SpriteSlider(Device device,Form frm,SliderTexture tex) : base(device,null) {
			//Create objects
			left=new SpriteObject(device,tex.Left);
			right=new SpriteObject(device,tex.Right);
			groove=new SpriteObject(device,tex.Groove);
			knob=new SpriteKnob(device,frm,tex.Knob);

			//Add to drawlist
			Add(left);
			Add(right);
			Add(groove);
			Add(knob);

			//Capture knob and form events
			knob.Press+=new MouseEventHandler(onKnobPress);
			knob.Release+=new MouseEventHandler(onKnobRelease);
			knob.ReleaseOutside+=new MouseEventHandler(onKnobRelease);
			frm.MouseDown+=new MouseEventHandler(onMouseDown);
			frm.MouseMove+=new MouseEventHandler(onMouseMove);

			//Capture own events to avoid null reference exceptions (C# is dumb and requires that all events have at least one handler
			Change+=new EventHandler(OnChange);

			//Set unchanging positions
			groove.X=left.Width;	//Never moves
			groove.Y=-groove.Height/2;
			left.Y=-left.Height/2;
			right.Y=-right.Height/2;
			knob.Y=-knob.Height/2;
			
			//Set default state
			slideRatio=0;	//Place knob at the leftmost possible position
			left.Frame=1;	//When slideRatio is 0, left should be lit up
			Width=0;		//Doesn't actually set to 0, but rather to the width of left+right

			//Set default state
			knobDown=false;
		}

		private void onKnobPress(object sender, MouseEventArgs e) {
			if (locked) return;

			knobDown=true;
			
			//Find out how far the mouse is from the knob
			Vector2 m=localMouse(e);
			knobOffset=(int)(knob.X-m.X);
		}

		private void onKnobRelease(object sender, MouseEventArgs e) {
			if (locked) return;

			knobDown=false;
		}

		private void onMouseDown(object sender, MouseEventArgs e) {
			if (locked) return;

			Vector2 m=localMouse(e);		//Get local mouse position
			float height=nativeHeight()/2;	//Get distance from y=0 to outer bounds

			//If the mouse clicks on the slider, but not on or too close to the knob...
			if (m.X>left.Width && m.X<left.Width+slideArea && m.Y>-height && m.Y<height && Math.Abs(knob.X-m.X)>knob.Width)
				moveKnob(m.X-knob.Width/2);	//move the knob.
		}

		private void onMouseMove(object sender, MouseEventArgs e) {
			if (locked) return;

			if (knobDown) {
				//Get local mouse coordinates
				Vector2 m=localMouse(e);

				//Set the knob's X-position (clamp within ok range)
				moveKnob(FMath.Clamp(m.X+knobOffset,left.Width,slideArea+left.Width));
			}
		}

		private void moveKnob(float pos) {
			if (locked) return;

			knob.X=pos;									//Position the knob
			slideRatio=(knob.X-left.Width)/slideArea;	//Determine the slider's new value
			Change(this,new EventArgs());				//Fire the change event
			
			//Light up the ends based on knob position
			updateEnds();
		}

		private void updateEnds() {	
			if (slideRatio==0) left.Frame=1; else left.Frame=0;
			if (slideRatio==1) right.Frame=1; else right.Frame=0;
		}

		/// <summary>
		/// Returns mouse position in local coordinate space
		/// </summary>
		protected Vector2 localMouse(MouseEventArgs e) {
			Vector2 m=new Vector2(e.X,e.Y);	//Put the mouse coords in a vector
			GlobalToLocal(ref m);			//Transform them to local coordinate space
			return m;
		}

		/// <summary>
		/// Capture the Change event so that there's no null ref exception when it's fired
		/// </summary>
		private void OnChange(object sender, EventArgs e) {
			; //Do nothing
		}

		/// <summary>
		/// Unscaled height
		/// </summary>
		private float nativeHeight() {
			return Math.Max(left.Height,knob.Height)*YScale;
		}

		#region Lockable Members
		public void Lock() {
			locked=true;
			knob.Lock();	//The knob is a SpriteButton, so it is Lockable
		}
		public void Unlock() {
			locked=false;
			knob.Unlock();	//The knob is a SpriteButton, so it is Lockable
		}
		public bool Locked() {
			return locked;
		}
		#endregion

	}
}
