using System;
using SpriteUtilities;

namespace SpriteMenu {
	/// <summary>
	/// Holds several textures to be passed to SpriteWindow
	/// </summary>
	public class WindowTexture {
		public SpriteTexture
			top,
			bottom,
			left,
			right,
			tlc,
			trc,
			blc,
			brc,
			background;

		/// <param name="top">The top bar of the window (the header text is verticall centered in this image)</param>
		/// <param name="bottom">The bottom bar of the window</param>
		/// <param name="left">The left bar of the window</param>
		/// <param name="right">The right bar of the window</param>
		/// <param name="tlc">The top left corner</param>
		/// <param name="trc">The top right corner</param>
		/// <param name="blc">The bottom left corner</param>
		/// <param name="brc">The bottom right corner</param>
		/// <param name="background">The middle background</param>
		public WindowTexture(SpriteTexture top,SpriteTexture bottom,SpriteTexture left,SpriteTexture right,SpriteTexture tlc,SpriteTexture trc,SpriteTexture blc,SpriteTexture brc,SpriteTexture background) {
			this.top=top;
			this.bottom=bottom;
			this.left=left;
			this.right=right;
			this.tlc=tlc;
			this.trc=trc;
			this.blc=blc;
			this.brc=brc;
			this.background=background;
		}
	}
} 
