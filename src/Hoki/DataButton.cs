using System;
using SharpDX.Direct3D9;
using SpriteUtilities;

namespace Hoki {
	/// <summary>
	/// Summary description for DataButton.
	/// </summary>
	public class DataButton : KeyButton {
		private object data;

		public DataButton (Device device,SpriteTexture leftTex,SpriteTexture middleTex,SpriteTexture rightTex,Sprite sprite,Font font,float width) : base(device,leftTex,middleTex,rightTex,sprite,font,width) {
			; //(nop)
		}

		public object Data {
			get { return data; }
			set { data=value; }
		}
	}
}
