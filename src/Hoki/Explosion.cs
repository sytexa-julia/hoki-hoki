using System;
using SharpDX;
using SharpDX.Direct3D9;
using DS=SharpDX.DirectSound;
using SpriteUtilities;
using FloatMath;

namespace Hoki {
	/// <summary>
	/// Explosion graphic effect
	/// </summary>
	public class Explosion : SpriteObject, Updateable {
		private Particle[] particles;
		private int liveParticles;	//Number of particles still alive

		public event EventHandler Die;
		
		public static DS.SecondarySoundBuffer[] Sounds;
		private static int currentSound;
		private bool spark;

		public Explosion(Device device,SpriteTexture tex,int numParticles,bool spark) : base(device,null) {
			this.spark=spark;
			particles=new Particle[numParticles];
			liveParticles=numParticles;

			for (int i=0;i<numParticles;i++) {
				if (spark)
						particles[i]=new SparkParticle(device,tex);
				else	particles[i]=new ExplosionParticle(device,tex);
				particles[i].ID=i;
				particles[i].Die+=new EventHandler(onParticleDie);
				Add((SpriteObject)particles[i]);
			}

			if (!spark && Game.FXOn) Sounds[(currentSound++)%Sounds.Length].Play(0,DS.PlayFlags.None);
		}
		
		private void onParticleDie(object sender, EventArgs e) {
			particles[((Particle)sender).ID]=null;
			Remove((TransformedObject)sender);
			if (--liveParticles==0) Die(this,new EventArgs());
		}

		#region Updateable Members
		public void Update(float elapsedTime) {
			foreach (Updateable p in particles) if (p!=null) p.Update(elapsedTime);	//Update every non-null particle
		}
		#endregion
		
		/// <summary>
		/// One particle in the explosion effect
		/// </summary>
		private class ExplosionParticle : SpriteObject, Updateable, Particle {
			private const float
				minInitAlpha=80,	//Minimum initial alpha value for particles
				maxInitAlpha=120,	//Maximum initial alpha value for particles
				initScatter=10,		//Radius within which to scatter the particles initially
				alphaDecay=200,		//Rate of particle alpha decay, units/sec
				scaleRate=1,		//Rate of particle scale increase, percentage points/sec
				maxRotSpeed=1;		//Maximum rate of particle rotation, rad/sec

			private float rSpeed;
			private int id;

			public event EventHandler Die;

			public ExplosionParticle(Device device,SpriteTexture tex) : base(device,tex) {
				//Center the origin
				origin.X=tex.Width/2;
				origin.Y=tex.Height/2;

				//Randomize
				X=Rand.NextFloat(-initScatter,initScatter);
				Y=Rand.NextFloat(-initScatter,initScatter);
				Rotation=Rand.NextFloat(2*FMath.PI);
				Alpha=Rand.NextFloat(minInitAlpha,maxInitAlpha);
				rSpeed=Rand.NextFloat(-maxRotSpeed,maxRotSpeed);
			}

			public int ID {
				get { return id; }
				set { id=value; }
			}

			#region Updateable Members

			public void Update(float elapsedTime) {
				XScale+=scaleRate*elapsedTime;
				YScale=XScale;
				Rotation+=rSpeed*elapsedTime;
				Alpha-=alphaDecay*elapsedTime;

				if (Alpha<=0) Die(this,new EventArgs());
			}

			#endregion

			protected override void deviceDraw(Matrix trans) {
				var oldSourceBlend = device.GetRenderState<Blend>(RenderState.SourceBlend);
				var oldDestBlend = device.GetRenderState<Blend>(RenderState.DestinationBlend);
				bool changeSource=(oldSourceBlend != Blend.SourceAlpha);
				bool changeDest=(oldDestBlend!=Blend.One);

				//Switch to an additive blend
				if (changeSource) device.SetRenderState(RenderState.SourceBlend,Blend.SourceAlpha);
				if (changeDest) device.SetRenderState(RenderState.DebugMonitorToken, Blend.One);

                base.deviceDraw(trans);

				//Switch back to a regular blend
				if (changeSource) device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
                if (changeDest) device.SetRenderState(RenderState.SourceBlend, Blend.InverseSourceAlpha);
            }
		}
		
		/// <summary>
		/// One particle in the spark effect
		/// </summary>
		private class SparkParticle : SpriteObject, Updateable, Particle {
			private const float
				minInitAlpha=255,	//Minimum initial alpha value for particles
				maxInitAlpha=255,	//Maximum initial alpha value for particles
				initScatter=8,		//Radius within which to scatter the particles initially
				alphaDecay=600,		//Rate of particle alpha decay, units/sec
				scaleRate=-0.1f,	//Rate of particle scale increase, percentage points/sec
				maxRotSpeed=3;		//Maximum rate of particle rotation, rad/sec

			private float rSpeed;
			private int id;

			private Vector2 move;

			public event EventHandler Die;

			public SparkParticle(Device device,SpriteTexture tex) : base(device,tex) {
				//Center the origin
				origin.X=tex.Width/2;
				origin.Y=tex.Height/2;

				//Randomize
				X=Rand.NextFloat(-initScatter,initScatter);
				Y=Rand.NextFloat(-initScatter,initScatter);
				Rotation=Rand.NextFloat(2*FMath.PI);
				Alpha=Rand.NextFloat(minInitAlpha,maxInitAlpha);
				rSpeed=Rand.NextFloat(-maxRotSpeed,maxRotSpeed);
				
				move=new Vector2(X,Y);
				move=((50+Rand.NextFloat(50))/move.Length())*move;
			}

			#region Updateable Members

			public int ID {
				get { return id; }
				set { id=value; }
			}

			public void Update(float elapsedTime) {
				XScale+=scaleRate*elapsedTime;
				YScale=XScale;
				Rotation+=rSpeed*elapsedTime;
				Alpha-=alphaDecay*elapsedTime;
				Position+=move*elapsedTime;

				if (Alpha<=0) Die(this,new EventArgs());
			}

			#endregion
		}

		private interface Particle {
			int ID { get; set; }
			event EventHandler Die;
		}
	}
}