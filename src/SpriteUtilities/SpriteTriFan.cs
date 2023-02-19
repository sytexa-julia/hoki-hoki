using System;
using SharpDX;
using SharpDX.Direct3D9;

namespace SpriteUtilities {
	/// <summary>
	/// Triangle fan that can be transformed as an EffectObject TODO: Implement tint changes
	/// </summary>
	public class SpriteTriFan : EffectObject {
		private PositionColoredTextured[] verts;
		private VertexBuffer vb;

		private SpriteTexture tex;

		public SpriteTriFan(Device device,SpriteTexture tex,Vector2[] vertices,Vector2[] texCoords) : base(device) {
			//Store a texture reference
			this.tex=tex;

			//Create a vertex list
			verts=new PositionColoredTextured[vertices.Length];
			for (int i=0;i<vertices.Length;i++)
				verts[i]=new PositionColoredTextured(vertices[i].X,vertices[i].Y,1.0f,System.Drawing.Color.White.ToArgb(),texCoords[i].X,texCoords[i].Y);

            //Make a vertex buffer
            vb = new VertexBuffer(device, PositionColoredTextured.StrideSize * verts.Length, Usage.Dynamic | Usage.WriteOnly, PositionColoredTextured.Format, Pool.Default);
            //vb=new VertexBuffer(typeof(PositionColoredTextured),verts.Length,device,Usage.Dynamic | Usage.WriteOnly,CustomVertex.PositionColoredTextured.Format,Pool.Default);
            vb.Lock(0, 0, LockFlags.Discard).WriteRange(verts);
			vb.Unlock();
		}


		protected override void deviceDraw(Matrix trans) {
			//Set pipeline state
			device.SetTransform(TransformState.World, trans);
			device.SetStreamSource(0,vb,0,PositionColoredTextured.StrideSize);
			device.SetTexture(0,tex.Tex);

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

		private void drawPrimitives() {
			device.DrawPrimitives(PrimitiveType.TriangleFan,0,verts.Length-2);
		}

		protected override void sendFXC(SpriteUtilities.EffectObject.FXConstant fxc, Matrix trans) {
			if (current!=null) {
				switch (fxc.Type) {
					case ConstType.Color:
						current.SetValue(fxc.Name,new Vector4(color.R,color.G,color.B,color.A));
						break;
					case ConstType.Texture:
						if (tex!=null) current.SetTexture(fxc.Name,tex.Tex); //Can't set with a null tex, but it doesn't matter because the effect routine won't be run
						break;
					case ConstType.WorldMatrix:
						current.SetValue(fxc.Name,trans);
						break;
				}
			}
		}
	}
}
