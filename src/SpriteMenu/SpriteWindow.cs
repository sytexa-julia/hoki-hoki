using System;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SpriteUtilities;
using FloatMath;

namespace SpriteMenu {
	/// <summary>
	/// A sizeable window derived from SpriteObject
	/// </summary>
	public class SpriteWindow : SpriteObject,Updateable,Lockable {
		#region sprite
		//All of the sprites that make up the window graphic
		private SpriteObject
			background,	//Background
			top,		//Walls
			bottom,
			left,
			right,
			tlc,		//Corners
			trc,
			blc,
			brc,
			content;	//Holds window content
		private SpriteText
			header;		//Name of the window
		#endregion

		#region state
		protected ArrayList
			lockables;		//Children added to the content object (needed for locking interface elements)
		protected float
			minWidth,		//Minimum width to use for rendering
			minHeight,
			width,			//Current displayed width of the window
			height,
			targetWidth,	//Width that the window is approaching
			targetHeight,
			targetAlpha;	//Alpha that the window is approaching
		protected bool
			open,
			locked;
		#endregion

		#region const
		private const float
			scaleRate=10.0f,	//Rate of change in scale when target!=current
			snapDist=1.0f,		//Distance at which dimensions will snap to their target
			switchDist=10.0f;	//Distance at which height begins getting scaled, too
		#endregion

		#region getset
		/// <summary>
		/// Actual width of entire window. When set, the window will snap to the provided size
		/// </summary>
		public override float Width {
			get { return width; }
			set { width=targetWidth=value; }
		}

		/// <summary>
		/// Actual height of entire window. When set, the window will snap to the provided size
		/// </summary>
		public override float Height {
			get { return height; }
			set { height=targetHeight=value; }
		}

		/// <summary>
		/// Width of space inside the window
		/// </summary>
		public float ClientWidth {
			get { return Width-left.Width-right.Width; }
			set { Width=value+left.Width+right.Width; }
		}

		/// <summary>
		/// Height of space inside the window
		/// </summary>
		public float ClientHeight {
			get { return Height-top.Height-bottom.Height; }
			set { Height=value+top.Width+bottom.Width; }
		}

		/// <summary>
		/// Width of space inside the window at target size
		/// </summary>
		public float TargetClientWidth {
			get { return targetWidth-left.Width-right.Width; }
			set { TargetWidth=value+left.Width+right.Width; }
		}

		/// <summary>
		/// Height of space inside the window at target size
		/// </summary>
		public float TargetClientHeight {
			get { return targetHeight-left.Height-right.Height; }
			set { TargetHeight=value+top.Height+bottom.Height; }
		}

		/// <summary>
		/// The width towards which the window is gradually approaching
		/// </summary>
		public float TargetWidth {
			get { return targetWidth; }
			set { targetWidth=value; }
		}
		
		/// <summary>
		/// The height towards which the window is gradually approaching
		/// </summary>
		public float TargetHeight {
			get { return targetHeight; }
			set { targetHeight=value; }
		}

		/// <summary>
		/// The window's header text
		/// </summary>
		public string Text {
			get { return header.Text; }
			set { header.Text=value; }
		}

		#endregion

		public SpriteWindow(Device device,Sprite sprite,Font font,WindowTexture tex) : base(device,null) {
			//Initialize the objects
			background=new SpriteObject(device,tex.background);
			top=new SpriteObject(device,tex.top);
			bottom=new SpriteObject(device,tex.bottom);
			left=new SpriteObject(device,tex.left);
			right=new SpriteObject(device,tex.right);
			tlc=new SpriteObject(device,tex.tlc);
			trc=new SpriteObject(device,tex.trc);
			blc=new SpriteObject(device,tex.blc);
			brc=new SpriteObject(device,tex.brc);
			content=new SpriteObject(device,null);

			//Make text with no clipping
			header=new SpriteText(device,sprite,font,0,(int)top.Height);
			header.Format=DrawTextFormat.NoClip|DrawTextFormat.VerticalCenter;

            //Add everything to the draw list - must use base.Add because Add is overridden in this class
			base.Add(background);
			base.Add(top);
			base.Add(bottom);
			base.Add(right);
			base.Add(left);
			base.Add(tlc);
			base.Add(trc);
			base.Add(blc);
			base.Add(brc);
			base.Add(content);
			base.Add(header);

			//Make an ArrayList to hold Lockable children
			lockables=new ArrayList();

			//Set minimum sizes to accommodate unscaled SpriteObjects
			minWidth=Math.Max(Math.Max(tlc.Width,left.Width),blc.Width)+Math.Max(Math.Max(trc.Width,right.Width),brc.Width);
			minHeight=Math.Max(Math.Max(tlc.Height,top.Height),trc.Height)+Math.Max(Math.Max(blc.Height,bottom.Height),brc.Height);

			//Use minimums as defaults
			width=targetWidth=minWidth;
			height=targetHeight=minHeight;
			Alpha=targetAlpha=0;
			open=false;

			//Set the sprites to their default positions and sizes
			initializePositions();
		}

		#region childlist
		/// <summary>
		/// Add a child SpriteObject to the content SpriteObject
		/// </summary>
		public void AddContent(SpriteObject child) {
			content.Add(child);
		}

		/// <summary>
		/// Remove a child SpriteObject from the content SpriteObject
		/// </summary>
		/// <param name="child"></param>
		public void RemoveContent(SpriteObject child) {
			content.Remove(child);
		}

		/// <summary>
		/// Adds an object to the lock list - when Lock() and Unlock() are called, the Lockable will be Lock()ed and
		/// Unlock()ed accordingly
		/// </summary>
		public void AddLockable(Lockable child) {
			lockables.Add(child);
		}

		/// <summary>
		/// Removes a child that was added with AddLockable
		/// </summary>
		public void RemoveLockable(Lockable child) {
			lockables.Remove(child);
		}
		#endregion

		public void Open() {
			if (open==false) alpha=width=height=0; //If it was closed, must reset width and height
			targetAlpha=MaxAlpha;	//Fade in
			open=true;				//Set open=true so that scale will update
		}

		public void Close() {
			open=false;
			targetAlpha=0;
		}

		/// <summary>
		/// Set positions that don't change, then call updatePositions()
		/// </summary>
		private void initializePositions() {
			//Set constant positions
			background.X=content.X=tlc.Width;
			background.Y=content.Y=top.Height;
			top.X=tlc.Width;
			left.Y=tlc.Height;
			bottom.X=blc.Width;
			right.Y=trc.Height;
			header.X=top.X;
			header.Y=top.Y+2;	//Text is always a little off

			//Set variable positions
			updatePositions();
		}

		/// <summary>
		/// Changes the sprites' positions and scales according to the current size
		/// </summary>
		private void updatePositions() {
			//Clamp size values within the allowed range (without altering the instance variables)
			float width=Math.Max(this.width,minWidth);
			float height=Math.Max(this.height,minHeight);

			//Set positions
			trc.X=width-trc.Width;			//Top right corner
			brc.X=width-brc.Width;			//Bottom right corner
			brc.Y=height-brc.Height;
			blc.Y=height-blc.Height;		//Bottom left corner
			right.X=width-right.Width;		//Right bar;
			bottom.Y=height-bottom.Height;	//Bottom bar

			//Scale bars and bg
			top.Width=width-tlc.Width-trc.Width;
			bottom.Width=width-blc.Width-brc.Width;
			left.Height=height-tlc.Height-blc.Height;
			right.Height=height-trc.Height-brc.Height;
			background.Width=width-left.Width-right.Width;
			background.Height=height-top.Height-bottom.Height;

			//Scale content
			float targetBGWidth=targetWidth-left.Width-right.Width;
			if (targetBGWidth!=0) content.XScale=background.Width/targetBGWidth; else content.XScale=0;

			float targetBGHeight=targetHeight-top.Height-bottom.Height;
			if (targetBGHeight!=0) content.YScale=background.Height/targetBGHeight; else content.YScale=0;
		}

		#region Updateable Members
		virtual public void Update(float elapsedTime) {
			//If the menu is opened, scale up and fade in
			if (open) {
				//Change scales: do width first
				width+=(targetWidth-width)*elapsedTime*scaleRate;			//Scale
				if (Math.Abs(targetWidth-width)<snapDist) width=targetWidth;//Snap to target

				//If width is done, change height
				if (Math.Abs(width-targetWidth)<switchDist) {
					height+=(targetHeight-height)*elapsedTime*scaleRate;			//Scale
					if (Math.Abs(targetHeight-height)<snapDist) height=targetHeight;//Snap to target
				}
			}

			//Move towards target alpha no matter what
			Alpha+=(targetAlpha-Alpha)*elapsedTime*scaleRate;
			if (Math.Abs(targetAlpha-Alpha)<snapDist) Alpha=targetAlpha;				

			//Change values
			updatePositions();
		}
		#endregion

		#region Lockable Members
		public void Lock() {
			if (!locked) {
				//Set state to locked
				locked=true;

				//Lock each Lockable child
				foreach (Lockable child in lockables) child.Lock();
			}
		}
		public void Unlock() {
			if (locked) {
				//Set state to unlocked
				locked=false;

				//Unlock each Lockable child
				foreach (Lockable child in lockables) child.Unlock();
			}
		}
		public bool Locked() {
			return locked;
		}
		#endregion
	}
}