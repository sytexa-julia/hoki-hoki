using System;
using System.Drawing;
using System.Collections;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SpriteUtilities {
	/// <summary>
	/// A class containing a textured quad and transformations of it. May contain nested child sprites to which
	/// the parent's transformations as well as their own are applied.
	/// </summary>
	public class SpriteObject : EffectObject {
		#region vars
		//D3D primitives/texture
		protected CustomVertex.PositionColoredTextured[] verts;	//List of vertices to put in the buffer
		protected VertexBuffer vb;		//Textured quad
		protected SpriteTexture tex;	//Texture with width, height, and frame count

		//Filtering
		protected TextureFilter filter;	//Filter to draw with, if setFilter is true
		protected bool setFilter;		//Whether the filter should be changed to draw

		//Shader vars
		protected ArrayList fxConstants;//List of constants to pass to effects on draw

		//State
		protected int frame;			//Current frame of the texture
		#endregion

		#region getset
		/// <summary>
		/// The scaled width of the sprite
		/// </summary>
		virtual public float Width {
			get {
				if (tex==null) return 0;
				return scale.X*tex.Width;
			}
			set { if (tex!=null) scale.X=value/tex.Width; }
		}
		
		/// <summary>
		/// The scaled height of the sprite
		/// </summary>
		virtual public float Height {
			get { 
				if (tex==null) return 0;
				return scale.Y*tex.Height;
			}
			set { if (tex!=null) scale.Y=value/tex.Height; }
		}

		/// <summary>
		/// Frame to draw, counting from 0
		/// </summary>
		virtual public int Frame {
			get { return frame; }
			set {
				//Set the frame
				frame=value;
				setFrame(value);
			}
		}

		virtual public float AbsoluteXScale {
			get {
				if (parent==null) return XScale;
				else return parent.XScale*XScale;
			}
		}

		virtual public float AbsoluteYScale {
			get {
				if (parent==null) return YScale;
				else return parent.YScale*YScale;
			}
		}

		virtual public float AbsoluteWidth {
			get {
				if (tex==null) return 0;
				else return AbsoluteXScale*tex.Width;
			}
		}

		virtual public float AbsoluteHeight {
			get {
				if (tex==null) return 0;
				else return AbsoluteYScale*tex.Height;
			}
		}

		virtual public TextureFilter DrawFilter {
			get { return filter; }
			set {
				filter=value;
				setFilter=true;
			}
		}

		/// <summary>
		/// The format of vertices used in the textured quad
		/// </summary>
		public static VertexFormats VertexFormat {
			get { return CustomVertex.PositionColoredTextured.Format; }
		}
		#endregion		

		#region constructor
		public SpriteObject(Device device,SpriteTexture tex) : base(device) {
			//Store the texture
			this.tex=tex;

			if (tex!=null) {
				//Create vertices for a textured quad (4 vertices/triangle strip)
				verts=new CustomVertex.PositionColoredTextured[4];
				verts[0]=new CustomVertex.PositionColoredTextured(0.0f,0.0f,1.0f,Color.White.ToArgb(),0.0f,1.0f);			//Top left
				verts[1]=new CustomVertex.PositionColoredTextured(0.0f,tex.Height,1.0f,Color.White.ToArgb(),0.0f,0.0f);		//Bottom left
				verts[2]=new CustomVertex.PositionColoredTextured(tex.Width,0.0f,1.0f,Color.White.ToArgb(),1.0f,1.0f);		//Top right
				verts[3]=new CustomVertex.PositionColoredTextured(tex.Width,tex.Height,1.0f,Color.White.ToArgb(),1.0f,0.0f);//Bottom right

				//Initialize the vertex buffer
				vb=new VertexBuffer(typeof(CustomVertex.PositionColoredTextured),4,device,Usage.Dynamic|Usage.WriteOnly,CustomVertex.PositionColoredTextured.Format,Pool.Default);
				resetBuffer();
			}

			//Set the frame
			frame=0;
			setFrame(0);

			//Construct an ArrayList to hold shader constants
			fxConstants=new ArrayList();
		}

		#endregion

		#region device
		public static void SetRenderStates(Device device) {
			//Basic renderstates
			device.RenderState.Lighting=false;
			device.RenderState.CullMode=Cull.None;
			device.SamplerState[0].MinFilter=TextureFilter.Linear;
			device.SamplerState[0].MagFilter=TextureFilter.Linear;

			//Blending states
			device.TextureState[0].ColorOperation=TextureOperation.Modulate;	//Required for vertex color blending
			device.TextureState[0].ColorArgument1=TextureArgument.TextureColor;	//Required for vertex color and alpha blending
			device.TextureState[0].ColorArgument2=TextureArgument.Diffuse;		//Required for vertex color blending
			device.TextureState[0].AlphaOperation=TextureOperation.Modulate;	//Required for pixel and vertex alpha blending
			device.TextureState[0].AlphaArgument1=TextureArgument.TextureColor;	//Required for pixel alpha blending
			device.TextureState[0].AlphaArgument2=TextureArgument.Diffuse;		//Required for vertex alpha blending
			device.RenderState.SourceBlend=Blend.SourceAlpha;					//Required for alpha blending
			device.RenderState.DestinationBlend=Blend.InvSourceAlpha;			//Required for alpha blending
			device.RenderState.AlphaBlendEnable=true;							//Required for alpha blending
		}

		public static void SetupCamera(Device device,int screenWidth,int screenHeight) {
			device.Transform.Projection=Project(screenWidth,screenHeight);
			device.Transform.View=Matrix.Identity;
		}

		/// <summary>
		/// Returns an orthoganal matrix for rendering such that one x- or y-unit equals one pixel on the screen.
		/// </summary>
		/// <param name="width">Viewport width</param>
		/// <param name="height">Viewport height</param>
		public static Matrix Project(int width,int height) {
			return Matrix.OrthoOffCenterLH(0.5f,width+0.5f,height+0.5f,0.5f,0.0f,1.0f);
		}
		#endregion

		#region buffer

		protected override void checkUpdates() {
			base.checkUpdates ();
			
			if (colorNeedsUpdate)	setColor(Color.FromArgb((int)alpha,color));	//Apply the color to the vertices
			if (bufferNeedsUpdate)	resetBuffer();								//Put the vertics in the buffer
		}

		private void setFrame(int frame) {
			if (tex==null) return;

			//Update the vertices' texture coordinates
			Vector4 coords=tex.TexCoords(frame);
			verts[0].Tu=verts[1].Tu=coords.X; //Left
			verts[0].Tv=verts[2].Tv=coords.Y; //Top
			verts[2].Tu=verts[3].Tu=coords.Z; //Right
			verts[1].Tv=verts[3].Tv=coords.W; //Bottom

			//Ensure that the buffer will be updated before drawing
			bufferNeedsUpdate=true;
		}

		private void setColor(Color color) {
			if (tex==null) return;

			//Set vertex colors
			verts[0].Color=verts[1].Color=verts[2].Color=verts[3].Color=color.ToArgb();
			
			//Ensure that the buffer will be updated before drawing
			bufferNeedsUpdate=true;
		}
		

		private void resetBuffer() {
			if (tex==null) return;

			vb.SetData(verts,0,LockFlags.Discard);
		}
		#endregion

		#region drawing
		public override void Draw(Matrix parentMatrix, Vector2 parentShift) {
			TextureFilter oldFilter=device.SamplerState[0].MagFilter;
			if (setFilter && filter!=oldFilter) device.SamplerState[0].MagFilter=device.SamplerState[0].MinFilter=filter;

			base.Draw (parentMatrix, parentShift);

			//By the end of this method the buffer has been updated if necessary
			bufferNeedsUpdate=false;

			if (setFilter && filter!=oldFilter) device.SamplerState[0].MagFilter=device.SamplerState[0].MinFilter=oldFilter;
		}

		protected override void deviceDraw(Matrix trans) {
			base.deviceDraw(trans);
			if (tex!=null) { //If a texture has been provided, draw
				//Set pipeline state
				device.Transform.World=trans;
				device.SetStreamSource(0,vb,0);
				device.SetTexture(0,tex.Tex);
				device.VertexFormat=CustomVertex.PositionColoredTextured.Format;

				if (current!=null) { //Method 1: use an effect (if the base class set one)
					//Pass constant values to the effect
					foreach (FXConstant fxc in allConstants) sendFXC(fxc,trans);

					//Draw with the effect
					int passes=current.Begin(FX.None);
					for (int i=0;i<passes;i++) {
						current.BeginPass(i);
						drawPrimitives();
						current.EndPass();
					}
					current.End();
				} else
					//Method 2: use the fixed function pipeline
					drawPrimitives();
			}
		}

		protected override void sendFXC(FXConstant fxc,Matrix trans) {
			if (current!=null) {
				switch (fxc.Type) {
					case ConstType.Color:
						current.SetValue(fxc.Name,new Vector4(color.R,color.G,color.B,color.A));
						break;
					case ConstType.Texture:
						if (tex!=null) current.SetValue(fxc.Name,tex.Tex); //Can't set with a null tex, but it doesn't matter because the effect routine won't be run
						break;
					case ConstType.WorldMatrix:
						current.SetValue(fxc.Name,trans);
						break;
				}
			}
		}

		
		/// <summary>
		/// Makes a Device.DrawPrimitives call
		/// </summary>
		private void drawPrimitives() {
			device.DrawPrimitives(PrimitiveType.TriangleStrip,0,2);
		}
		#endregion
	}
}