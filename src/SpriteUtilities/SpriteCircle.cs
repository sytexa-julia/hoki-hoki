using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using FloatMath;

namespace SpriteUtilities {
	/// <summary>
	/// Draws a circle using a line strip
	/// NOTE: Doesn't support tinting/alpha operations
	/// 
	/// MORE IMPORTANT NOTE: This class doesn't work, and I haven't figured out why yet.
	/// </summary>
	public class SpriteCircle : TransformedObject {
		private VertexBuffer vb;
		private bool antialias;
		private float radius;
		private int detail;

		public SpriteCircle(Device device,float radius,int detail,int color) : base(device) {
			//Store the radius locally
			this.radius=radius;
			this.detail=detail;

			//Create a vertex array
			CustomVertex.PositionColored[] verts=new CustomVertex.PositionColored[detail+1];
			float degPerLine=2*FMath.PI/detail;
			for (int i=0;i<detail;i++) {
				float ang=degPerLine*i;
				verts[i]=new CustomVertex.PositionColored(FMath.Cos(ang)*radius,FMath.Sin(ang)*radius,1,color);
			}
			verts[verts.Length-1]=verts[0];	//Copy the first point into the last slot to connect back around

			//Create a VB and load the verts into it
			vb=new VertexBuffer(typeof(CustomVertex.PositionColored),detail+1,device,Usage.WriteOnly,CustomVertex.PositionColored.Format,Pool.Default);
			vb.SetData(verts,0,LockFlags.None);
		}

		protected override void deviceDraw(Matrix trans) {
			device.Transform.World=trans;
			device.SetStreamSource(0,vb,0);
			device.VertexFormat=CustomVertex.PositionColored.Format;
			device.DrawPrimitives(PrimitiveType.LineStrip,0,detail);
		}

		public bool Antialias {
			get { return antialias; }
			set { antialias=value; }
		}

		public float Width {
			get { return 2*radius; }
		}

		public float Height {
			get { return 2*radius; }
		}
	}
}
