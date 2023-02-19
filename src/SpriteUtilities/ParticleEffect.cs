using SharpDX;
using SharpDX.Direct3D9;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace SpriteUtilities {
	/// <summary>
	/// Visual effect that controls numerous point-sprite particles
	/// </summary>
	public abstract class ParticleEffect : EffectObject,Updateable {
		private VertexBuffer vb;
		private Texture tex;
		protected PositionColoredTextured[] verts;
		protected Particle[] particles;

		public ParticleEffect(Device device,Texture tex,int particleCount) : base(device) {
			//Make particle and vertex arrays
			particles=new Particle[particleCount];
			verts=new PositionColoredTextured[particleCount];

			//Initialize the particles
			for (int i=0;i<particleCount;i++) generateParticle(i);


			//Make a vertex buffer
            vb = new VertexBuffer(device, PositionColoredTextured.StrideSize * particleCount, Usage.Dynamic | Usage.WriteOnly, PositionColoredTextured.Format, Pool.Default);
            //vb =new VertexBuffer(typeof(PositionColoredTextured),particleCount,device,Usage.Dynamic | Usage.WriteOnly,CustomVertex.PositionColoredTextured.Format,Pool.Default);

			//Store the texture reference
			this.tex=tex;
		}

		/// <summary>
		/// Creates a new particle at the given index in the array
		/// </summary>
		protected abstract void generateParticle(int index);

		protected override void deviceDraw(Matrix trans) {
			//Update the vertex buffer
			vb.Lock(0, 0, LockFlags.Discard).WriteRange(verts);
			vb.Unlock();

			base.deviceDraw(trans);
			if (tex!=null) { //If a texture has been provided, draw
				//Set pipeline state
				device.SetTransform(TransformState.World, trans);
				
				device.SetStreamSource(0,vb,0, PositionColoredTextured.StrideSize);
				device.SetTexture(0,tex);

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

		virtual protected void drawPrimitives() {
			device.DrawPrimitives(PrimitiveType.PointList,0,particles.Length);
		}

		protected override void sendFXC(FXConstant fxc,Matrix trans) {
			if (current!=null) {
				switch (fxc.Type) {
					case ConstType.Color:
						current.SetValue(fxc.Name,new Vector4(color.R,color.G,color.B,color.A));
						break;
					case ConstType.Texture:
						if (tex != null) current.SetTexture(fxc.Name, tex); //Can't set with a null tex, but it doesn't matter because the effect routine won't be run
						break;
					case ConstType.WorldMatrix:
						current.SetValue(fxc.Name,trans);
						break;
				}
			}
		}

		#region Update
		public void Update(float elapsedTime) {
			for (int i=0;i<particles.Length;i++) UpdateParticle(i,elapsedTime);
		}

		/// <summary>
		/// Updates an individual particle at index i
		/// </summary>
		/// <param name="elapsedTime">Time since the last update</param>
		virtual protected void UpdateParticle(int i,float elapsedTime) {
			//Update the particle
			particles[i].Update(elapsedTime);

			//Update the vertices
			verts[i].X=particles[i].Position.X;
			verts[i].Y=particles[i].Position.Y;
			verts[i].Color=System.Drawing.Color.FromArgb((int)particles[i].Color.X,(int)particles[i].Color.Y,(int)particles[i].Color.Z,(int)particles[i].Alpha).ToArgb();
		}

		#endregion
	}
}
