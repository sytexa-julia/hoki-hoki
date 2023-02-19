using System;
using System.Drawing;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct3D9;
using SpriteUtilities;

namespace Hoki {
	/// <summary>
	/// A single pixel in the FLECKO.NET intro effect
	/// </summary>
	public class FleckoPX : SpriteObject,Updateable {
		private float
			bg,				//Value of blue and green in tint
			bgTarget,		//Value towards which bg approaches
			timeLeft;		//Time remaining until fade begins
		private Vector2
			offset,			//Offset from position caused by mouse influence
			offsetTarget;	//Target offset (offset gets closer to this each frame)
		private const int
			radius=50;		//Size of mouse's influence
		private const float
			liveTime=3,		//Total time to exist before fading
			attraction=6;	//Attraction to target location
		protected readonly float
			pxDist;			//Distance between pixels

		protected override Vector2 coords {
			get { return base.coords+offset; }
		}

		public FleckoPX(Form frm,Device device,SpriteTexture tex,float pxDist) : base(device,tex) {
			//Distance
			this.pxDist=pxDist;

			//Defaults
			bg=255;
			bgTarget=0;
			offset=new Vector2();

			//Mouse events
			frm.MouseMove+=new MouseEventHandler(OnMouseMove);	//Change offset relative to mouse distance
			frm.MouseDown+=new MouseEventHandler(OnMouseDown);	//Change color relative to mouse distance, lock colors
			frm.MouseUp+=new MouseEventHandler(OnMouseUp);		//Unlock colors

			//Set fade countdown
			timeLeft=liveTime;
		}

		public void Unhook(Form frm) {
			frm.MouseMove-=new MouseEventHandler(OnMouseMove);
			frm.MouseDown-=new MouseEventHandler(OnMouseDown);
			frm.MouseUp-=new MouseEventHandler(OnMouseUp);
		}

		virtual public void Update(float elapsedTime) {
			//Decrease and update the blue/green tint values
			if (bg!=bgTarget) {
				//Approach target color
				bg+=(bgTarget-bg)*elapsedTime*3;
				//Snap to target when close
				if (Math.Abs(bg-bgTarget)<0.5f) bg=bgTarget;

				//Adjust the sprite's tint value
				Tint= System.Drawing.Color.FromArgb(255,(int)bg,(int)bg);
			}

			//Move closer to the targeted offset
			offset.X+=(offsetTarget.X-offset.X)*attraction*elapsedTime;
			offset.Y+=(offsetTarget.Y-offset.Y)*attraction*elapsedTime;

			//Countdown to fade/fade if it's time
			timeLeft-=elapsedTime;
			if (timeLeft<0) Alpha=(int)Math.Max(Alpha-elapsedTime*100,0);
		}

		/// <summary>
		/// Converts global mouse coordinates to parent's coordinate space
		/// </summary>
		/// <param name="x">Mouse's screen X</param>
		/// <param name="y">Mouse's screen Y</param>
		private Vector2 convertMouse(int x,int y) {
			Vector2 mousePos=new Vector2(x,y);
			if (parent!=null) parent.GlobalToLocal(ref mousePos);
			return mousePos;
		}

		private void OnMouseMove(object sender, MouseEventArgs e) {
			//Get usable coordinates
			Vector2 mousePos=convertMouse(e.X,e.Y);

			//Get distances
			float xd=mousePos.X-X,yd=mousePos.Y-Y;
			float d=Math.Max((float)Math.Sqrt(Math.Pow(xd,2)+Math.Pow(yd,2)),1.0f);

			//If the pixel is outside the mouse's influence radius, no offset 
			if (d>radius) offsetTarget.X=offsetTarget.Y=0.0f;
			//Otherwise, set offset by mouse distance
			else {
				offsetTarget.X=(float)(-xd/d*Math.Sqrt(radius-d)*pxDist);
				offsetTarget.Y=(float)(-yd/d*Math.Sqrt(radius-d)*pxDist);
			}

			//Add to whiteness if pixel colors are locked
			OnMouseDown(this,e);
		}

		private void OnMouseDown(object sender, MouseEventArgs e) {
			if (e.Button==MouseButtons.Right) {
				//Get usable coordinates
				Vector2 mousePos=convertMouse(e.X,e.Y);

				//Get distance
				float d=(float)Math.Sqrt(Math.Pow(mousePos.X-X,2)+Math.Pow(mousePos.Y-Y,2));

				//Set whiteness target from 0 to 255 based on distance
				if (d<radius) bgTarget=Math.Max(bgTarget,(radius-d)/radius*255);
			}
		}

		private void OnMouseUp(object sender,MouseEventArgs e) {
			if (e.Button==MouseButtons.Right) bgTarget=0; //Reset color target
		}
	}
}