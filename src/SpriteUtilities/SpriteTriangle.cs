using System;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace SpriteUtilities {
	/// <summary>
	/// A textured, screen-aligned triangle
	/// </summary>
	public class SpriteTriangle : TransformedObject {
		protected CustomVertex.PositionColoredTextured[] verts;	//List of vertices to put in the buffer
		protected VertexBuffer vb;	//Textured triangle
		protected Texture tex;		//Texture to apply
		protected bool reset=false;	//Whether the buffer needs to be reset

		public SpriteTriangle(Device device, Texture tex) : base(device) {
			//Store the texture
			this.tex=tex;

			//Create the vertices/vertex buffer
			verts=new CustomVertex.PositionColoredTextured[3];
			for (int i=0;i<verts.Length;i++)
				verts[i]=new CustomVertex.PositionColoredTextured(0,0,1,Color.White.ToArgb(),0,0);
			
			vb=new VertexBuffer(typeof(CustomVertex.PositionColoredTextured),3,device,Usage.Dynamic | Usage.WriteOnly,CustomVertex.PositionColoredTextured.Format,Pool.Default);
	
			resetBuffer();
		}


		#region vertex control
		/// <summary>
		/// Sets the position of a vertex
		/// </summary>
		/// <param name="index">The vertex's index, 0 to 2</param>
		/// <param name="position">The vertex's new position</param>
		public void SetPosition(int index,Vector2 position) {
			verts[index].X=position.X;
			verts[index].Y=position.Y;
			reset=true;
		}

		/// <summary>
		/// Gets the position of a vertex
		/// </summary>
		/// <param name="index">The vertex's index, 0 to 2</param>
		public Vector2 GetPosition(int index) {
			return new Vector2(verts[index].X,verts[index].Y);
		}

		/// <summary>
		/// Sets the texture coordinates of a vertex
		/// </summary>
		/// <param name="index">The vertex's index, 0 to 2</param>
		/// <param name="coords">The vertex's new texture coordinates</param>
		public void SetTexCoords(int index,Vector2 coords) {
			verts[index].Tu=coords.X;
			verts[index].Tv=coords.Y;
			reset=true;
		}

		/// <summary>
		/// Gets the texture coordinates of a vertex
		/// </summary>
		/// <param name="index">The vertex's index, 0 to 2</param>
		public Vector2 GetTexCoords(int index) {
			return new Vector2(verts[index].Tu,verts[index].Tv);
		}
		#endregion

		#region buffer
		protected override void checkUpdates() {
			base.checkUpdates ();
			if (colorNeedsUpdate)	setColor(Color.FromArgb((int)alpha,color));	//Apply the color to the vertices
			if (reset)				resetBuffer();
		}

		protected void setColor(Color color) {
			verts[0].Color=verts[1].Color=verts[2].Color=color.ToArgb();
		}

		protected void resetBuffer() {
			vb.SetData(verts,0,LockFlags.None);
		}
		#endregion

		#region drawing
		public override void Draw(Matrix parentMatrix, Vector2 parentShift) {
			base.Draw (parentMatrix, parentShift);
			reset=false;	//By the end of the call, the buffer must be up-to-date
		}

		protected override void deviceDraw(Matrix trans) {
			device.SetTexture(0,tex);
			device.VertexFormat=CustomVertex.PositionColoredTextured.Format;
			device.SetStreamSource(0,vb,0);
			device.Transform.World=trans;
			device.DrawPrimitives(PrimitiveType.TriangleList,0,1);
		}

		#endregion
	}
}