using System;
using System.Collections;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SpriteUtilities;

namespace SpriteMenu {
	/// <summary>
	/// A selection menu derived from SpriteButton
	/// </summary>
	public class SpriteSelect : SpriteButton {
		protected object selectValue;	//The value of the currently selected option
		protected ArrayList
			options,		//List of options to create when the menu is opened
			optionSprites;	//List of option sprites that exist when the menu is open (used to determine what to remove from children on close)

		protected ButtonTexture optionTex;	//SpriteTexture to be used for the Options
		
		private bool
			open;			//Whether the menu is currently open

		//
		// TODO: FIX THE EVENTS
		//
//		public event EventHandler Open;
//		public event EventHandler Close;
//		public event ChangeHandler Change;

		#region getset
		/// <summary>
		/// The value of the option currently selected
		/// </summary>
		public object Value {
			get { return selectValue; }
		}

		/// <summary>
		/// The name of the option currently selected
		/// </summary>
		public string Selected {
			get { return Text; }
		}
		#endregion

		#region constructor
		/// <param name="device">A Direct3D device</param>
		/// <param name="tex">A button texture (3+ frames) for the menu</param>
		/// <param name="optionTex">A button texture (3+ frames) for the options</param>
		/// <param name="frm">The Windows Form that holds this SpriteSelect</param>
		public SpriteSelect(Device device,Sprite sprite,Font font,ButtonTexture tex,ButtonTexture optionTex,Form frm) : base(device,sprite,font,tex,frm) {
			//Set defaults
			selectValue=null;
			open=false;

			//Store option texture
			this.optionTex=optionTex;

			//Create lists
			options=new ArrayList();
			optionSprites=new ArrayList();
		}
		#endregion

		#region options
		/// <summary>
		/// Adds an option to the list
		/// </summary>
		/// <param name="name">The name that will appear on the list</param>
		/// <param name="val">The value attributed to the option</param>
		public void AddOption(string name,object val) {
			options.Add(new OptionSet(name,val));
		}
		
		/// <summary>
		/// Removes the first option on the list with a given name
		/// </summary>
		public void RemoveOption(string name) {
			for (int i=0;i<options.Count;i++) {
				if (((OptionSet)options[i]).name==name) {
					options.RemoveAt(i);
					break;
				}
			}
		}

		/// <summary>
		/// Removes the first option on the list with a given value.
		/// </summary>
		public void RemoveOption(object val) {
			for (int i=0;i<options.Count;i++) {
				if (((OptionSet)options[i]).val==val) {
					options.RemoveAt(i);
					break;
				}
			}
		}
		#endregion

		protected void openList() {
			if (open) return;	//The list is already open
			open=true;

			//Cycle through the OptionSets and create a new Option for each
			Option o;
			OptionSet s;
			for (int i=0;i<options.Count;i++) {
				s=(OptionSet)options[i];		//Get the OptionSet
				o=new Option(device,text.DrawSprite,text.DrawFont,optionTex,frm);	//Construct a new Option

				//Set option data
				o.Text=s.name;
				o.Value=s.val;

				//Position the option
				o.Y=Height+i*o.Height;
				o.Width=Width;

				//Add the option to the draw and Options lists
				Add(o);
				optionSprites.Add(o);

				//Capture the option's press event
				o.Press+=new MouseEventHandler(onOptionRelease);
			}
		}

		/// <summary>
		/// Closes the option list
		/// </summary>
		protected void closeList() {
			if (!open) return; //The list is already closed
			for (int i=0;i<optionSprites.Count;i++) {
				Remove((SpriteObject)optionSprites[i]);
			}
			optionSprites.Clear();
			open=false;
		}

		/// <summary>
		/// Sets the list's selected option
		/// </summary>
		/// <param name="option">The option's index</param>
		public void setOption(int option) {
			OptionSet o=(OptionSet)options[option];
			Text=o.name;
			selectValue=o.val;
		}

		/// <summary>
		/// Sets the selected option and closes the list
		/// </summary>
		protected void setOption(Option o) {
			//See if there is any change
			if (Text!=o.Text || this.selectValue!=o.Value) {
				//Store the changes
				Text=o.Text;
				selectValue=o.Value;
			}
			closeList();
		}

		/// <summary>
		/// Handles the Windows Form's MouseDown event
		/// </summary>
		protected override void onMouseDown(object sender, MouseEventArgs e) {
			base.onMouseDown(sender,e);		//Do button actions first
			if (open) closeList();			//If the list is open, close it
			if (hover && !open)	openList();	//If the list is closed and the mouse is hovering, open it
		}


		/// <summary>
		/// Used to store options in the list
		/// </summary>
		protected struct OptionSet {
			public string name;
			public object val;

			public OptionSet(string name,object val) {
				this.name=name;
				this.val=val;
			}
		}

		private void onOptionRelease(object sender, MouseEventArgs e) {
			setOption((Option)sender);
			closeList();
		}

		#region Lockable Members
		public override void Lock() {
			base.Lock ();
			if (open) closeList();	//Make sure the list is closed
		}

		#endregion
	}

	#region event stuff
	public delegate void ChangeHandler(object sender,ChangeEventArgs e);

	/// <summary>
	/// Used to pass old and new values for a event of type ChangeHandler
	/// </summary>
	public class ChangeEventArgs : EventArgs {
		private object oldValue,newValue;
		public object OldValue {
			get { return oldValue; }
		}

		public object NewValue {
			get { return newValue; }
		}

		public ChangeEventArgs(object oldValue,object newValue) {
			this.oldValue=oldValue;
			this.newValue=newValue;
		}
	}
	#endregion
}