using System;
using System.IO;
using FSound=FmodManaged.FSOUND;

namespace Hoki {
	/// <summary>
	/// Song implementation that uses a single looping sample
	/// </summary>
	public class SimpleSong : Song {
		private IntPtr sample;
		private int channel;

		public SimpleSong(string filename) {
			if (!File.Exists(filename)) throw new FileNotFoundException(filename);
			sample=FSound.Function.Stream.FSOUND_Stream_Open(filename,FSound.Enums.FSOUND_MODES.FSOUND_LOOP_NORMAL,0,0);
			if (sample.Equals(new IntPtr(0))) throw new Exception("Could not load sample");
		}

		#region Song Members

		public override void Play() {
			channel=FSound.Function.Stream.FSOUND_Stream_Play((int)FSound.Enums.FSOUND_MISC_VALUES.FSOUND_FREE,sample);
			onVolumeChange(null,new EventArgs());

			VolumeChange+=new EventHandler(onVolumeChange);
		}

		public override void Stop() {
			FSound.Function.Stream.FSOUND_Stream_Stop(sample);
		}

		#endregion

		private void onVolumeChange(object sender, EventArgs e) {
			FSound.Function.Channels.FSOUND_SetVolume(channel,(int)(volume*2.55f));
		}
	}
}
