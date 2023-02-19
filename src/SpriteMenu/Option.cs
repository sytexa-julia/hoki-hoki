using System;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SpriteUtilities;

namespace SpriteMenu {
	/// <summary>
	/// An option button in a select menu
	/// </summary>
	public class Option : SpriteButton {
		protected object val;

		#region getset
		public object Value {
			get { return val; }
			set { val=value; }
		}

		#endregion

		public Option(Device device,Sprite sprite,Font font,ButtonTexture tex,Form frm) : base(device,sprite,font,tex,frm) {
			//Set defaults
			val=null;
		}
	}
}