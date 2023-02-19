using System;
using SharpDX;
using SharpDX.Direct3D9;
using SpriteUtilities;

namespace Hoki {
	/// <summary>
	/// Summary description for AALayer.
	/// </summary>
	public class AASpriteObject : SpriteObject {

		private bool antiAlias;

		public AASpriteObject(Device device,SpriteTexture tex) : base(device,tex) {}

		public override void Draw(Matrix parentMatrix, Vector2 parentShift) {
			bool oldAntiAlias=device.GetRenderState<bool>(RenderState.MultisampleAntialias);
			bool change=(antiAlias!=oldAntiAlias);
			if (change) device.SetRenderState(RenderState.MultisampleAntialias,antiAlias);
			base.Draw (parentMatrix, parentShift);
			if (change) device.SetRenderState(RenderState.MultisampleAntialias, oldAntiAlias);//device.RenderState.MultiSampleAntiAlias=device.RenderState.MultiSampleAntiAlias;
        }

		public bool AntiAlias {
			get { return antiAlias; }
			set { antiAlias=value; }
		}
	}
}