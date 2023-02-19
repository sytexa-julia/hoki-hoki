using System;
using System.Collections;
using SpriteUtilities;
using SpriteMenu;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Hoki {
	/// <summary>
	/// A SpriteMenu with the capability to render a blurred version of itself to texture and blend with it.
	/// </summary>
	public class BlurWindow : SpriteWindow {
		protected Effect blurEffect;	//Effect to use for the blur render
		protected string
			blurTechnique,		//Technique to use for the blur render
			worldConstName,		//Name of the effect constant that world matrix should be passed to
			projConstName,		//Name of the effect constant that projection matrix should be passed to
			blurSizeConstName,	//Name of the effect constant that blur size should be passed to
			widthConstName,		//Name of the effect constant that texture width should be passed to
			heightConstName;
		protected int
			blurIndex;			//The current blur level. At 0, no blur layers are visible.
		protected ArrayList
			blurLayers;			//List of SpriteObjects containing blur images

		/// <param name="font">Font to use for header text</param>
		/// <param name="tex">Set of textures to draw the window with</param>
		/// <param name="constants">Set of shader information</param>
		public BlurWindow(Device device,Sprite sprite,Font font,WindowTexture tex,BlurConstants constants) : base(device,sprite,font,tex) {
			//Store effect information
			this.blurEffect=constants.blurEffect;
			this.blurTechnique=constants.blurTechnique;
			this.worldConstName=constants.worldConstName;
			this.projConstName=constants.projConstName;
			this.blurSizeConstName=constants.blurSizeConstName;
			this.widthConstName=constants.widthConstName;
			this.heightConstName=constants.heightConstName;

			//Create blur layer list
			blurLayers=new ArrayList();
		}

		/// <summary>
		/// Updates the blurred object to reflect the menu's current state
		/// </summary>
		/// <param name="levels">The number of successive levels of bluring to render</param>
		/// <param name="minBlur">The least blurred level, in terms of pixel blur range (this will be level 1; level 0 is unblurred)</param>
		/// <param name="maxBlur">The most blurred level, in terms of pixel blur range</param>
		public void RenderBlur(int levels,float minBlur,float maxBlur) {
			//Declare SpriteObjects for later use
			SpriteObject rendered,blurred;

			//Clear the old list of blurred sprites
			foreach (SpriteObject blurLayer in blurLayers) Remove(blurLayer);
			blurLayers.Clear();

			//The dimensions the window will be when rendered, including space taken up by blurring
			int windowWidth=(int)(width+2*maxBlur);
			int windowHeight=(int)(height+2*maxBlur);

			//The size of the texture
			int texWidth=textureDimension((int)width);
			int texHeight=textureDimension((int)height);

			//Set the device's projection so that it fits the texture size
			Matrix projection=device.Transform.Projection;						//Store old projection
			Matrix windowProjection=SpriteObject.Project(texWidth,texHeight);	//Get new projection
			if (current!=null) current.SetValue("Proj",windowProjection);			//Pass projection to effect if necessary
			device.Transform.Projection=windowProjection;						//Set device's projection in case using FF pipeline

			//Create textures and a matching RTS
			Texture sharpTex=new Texture(device,texWidth,texHeight,1,Usage.RenderTarget,Format.A8R8G8B8,Pool.Default);
			Texture blurTex; //Use to store blur levels later
			RenderToSurface rts=new RenderToSurface(device,texWidth,texHeight,Format.A8R8G8B8,false,DepthFormat.Unknown);

			//Create a viewport, also equal in dimensions to the texture size
			Viewport view=new Viewport();
			view.Width=texWidth;
			view.Height=texHeight;
			view.MaxZ=1;

			//Start the scene, clear the device...
			rts.BeginScene(sharpTex.GetSurfaceLevel(0),view);
			device.Clear(ClearFlags.Target,System.Drawing.Color.FromArgb(0,0,0,0).ToArgb(),0,0);

			//Get the inverse transformation matrix - if drawing with this, transformations will be canceled
			Matrix trans=LocalTransform();
			trans=Matrix.Invert(trans);

			//Shift trans by blur size so that there is space for any blurred pixels the the top and left of the object
			trans=Matrix.Multiply(trans,Matrix.Translation(maxBlur,maxBlur,0));

			Draw(trans,Vector2.Empty);

			//Finish the scene
			rts.EndScene(Filter.None);

			//Put the texture in a SpriteObject so it can easily be drawn again
			rendered=new SpriteObject(device,new SpriteTexture(device,sharpTex,null,texWidth,texHeight));

			//Prepare the effect
			blurEffect.Technique=blurTechnique;
			blurEffect.SetValue(worldConstName,Matrix.Identity);
			blurEffect.SetValue(widthConstName,texWidth);
			blurEffect.SetValue(heightConstName,texHeight);

			for (int i=0;i<levels;i++) {
				//Determine the blur size to user for this level
				float blurSize=minBlur+i*(maxBlur-minBlur)/levels;

				//Create a new texture
				blurTex=new Texture(device,texWidth,texHeight,1,Usage.RenderTarget,Format.A8R8G8B8,Pool.Default);

				//Update the effect's blur size
				blurEffect.SetValue(blurSizeConstName,blurSize);
				
				//Begin the blurry render
				rts.BeginScene(blurTex.GetSurfaceLevel(0),view);
				device.Clear(ClearFlags.Target,System.Drawing.Color.FromArgb(0,0,0,0).ToArgb(),0,0);

				//Cycle through effect passes and draw
				int passes=blurEffect.Begin(FX.None);
				for (int n=0;n<passes;n++) {
					blurEffect.BeginPass(n);
					rendered.Draw();
					blurEffect.EndPass();
				}
				blurEffect.End();

				//Finish the scene up with a linear filter so that decimal blurriness values work
				rts.EndScene(Filter.Linear);

				//Put the new texture in a SpriteObject, add it to self, and add it to the blur layers list
				blurred=new SpriteObject(device,new SpriteTexture(device,blurTex,null,texWidth,texHeight));
				blurred.X=-maxBlur;
				blurred.Y=-maxBlur;
				blurred.Alpha=0;
				Add(blurred);
				blurLayers.Add(blurred);
			}

			//Restore the old projection
			if (current!=null) current.SetValue("Proj",projection);
			device.Transform.Projection=projection;
		}

		/// <summary>
		/// Sets the current level of blurriness.
		/// </summary>
		/// <param name="level">The level to use. Must be within the range created by the last RenderBlur() call</param>
		public void SetBlur(int level) {
			blurIndex=level;
		}

		/// <summary>
		/// Returns the first power of two greater than the input value
		/// </summary>
		private int textureDimension(int imgDimension) {
			int size=1;
			while (size<imgDimension) size*=2;
			return size;
		}

		public override void Update(float elapsedTime) {
			base.Update (elapsedTime);

			int blurTarget;
			for (int i=0;i<blurLayers.Count;i++) {
				if (i==blurIndex-1) blurTarget=byte.MaxValue; else blurTarget=0;
				SpriteObject blurLayer=(SpriteObject) blurLayers[i];
				blurLayer.Alpha+=(blurTarget-blurLayer.Alpha)*elapsedTime*3;
			}
		}
	}
}
