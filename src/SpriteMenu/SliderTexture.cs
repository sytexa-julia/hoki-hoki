using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SpriteUtilities;

namespace SpriteMenu {
	/// <summary>
	/// A set of textures to be used by a SpriteSlider
	/// </summary>
	public class SliderTexture {
		protected SpriteTexture
			left,	//Left end of the slider
			right,	//Right end of the slider
			knob,	//Knob that the user controls the slider with
			groove; //Groove that the knob is moved along

		#region getset
		/// <summary>
		/// Left end of the slider
		/// </summary>
		public SpriteTexture Left {
			get { return left; }
		}
		
		/// <summary>
		/// Right end of the slider
		/// </summary>
		public SpriteTexture Right {
			get { return right; }
		}
		
		/// <summary>
		/// Knob that the user controls the slider with
		/// </summary>
		public SpriteTexture Knob {
			get { return knob; }
		}
		
		/// <summary>
		/// Groove that the knob is moved along
		/// </summary>
		public SpriteTexture Groove {
			get { return groove; }
		}
		#endregion

		/// <param name="left">The left end of the slider (two frames)</param>
		/// <param name="right">The right end of the slider (two frames)</param>
		/// <param name="knob">The knob that the user controls the slider with (two frames)</param>
		/// <param name="groove">The groove that the knob is moved along (one frame, stretched)</param>
		public SliderTexture(SpriteTexture left,SpriteTexture right,SpriteTexture knob,SpriteTexture groove) {
			this.left=left;
			this.right=right;
			this.knob=knob;
			this.groove=groove;
		}
	}
}
