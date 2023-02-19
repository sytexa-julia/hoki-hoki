using System;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SpriteUtilities;

namespace SpriteMenu {
	/// <summary>
	/// The knob in a SpriteSelect
	/// </summary>
	internal class SpriteKnob : SpriteButton {
		public SpriteKnob(Device device,Form frm,SpriteTexture tex) : base(device,null,null,new ButtonTexture(tex,null,null),frm) {			
		}

		#region frame/state overrides
		protected override void onPress(object sender, MouseEventArgs e) {
			base.onPress(sender,e);
			setButtonFrame(1);
		}

		protected override void onRelease(object sender, MouseEventArgs e) {
			base.onRelease(sender,e);
			setButtonFrame(0);
		}

		protected override void onReleaseOutside(object sender, MouseEventArgs e) {
			base.onReleaseOutside(sender,e);
			setButtonFrame(0);
		}

		protected override void onRollOver(object sender, MouseEventArgs e) {
			base.onRollOver(sender,e);
			if (down)	setButtonFrame(1);
			else		setButtonFrame(0);
		}

		protected override void onRollOut(object sender, MouseEventArgs e) {
			base.onRollOut(sender,e);
			if (down)	setButtonFrame(1);
			else		setButtonFrame(0);
		}

		#endregion
	}
}
