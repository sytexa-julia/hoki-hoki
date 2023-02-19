using System;
using SharpDX;
using SharpDX.Direct3D9;

namespace Hoki {
	/// <summary>
	/// A set of shader constant names, and effect, and a technique to pass to BlurWindows
	/// </summary>
	public class BlurConstants {
		public string
			blurTechnique,		//Technique to use for the blur render
			worldConstName,		//Name of the effect constant that world matrix should be passed to
			projConstName,		//Name of the effect constant that projection matrix should be passed to
			blurSizeConstName,	//Name of the effect constant that blur size should be passed to
			widthConstName,		//Name of the effect constant that texture width should be passed to
			heightConstName;
		public Effect
			blurEffect;

		/// <param name="blurEffect">Effect to use when blurring</param>
		/// <param name="blurTechnique">Technique to use when blurring</param>
		/// <param name="worldConstName">Effect constant to pass world matrix to</param>
		/// <param name="projConstName">Effect constant to pass proj matrix to</param>
		/// <param name="blurSizeConstName">Effect constant to pass the desired blur size to</param>
		/// <param name="texSizeConstName">Effect constant to pass texture size to</param>
		public BlurConstants(Effect blurEffect,string blurTechnique,string worldConstName,string projConstName,string blurSizeConstName,string widthConstName,string heightConstName) {
			this.blurEffect=blurEffect;
			this.blurTechnique=blurTechnique;
			this.worldConstName=worldConstName;
			this.projConstName=projConstName;
			this.blurSizeConstName=blurSizeConstName;
			this.widthConstName=widthConstName;
			this.heightConstName=heightConstName;
		}
	}
}
