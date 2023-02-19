using System;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SpriteUtilities;

namespace SpriteMenu {
	/// <summary>
	/// A SpriteTexture with text offset data
	/// </summary>
	public class ButtonTexture {
		protected SpriteTexture
			left,	//Left end
			middle,	//Middle (stretched)
			right;	//Right

		#region getset
		/// <summary>
		/// Left end of the button
		/// </summary>
		public SpriteTexture Left {
			get { return left; }
		}

		/// <summary>
		/// Middle of the button (stretched horizontally)
		/// </summary>
		public SpriteTexture Middle {
			get { return middle; }
		}

		/// <summary>
		/// Right end of the button
		/// </summary>
		public SpriteTexture Right {
			get { return right; }
		}
		#endregion

		/// <summary>
		/// A set of three textures to pass to a SpriteButton. Each texture should have three frames for default, hover, and pressed states.
		/// </summary>
		/// <param name="left">The left end of the button</param>
		/// <param name="middle">The scaled middle section of the button</param>
		/// <param name="right">The right end of the button</param>
		public ButtonTexture(SpriteTexture left,SpriteTexture middle,SpriteTexture right) {
			//Store textures
			this.left=left;
			this.right=right;
			this.middle=middle;
		}
	}
}