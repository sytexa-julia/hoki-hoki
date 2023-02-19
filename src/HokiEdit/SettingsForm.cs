using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace HokiEdit
{
	/// <summary>
	/// Summary description for SettingsForm.
	/// </summary>
	public class SettingsForm : System.Windows.Forms.Form
	{
		public System.Windows.Forms.Button OkButton;
		new public System.Windows.Forms.Button CancelButton;
		private System.Windows.Forms.Label AuthorLabel;
		private System.Windows.Forms.Label GhostLabel;
		public System.Windows.Forms.TextBox Author;
		public System.Windows.Forms.TextBox Ghost;
		private System.Windows.Forms.Label themeLabel;
		public System.Windows.Forms.TextBox Theme;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SettingsForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
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
		private void InitializeComponent()
		{
			this.OkButton = new System.Windows.Forms.Button();
			this.CancelButton = new System.Windows.Forms.Button();
			this.AuthorLabel = new System.Windows.Forms.Label();
			this.GhostLabel = new System.Windows.Forms.Label();
			this.Author = new System.Windows.Forms.TextBox();
			this.Ghost = new System.Windows.Forms.TextBox();
			this.themeLabel = new System.Windows.Forms.Label();
			this.Theme = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// OkButton
			// 
			this.OkButton.Location = new System.Drawing.Point(80, 152);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(64, 24);
			this.OkButton.TabIndex = 0;
			this.OkButton.Text = "Ok";
			// 
			// CancelButton
			// 
			this.CancelButton.Location = new System.Drawing.Point(152, 152);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(64, 24);
			this.CancelButton.TabIndex = 1;
			this.CancelButton.Text = "Cancel";
			// 
			// AuthorLabel
			// 
			this.AuthorLabel.Location = new System.Drawing.Point(8, 8);
			this.AuthorLabel.Name = "AuthorLabel";
			this.AuthorLabel.Size = new System.Drawing.Size(48, 16);
			this.AuthorLabel.TabIndex = 2;
			this.AuthorLabel.Text = "Author:";
			// 
			// GhostLabel
			// 
			this.GhostLabel.Location = new System.Drawing.Point(8, 56);
			this.GhostLabel.Name = "GhostLabel";
			this.GhostLabel.Size = new System.Drawing.Size(72, 16);
			this.GhostLabel.TabIndex = 3;
			this.GhostLabel.Text = "Ghost:";
			// 
			// Author
			// 
			this.Author.Location = new System.Drawing.Point(8, 24);
			this.Author.Name = "Author";
			this.Author.Size = new System.Drawing.Size(272, 20);
			this.Author.TabIndex = 4;
			this.Author.Text = "Anonymous";
			// 
			// Ghost
			// 
			this.Ghost.Location = new System.Drawing.Point(8, 72);
			this.Ghost.Name = "Ghost";
			this.Ghost.Size = new System.Drawing.Size(272, 20);
			this.Ghost.TabIndex = 5;
			this.Ghost.Text = "<None>";
			// 
			// themeLabel
			// 
			this.themeLabel.Location = new System.Drawing.Point(8, 104);
			this.themeLabel.Name = "themeLabel";
			this.themeLabel.Size = new System.Drawing.Size(80, 16);
			this.themeLabel.TabIndex = 3;
			this.themeLabel.Text = "Theme";
			// 
			// Theme
			// 
			this.Theme.Location = new System.Drawing.Point(8, 120);
			this.Theme.Name = "Theme";
			this.Theme.Size = new System.Drawing.Size(272, 20);
			this.Theme.TabIndex = 5;
			this.Theme.Text = "default.theme";
			// 
			// SettingsForm
			// 
			this.AcceptButton = this.OkButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 181);
			this.ControlBox = false;
			this.Controls.Add(this.Ghost);
			this.Controls.Add(this.Author);
			this.Controls.Add(this.GhostLabel);
			this.Controls.Add(this.AuthorLabel);
			this.Controls.Add(this.CancelButton);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.themeLabel);
			this.Controls.Add(this.Theme);
			this.Name = "SettingsForm";
			this.Text = "Settings";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
