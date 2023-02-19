using System;
using System.Collections;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using D3D=Microsoft.DirectX.Direct3D;

namespace SpriteUtilities {
	/// <summary>
	/// SpriteObject derivative that draws text
	/// </summary>
	public class SpriteText : SpriteObject{
		protected Sprite sprite;			//Sprite to draw with
		protected D3D.Font font;			//Font to draw with
		protected Rectangle rect;			//Clipping dimensions for the text
		protected string text;				//text to draw
		protected DrawTextFormat format;	//Format for drawing
		protected int width,height;			//Space the text will take up

		#region getset
		/// <summary>
		/// Text to draw
		/// </summary>
		public string Text {
			get { return text; }
			set { text=value; }
		}

		/// <summary>
		/// Font used for drawing
		/// </summary>
		public D3D.Font DrawFont {
			get { return font; }
			set { font=value; }
		}

		/// <summary>
		/// Sprite used for drawing
		/// </summary>
		public Sprite DrawSprite {
			get { return sprite; }
			set { sprite=value; }

		}

		/// <summary>
		/// Format to pass to DrawText
		/// </summary>
		public DrawTextFormat Format {
			get { return format; }
			set { format=value; }
		}

		/// <summary>
		/// Clipping width
		/// </summary>
		public override float Width {
			get { return (float)rect.Width; }
			set { rect.Width=(int)value; }
		}
		
		/// <summary>
		/// Clipping height
		/// </summary>
		public override float Height {
			get { return (float)rect.Height; }
			set { rect.Height=(int)value; }
		}

		#endregion

		/// <param name="width">Clipping width</param>
		/// <param name="height">Clipping height</param>
		public SpriteText(Device device,Sprite sprite,D3D.Font font,int width,int height) : base(device,null) {
			//Store sprite and font
			this.sprite=sprite;
			this.font=font;

			//Create a clipping rectangle
			rect=new Rectangle(0,0,width,height);

			//Set defaults
			text="";
			format=DrawTextFormat.None;
			Tint=Color.Black;
		}

		public Rectangle Area() {
			return font.MeasureString(sprite,text,DrawTextFormat.None,Color.White);
		}

		/// <summary>
		/// Transform the device and sprite and draw the text to the screen
		/// </summary>
		/// <param name="trans">Absolute transformation matrix</param>
		protected override void deviceDraw(Matrix trans) {
			sprite.Begin(SpriteFlags.AlphaBlend);
			device.Transform.World=Matrix.Identity;
			sprite.Transform=trans;
			font.DrawText(sprite,text,rect,format,Color.FromArgb((int)alpha,color).ToArgb());
			sprite.End();
		}
	}
}
