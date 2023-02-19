using System;
using FloatMath;
using FSound=FmodManaged.FSOUND;

namespace Hoki {
	public abstract class Song {
		protected static float volume;	//Volume from 0 to 100

		public static event EventHandler VolumeChange;

		static Song() {
			VolumeChange+=new EventHandler(VCHandler);
		}

		public static float Volume {
			get { return volume; }
			set {
				volume=value;
				FSound.Function.Channels.FSOUND_SetVolume((int)FSound.Enums.FSOUND_MISC_VALUES.FSOUND_ALL,(int)(FMath.Clamp(value,0,100)*2.55f));
			}
		}

		public abstract void Play();
		public abstract void Stop();

		private static void VCHandler(object sender, EventArgs e) {
			//This is just here so there aren't any dumb NREs.
		}
	}
}