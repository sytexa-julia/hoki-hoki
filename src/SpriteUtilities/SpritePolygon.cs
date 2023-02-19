using SharpDX;
using SharpDX.Direct3D9;
using System;

namespace SpriteUtilities {
	/// <summary>
	/// Polygonal surface over which a SpriteTexture is tiled TODO: Implement tint changes
	/// </summary>
	public class SpritePolygon : EffectObject {
		private IndexBuffer ib;
		private VertexBuffer vb;
		private PositionColoredTextured[] verts;

		private Vector2 size;
		private SpriteTexture tex;
		private int numIndices,numVertices;
		private bool useIB;
		private VertexFormat format=PositionColoredTextured.Format;

		public SpritePolygon(Device device,SpriteTexture tex) : base(device) {
			//Store tex reference
			this.tex=tex;
		}

		public SpritePolygon(Device device,SpriteTexture tex,Vector2[] vertices) : base(device) {
			//Store tex reference
			this.tex=tex;

			//Prepare the data
			Build(vertices);
		}

		public SpritePolygon(Device device,SpriteTexture tex,Vector2[] vertices,short[] indices) : base(device) {
			//Store tex reference
			this.tex=tex;

			//Prepare the data
			Build(vertices,indices);
		}

		/*TEST
		//This code is still being tested, and may cause some compatibility problems
		~SpritePolygon() {
			if (vb!=null) vb.Dispose();
			if (ib!=null) ib.Dispose();
		}
		//END TEST*/

		public float Width {
			get { return size.X*XScale; }
			set { XScale=value/size.X; }
		}

		public float Height {
			get { return size.Y*YScale; }
			set { YScale=value/size.Y; }
		}

		public override System.Drawing.Color Tint {
			get { return base.Tint; }
			set {
				base.Tint = value;
				for (int i=0;i<verts.Length;i++) verts[i].Color=value.ToArgb();
				vb.Lock(0, 0, LockFlags.Discard).WriteRange(verts);
				vb.Unlock();
			}
		}

		public VertexFormat Format {
			get { return format; }
			set { format=value; }
		}

		public void Build(Vector2[] vertices) {
			Vector2	//Store minimum and maximum positions to calculate size
				min=new Vector2(float.PositiveInfinity,float.PositiveInfinity),
				max=new Vector2(float.NegativeInfinity,float.NegativeInfinity);

			//Create vertices
			verts=new PositionColoredTextured[vertices.Length];
			for (int i=0;i<vertices.Length;i++) {
				verts[i]=new PositionColoredTextured(vertices[i].X,
					vertices[i].Y,
					1.0f,
					Color.White.ToBgra(),//Color.White.ToArgb(),
					vertices[i].X/tex.Width,
					vertices[i].Y/tex.Height);
				min.X=Math.Min(vertices[i].X,min.X);
				min.Y=Math.Min(vertices[i].Y,min.Y);
				max.X=Math.Max(vertices[i].X,max.X);
				max.Y=Math.Max(vertices[i].Y,max.Y);
			}

			size=max-min;

			//Make a vertex buffer
			Build(verts);

			//Don't use an index buffer
			useIB=false;
		}

		public void Build(Vector2[] vertices,short[] indices) {
			//Build the vertex buffer
			Build(vertices);

			//Build the index buffer
			makeIB(indices);

			//This flag lets drawPrimitives know to use it
			useIB=true;
		}

		public void Build(PositionColoredTextured[] verts) {
			numVertices=verts.Length;
            vb = new VertexBuffer(device, PositionColoredTextured.StrideSize * verts.Length, Usage.Dynamic | Usage.WriteOnly, PositionColoredTextured.Format, Pool.Default);
            //vb=new VertexBuffer(typeof(PositionColoredTextured),verts.Length,device,Usage.Dynamic | Usage.WriteOnly,CustomVertex.PositionColoredTextured.Format,Pool.Default);
            vb.Lock(0, 0, LockFlags.Discard).WriteRange(verts);
			vb.Unlock();

			useIB=false;
		}

		public void Build(PositionColoredTextured[] verts,short[] indices) {
			Build(verts);
			makeIB(indices);
			useIB=true;
		}

		private void makeIB(short[] indices) {
			//Make an index buffer
			numIndices=indices.Length;
            ib = new IndexBuffer(device, sizeof(short) * indices.Length, Usage.Dynamic | Usage.WriteOnly, Pool.Default, false);
            //ib=new IndexBuffer(typeof(short),indices.Length,device,Usage.Dynamic|Usage.WriteOnly,Pool.Default);

			ib.Lock(0, 0, LockFlags.Discard).WriteRange(indices);
			ib.Unlock();
		}

		protected override void deviceDraw(Matrix trans) {
			//Set pipeline state
			device.SetTransform(TransformState.World, trans);
			device.SetStreamSource(0,vb,0, PositionColoredTextured.StrideSize);
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
			if (useIB) {
				device.Indices=ib;
				device.DrawIndexedPrimitive(PrimitiveType.TriangleList,0,0,numVertices,0,numIndices/3);
			} else device.DrawPrimitives(PrimitiveType.TriangleList,0,numVertices/3);
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