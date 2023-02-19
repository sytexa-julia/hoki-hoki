using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SpriteUtilities;
using FloatMath;

namespace HokiEdit {
	/// <summary>
	/// Summary description for SpriteArrow.
	/// </summary>
	public class SpriteArrow : SpriteObject {
		private SpriteLine main,left,right;

		/// <summary>
		/// Thickness of the lines used to draw the arrow
		/// </summary>
		public float Thickness {
			get { return main.Thickness; }
			set { main.Thickness=left.Thickness=right.Thickness=value; }
		}

		public override System.Drawing.Color Tint {
			get { return base.Tint; }
			set { base.Tint=main.Tint=left.Tint=right.Tint=value; }
		}

		public SpriteArrow(Device device,Vector2 p1,Vector2 p2) : base(device,null) {
			main=new SpriteLine(device);
			left=new SpriteLine(device);
			right=new SpriteLine(device);
			Add(main);
			Add(left);
			Add(right);

			SetPos(p1,p2);
		}

		public void SetPos(Vector2 p1,Vector2 p2) {
			float angle=FMath.Angle(p2,p1);

			main.X=p1.X;
			main.Y=p1.Y;
			main.Angle=angle;
			main.Length=FMath.Distance(p2,p1);

			left.X=p2.X;
			left.Y=p2.Y;
			left.Angle=angle+FMath.PI*3/4;
			left.Length=10;

			right.X=p2.X;
			right.Y=p2.Y;
			right.Angle=angle-FMath.PI*3/4;
			right.Length=10;
		}
	}
}
