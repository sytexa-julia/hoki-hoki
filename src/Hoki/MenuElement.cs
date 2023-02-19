using System;
using SharpDX.Direct3D9;
using SpriteUtilities;

namespace Hoki {
	public abstract class MenuElement : SpriteObject {
		public abstract void Select();
		public abstract void Deselect();
		public abstract void Input(Controls control);

		public MenuElement(Device device,SpriteTexture tex) : base(device,tex) {
			; //(nop)
		}
	}
}