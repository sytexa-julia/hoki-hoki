using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace HokiConfig {
	/// <summary>
	/// Hoki Hoki configuration program
	/// </summary>
	public class Config : System.Windows.Forms.Form {
		#region Form designer vars
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private HokiConfig.ControlButton buttonDown;
		private HokiConfig.ControlButton buttonUp;
		private HokiConfig.ControlButton buttonRight;
		private HokiConfig.ControlButton buttonLeft;
		private HokiConfig.ControlButton buttonB;
		private HokiConfig.ControlButton buttonA;
		private System.Windows.Forms.Label labelDown;
		private System.Windows.Forms.Label labelUp;
		private System.Windows.Forms.Label labelRight;
		private System.Windows.Forms.Label labelB;
		private System.Windows.Forms.Label labelLeft;
		private System.Windows.Forms.Label labelA;
		#endregion

		private const string
			aName="A",		//Names of variables in the file
			bName="B",
			leftName="L",
			rightName="R",
			upName="U",
			downName="D",
			startName="S",
			musicName="M",
			volumeName="V",
			fxName="X",
			windowedName="W",
			firstTimeName="F",
			highColorName="T",
			antiAliasName="N";
		private System.Windows.Forms.Label labelStart;
		private HokiConfig.ControlButton buttonStart;

		private ControlButton setButton;

		private string music,volume;

		private string
			fx,
			firstTime;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox highColor;
		private System.Windows.Forms.CheckBox antialias;

		private System.Windows.Forms.CheckBox windowed;

		public Config() {
			InitializeComponent();

			StreamReader reader=new StreamReader("config");
			String line;
			while ((line=reader.ReadLine())!=null) {	//Read each line
				String[] data=line.Split(':');			//Lines are formatted variable:value

				switch (data[0]) {
					//Key values
					case aName:
						buttonA.Key=(Keys)int.Parse(data[1]);
						break;
					case bName:
						buttonB.Key=(Keys)int.Parse(data[1]);
						break;
					case leftName:
						buttonLeft.Key=(Keys)int.Parse(data[1]);
						break;
					case rightName:
						buttonRight.Key=(Keys)int.Parse(data[1]);
						break;
					case upName:
						buttonUp.Key=(Keys)int.Parse(data[1]);
						break;
					case downName:
						buttonDown.Key=(Keys)int.Parse(data[1]);
						break;
					case startName:
						buttonStart.Key=(Keys)int.Parse(data[1]);
						break;
					case musicName:
						music=data[1];
						break;
					case volumeName:
						volume=data[1];
						break;
					case fxName:
						fx=data[1];
						break;
					case firstTimeName:
						firstTime=data[1];
						break;
					case windowedName:
						windowed.Checked=(data[1].Equals("1")?true:false);
						break;
					case highColorName:
						highColor.Checked=(data[1].Equals("1")?true:false);
						break;
					case antiAliasName:
						antialias.Checked=(data[1].Equals("1")?true:false);
						break;
				}
			}

			reader.Close();
		}

		private void onControlButtonClick(object sender, EventArgs e) {
			if (setButton==null) {
				setButton=(ControlButton)sender;
				setButton.Text="<Press a key>";
			}
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			if (setButton!=null) {
				setButton.Key=e.KeyData;
				setButton=null;
				return;
			}
			base.OnKeyDown (e);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			if (setButton!=null) {
				setButton.Key=keyData;
				setButton=null;
				return true;
			}
			return base.ProcessCmdKey (ref msg, keyData);
		}

		private void onOk(object sender, System.EventArgs e) {
			//Save the changes to file
			StreamWriter writer=new StreamWriter("config");
			writer.WriteLine(aName+":"+(int)buttonA.Key);
			writer.WriteLine(bName+":"+(int)buttonB.Key);
			writer.WriteLine(leftName+":"+(int)buttonLeft.Key);
			writer.WriteLine(rightName+":"+(int)buttonRight.Key);
			writer.WriteLine(upName+":"+(int)buttonUp.Key);
			writer.WriteLine(downName+":"+(int)buttonDown.Key);
			writer.WriteLine(startName+":"+(int)buttonStart.Key);
			writer.WriteLine(musicName+":"+music);
			writer.WriteLine(volumeName+":"+volume);
			writer.WriteLine(fxName+":"+fx);
			writer.WriteLine(firstTimeName+":"+firstTime);
			writer.WriteLine(windowedName+":"+(windowed.Checked?"1":"0"));
			writer.WriteLine(antiAliasName+":"+(antialias.Checked?"1":"0"));
			writer.WriteLine(highColorName+":"+(highColor.Checked?"1":"0"));
			writer.Close();

			//Quit
			onCancel(sender,e);
		}

		private void onCancel(object sender, System.EventArgs e) {
			Application.Exit();
		}

		#region windows stuff
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonDown = new HokiConfig.ControlButton();
			this.buttonUp = new HokiConfig.ControlButton();
			this.buttonRight = new HokiConfig.ControlButton();
			this.buttonLeft = new HokiConfig.ControlButton();
			this.buttonB = new HokiConfig.ControlButton();
			this.buttonA = new HokiConfig.ControlButton();
			this.labelDown = new System.Windows.Forms.Label();
			this.labelUp = new System.Windows.Forms.Label();
			this.labelRight = new System.Windows.Forms.Label();
			this.labelB = new System.Windows.Forms.Label();
			this.labelLeft = new System.Windows.Forms.Label();
			this.labelA = new System.Windows.Forms.Label();
			this.labelStart = new System.Windows.Forms.Label();
			this.buttonStart = new HokiConfig.ControlButton();
			this.windowed = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.antialias = new System.Windows.Forms.CheckBox();
			this.highColor = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonOk
			// 
			this.buttonOk.Location = new System.Drawing.Point(92, 216);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(80, 24);
			this.buttonOk.TabIndex = 1;
			this.buttonOk.Text = "Ok";
			this.buttonOk.Click += new System.EventHandler(this.onOk);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(180, 216);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(80, 24);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.onCancel);
			// 
			// buttonDown
			// 
			this.buttonDown.Key = System.Windows.Forms.Keys.None;
			this.buttonDown.Location = new System.Drawing.Point(226, 48);
			this.buttonDown.Name = "buttonDown";
			this.buttonDown.Size = new System.Drawing.Size(104, 24);
			this.buttonDown.TabIndex = 23;
			this.buttonDown.TabStop = false;
			this.buttonDown.Text = "Down";
			this.buttonDown.Click += new System.EventHandler(this.onControlButtonClick);
			// 
			// buttonUp
			// 
			this.buttonUp.Key = System.Windows.Forms.Keys.None;
			this.buttonUp.Location = new System.Drawing.Point(226, 16);
			this.buttonUp.Name = "buttonUp";
			this.buttonUp.Size = new System.Drawing.Size(104, 24);
			this.buttonUp.TabIndex = 22;
			this.buttonUp.TabStop = false;
			this.buttonUp.Text = "Up";
			this.buttonUp.Click += new System.EventHandler(this.onControlButtonClick);
			// 
			// buttonRight
			// 
			this.buttonRight.Key = System.Windows.Forms.Keys.None;
			this.buttonRight.Location = new System.Drawing.Point(66, 112);
			this.buttonRight.Name = "buttonRight";
			this.buttonRight.Size = new System.Drawing.Size(104, 24);
			this.buttonRight.TabIndex = 21;
			this.buttonRight.TabStop = false;
			this.buttonRight.Text = "Right";
			this.buttonRight.Click += new System.EventHandler(this.onControlButtonClick);
			// 
			// buttonLeft
			// 
			this.buttonLeft.Key = System.Windows.Forms.Keys.None;
			this.buttonLeft.Location = new System.Drawing.Point(66, 80);
			this.buttonLeft.Name = "buttonLeft";
			this.buttonLeft.Size = new System.Drawing.Size(104, 24);
			this.buttonLeft.TabIndex = 20;
			this.buttonLeft.TabStop = false;
			this.buttonLeft.Text = "Left";
			this.buttonLeft.Click += new System.EventHandler(this.onControlButtonClick);
			// 
			// buttonB
			// 
			this.buttonB.Key = System.Windows.Forms.Keys.None;
			this.buttonB.Location = new System.Drawing.Point(66, 48);
			this.buttonB.Name = "buttonB";
			this.buttonB.Size = new System.Drawing.Size(104, 24);
			this.buttonB.TabIndex = 19;
			this.buttonB.TabStop = false;
			this.buttonB.Text = "B";
			this.buttonB.Click += new System.EventHandler(this.onControlButtonClick);
			// 
			// buttonA
			// 
			this.buttonA.Key = System.Windows.Forms.Keys.None;
			this.buttonA.Location = new System.Drawing.Point(66, 16);
			this.buttonA.Name = "buttonA";
			this.buttonA.Size = new System.Drawing.Size(104, 24);
			this.buttonA.TabIndex = 18;
			this.buttonA.TabStop = false;
			this.buttonA.Text = "A";
			this.buttonA.Click += new System.EventHandler(this.onControlButtonClick);
			// 
			// labelDown
			// 
			this.labelDown.Location = new System.Drawing.Point(178, 53);
			this.labelDown.Name = "labelDown";
			this.labelDown.Size = new System.Drawing.Size(34, 16);
			this.labelDown.TabIndex = 17;
			this.labelDown.Text = "Down:";
			this.labelDown.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelUp
			// 
			this.labelUp.Location = new System.Drawing.Point(178, 21);
			this.labelUp.Name = "labelUp";
			this.labelUp.Size = new System.Drawing.Size(34, 16);
			this.labelUp.TabIndex = 16;
			this.labelUp.Text = "Up:";
			this.labelUp.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelRight
			// 
			this.labelRight.Location = new System.Drawing.Point(18, 117);
			this.labelRight.Name = "labelRight";
			this.labelRight.Size = new System.Drawing.Size(40, 16);
			this.labelRight.TabIndex = 15;
			this.labelRight.Text = "Right:";
			this.labelRight.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelB
			// 
			this.labelB.Location = new System.Drawing.Point(2, 53);
			this.labelB.Name = "labelB";
			this.labelB.Size = new System.Drawing.Size(51, 16);
			this.labelB.TabIndex = 14;
			this.labelB.Text = "B Button:";
			this.labelB.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelLeft
			// 
			this.labelLeft.Location = new System.Drawing.Point(26, 85);
			this.labelLeft.Name = "labelLeft";
			this.labelLeft.Size = new System.Drawing.Size(32, 16);
			this.labelLeft.TabIndex = 13;
			this.labelLeft.Text = "Left:";
			this.labelLeft.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelA
			// 
			this.labelA.Location = new System.Drawing.Point(2, 21);
			this.labelA.Name = "labelA";
			this.labelA.Size = new System.Drawing.Size(51, 16);
			this.labelA.TabIndex = 12;
			this.labelA.Text = "A Button:";
			this.labelA.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelStart
			// 
			this.labelStart.Location = new System.Drawing.Point(178, 85);
			this.labelStart.Name = "labelStart";
			this.labelStart.Size = new System.Drawing.Size(34, 16);
			this.labelStart.TabIndex = 24;
			this.labelStart.Text = "Start:";
			this.labelStart.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// buttonStart
			// 
			this.buttonStart.Key = System.Windows.Forms.Keys.None;
			this.buttonStart.Location = new System.Drawing.Point(226, 80);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(104, 24);
			this.buttonStart.TabIndex = 25;
			this.buttonStart.TabStop = false;
			this.buttonStart.Text = "Enter";
			this.buttonStart.Click += new System.EventHandler(this.onControlButtonClick);
			// 
			// windowed
			// 
			this.windowed.Location = new System.Drawing.Point(8, 16);
			this.windowed.Name = "windowed";
			this.windowed.TabIndex = 26;
			this.windowed.Text = "Run Windowed";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.labelA);
			this.groupBox1.Controls.Add(this.labelLeft);
			this.groupBox1.Controls.Add(this.labelB);
			this.groupBox1.Controls.Add(this.labelRight);
			this.groupBox1.Controls.Add(this.buttonA);
			this.groupBox1.Controls.Add(this.buttonB);
			this.groupBox1.Controls.Add(this.buttonLeft);
			this.groupBox1.Controls.Add(this.buttonRight);
			this.groupBox1.Controls.Add(this.buttonStart);
			this.groupBox1.Controls.Add(this.labelStart);
			this.groupBox1.Controls.Add(this.labelUp);
			this.groupBox1.Controls.Add(this.labelDown);
			this.groupBox1.Controls.Add(this.buttonUp);
			this.groupBox1.Controls.Add(this.buttonDown);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(338, 144);
			this.groupBox1.TabIndex = 27;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Controls";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.antialias);
			this.groupBox2.Controls.Add(this.highColor);
			this.groupBox2.Controls.Add(this.windowed);
			this.groupBox2.Location = new System.Drawing.Point(8, 160);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(336, 48);
			this.groupBox2.TabIndex = 28;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Video Options";
			// 
			// antialias
			// 
			this.antialias.Location = new System.Drawing.Point(248, 16);
			this.antialias.Name = "antialias";
			this.antialias.Size = new System.Drawing.Size(81, 24);
			this.antialias.TabIndex = 28;
			this.antialias.Text = "Antialiasing";
			// 
			// highColor
			// 
			this.highColor.Location = new System.Drawing.Point(126, 16);
			this.highColor.Name = "highColor";
			this.highColor.TabIndex = 27;
			this.highColor.Text = "32 Bit Textures";
			// 
			// Config
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(354, 247);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.Name = "Config";
			this.Text = "Hoki Hoki Config";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new Config());
		}
		#endregion
	}
}
