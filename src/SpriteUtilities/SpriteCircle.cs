using System;
using System.Runtime.InteropServices;
using FloatMath;
using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Win32;

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
			PositionColored[] verts=new PositionColored[detail+1];
			float degPerLine=2*FMath.PI/detail;
			for (int i=0;i<detail;i++) {
				float ang=degPerLine*i;
				verts[i]=new PositionColored(FMath.Cos(ang)*radius,FMath.Sin(ang)*radius,1,color);
			}
			verts[verts.Length-1]=verts[0]; //Copy the first point into the last slot to connect back around

            //Create a VB and load the verts into it
            vb = new VertexBuffer(device, PositionColored.StrideSize * (detail + 1), Usage.WriteOnly, PositionColored.Format, Pool.Default);
			vb.Lock(0, 0, LockFlags.None).WriteRange(verts);
			vb.Unlock();
		}

		protected override void deviceDraw(Matrix trans) {
			device.SetTransform(0, trans);
			//device.Transform.World=trans;
			device.SetStreamSource(0,vb,0, PositionColored.StrideSize);
			device.VertexFormat=PositionColored.Format;
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
