using System;
using System.Windows.Forms;

namespace HokiConfig {
	/// <summary>
	/// Summary description for ControlButton.
	/// </summary>
	public class ControlButton : Button {
		private Keys key;

		public Keys Key {
			get { return key; }
			set {
				key=value;
				Text=key.ToString();

				//Clean up Text
				int keyCode=(int)key;
				if (keyCode<58 && keyCode>46) Text=""+Text[1];	//D0-D9
				else {
					int commaIndex=Text.IndexOf(',');
					if (commaIndex>=0) Text=Text.Substring(commaIndex+2);
				}

				//Special cases
				switch (Text) {
					case "Oemcomma":
						Text=",";
						break;
					case "OemPeriod":
						Text=".";
						break;
					case "OemQuestion":
						Text="/";
						break;
					case "OemQuotes":
						Text="\"";
						break;
					case "OemOpenBrackets":
						Text="[";
						break;
					case "OemCloseBrackets":
						Text="]";
						break;
					case "OemPipe":
					case "OemBackslash":
						Text="\\";
						break;
					case "OemMinus":
						Text="-";
						break;
					case "Oemplus":
						Text="=";
						break;
					case "Oemtilde":
						Text="~";
						break;
					case "OemSemicolon":
						Text=";";
						break;
					case "Back":
						Text="Backspace";
						break;
					case "Prior":
						Text="Page Up";
						break;
					case "Next":
						Text="Page Down";
						break;
					case "Divide":
						Text="/";
						break;
					case "Multiply":
						Text="*";
						break;
					case "Subtract":
						Text="-";
						break;
					case "Add":
						Text="+";
						break;
					case "Decimal":
						Text=".";
						break;
					case "Capital":
						Text="Caps Lock";
						break;
				}				
			}
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			return; //Do NOT process key events
		}
	}
}
