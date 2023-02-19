using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SpriteUtilities;

namespace Hoki {
	/// <summary>
	/// Summary description for Planet.
	/// </summary>
	public class Planet : TransformedObject, Updateable {
		private VertexBuffer vb;
		private CustomVertex.PositionColoredTextured[] verts;
		private Vector2 shiftRate;
		private Texture tex;
		private Game owner;
		private float radius;

		static int id;
		private int myId;

		public Planet(Device device,Texture tex,SpriteTexture sphereTex,Game owner,int detail,float radius,float texrep,Vector2 shiftRate) : base(device) {
			this.tex=tex;
			this.shiftRate=shiftRate;
			this.owner=owner;
			this.radius=radius;

			myId=id++;

			verts=new CustomVertex.PositionColoredTextured[detail+2];
			int white=System.Drawing.Color.White.ToArgb();

			verts[0]=new CustomVertex.PositionColoredTextured(0,0,0,white,0.5f,0.5f);
			for (int i=1;i<=detail+1;i++) {
				double angle=((double)i)/detail*2*Math.PI;
				Vector2 v=new Vector2((float)Math.Cos(angle),(float)Math.Sin(angle));
				verts[i]=new CustomVertex.PositionColoredTextured(radius*v.X,radius*v.Y,0,white,(1f+v.X*texrep)/2f,(1f+v.Y*texrep)/2f);
			}

			vb=new VertexBuffer(typeof(CustomVertex.PositionColoredTextured),verts.Length,device,Usage.Dynamic|Usage.WriteOnly,CustomVertex.PositionColoredTextured.Format,Pool.Default);
			vb.SetData(verts,0,LockFlags.None);

			SpriteObject sphere=new SpriteObject(device,sphereTex);
			sphere.Width=sphere.Height=2*radius;
			sphere.X=-radius;
			sphere.Y=-radius;
			Add(sphere);
		}

		#region Updateable Members
		public void Update(float elapsedTime) {
			//HACK: control visibility for speed. This is really bad code, but it's faster than using localToGlobal.
			Vector2 screenPos=parent.Parent.Position+position;
			Visible=(screenPos.X+radius>0 && screenPos.X-radius<owner.ClientSize.Width && screenPos.Y+radius>0 && screenPos.Y-radius<owner.ClientSize.Height);

			//Slide the texture along if visible.
			if (Visible) {
				for (int i=0;i<verts.Length;i++) {
					verts[i].Tu+=shiftRate.X*elapsedTime;
					verts[i].Tv+=shiftRate.Y*elapsedTime;
				}
				vb.SetData(verts,0,LockFlags.None);
			}
		}
		#endregion

		protected override void deviceDraw(Matrix trans) {
			//Set pipeline state
			device.Transform.World=trans;
			device.SetStreamSource(0,vb,0);
			device.SetTexture(0,tex);
			device.VertexFormat=CustomVertex.PositionColoredTextured.Format;

			device.DrawPrimitives(PrimitiveType.TriangleFan,0,verts.Length-2);
		}
	}
}