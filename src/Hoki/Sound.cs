using System;
using System.IO;
using Microsoft.DirectX.DirectSound;
using SpriteUtilities;
using FloatMath;

namespace Hoki {
	/// <summary>
	/// Represents a 
	/// </summary>
	public class Sound : Updateable {
		private static Device device;
		private static event EventHandler updateVolume;
		private static float master=1;

		private SecondaryBuffer sound;
		private int fade,numBytes;
		private float volume=100,fadeRate=60;

		static Sound() {
			updateVolume+=new EventHandler(updateHook);
		}

		public Sound(string sourceFile) {
			//Create a description and enable volume control
			numBytes=(int)(new FileInfo(sourceFile).Length);
			BufferDescription desc=new BufferDescription();
			desc.ControlVolume=true;
			desc.ControlPositionNotify=true;

			//Create the sound
			sound=new SecondaryBuffer(sourceFile,desc,device);

			//Hook the volume update event
			updateVolume+=new EventHandler(onUpdateVolume);
		}

		~Sound() {
			Dispose();
		}

		public void Dispose() {
			sound.Dispose();
		}

		/// <summary>
		/// Initializes the device, enabling instances of Sound to play
		/// </summary>
		public static void InitSound(Game game) {
			device=new Device();
			device.SetCooperativeLevel(game,CooperativeLevel.Normal);
		}

		/// <summary>
		/// Volume scalar (0 to 1)
		/// </summary>
		public static float MasterVolume {
			get { return master; }
			set {
				master=value;
				updateVolume(null,new EventArgs());
			}
		}

		public void RandomPosition() {
			sound.SetCurrentPosition(Rand.Next(numBytes-319));	//I don't know why, but 319 is min. we can subtract without having problems
		}

		#region play control
		public void Play() {
			sound.Play(0,BufferPlayFlags.Default);
		}

		public void Loop() {
			sound.Play(0,BufferPlayFlags.Looping);
		}

		public void Stop() {
			sound.Stop();
		}
		#endregion

		public void Fade(int target) {
			fade=target;
		}

		private void setVolume(int volume) {
			sound.Volume=(int)((50-volume*master/2)*-100);
		}

		private void onUpdateVolume(object sender, EventArgs e) {
			setVolume((int)volume);
		}

		#region getset
		/// <summary>
		/// Volume on a scale from 0 to 100
		/// </summary>
		public int Volume {
			get { return sound.Volume; }
			set {
				volume=fade=value;
				setVolume(value);
			}
		}

		public float FadeRate {
			get { return fadeRate; }
			set { fadeRate=value; }
		}
		#endregion

		#region Updateable Members
		public void Update(float elapsedTime) {
			if (FMath.Abs(fade-volume)<fadeRate*elapsedTime) volume=fade;
			else volume+=fadeRate*elapsedTime*FMath.Sgn(fade-volume);

			setVolume((int)volume);
		}
		#endregion

		private static void updateHook(object sender, EventArgs e) {

		}
	}
}
