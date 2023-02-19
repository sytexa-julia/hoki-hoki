using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SpriteUtilities;

namespace Hoki {
	/// <summary>
	/// Summary description for AALayer.
	/// </summary>
	public class AASpriteObject : SpriteObject {

		private bool antiAlias;

		public AASpriteObject(Device device,SpriteTexture tex) : base(device,tex) {}

		public override void Draw(Matrix parentMatrix, Vector2 parentShift) {
			bool oldAntiAlias=device.RenderState.MultiSampleAntiAlias;
			bool change=(antiAlias!=oldAntiAlias);
			if (change) device.RenderState.MultiSampleAntiAlias=antiAlias;
			base.Draw (parentMatrix, parentShift);
			if (change) device.RenderState.MultiSampleAntiAlias=device.RenderState.MultiSampleAntiAlias;
		}

		public bool AntiAlias {
			get { return antiAlias; }
			set { antiAlias=value; }
		}
	}
}