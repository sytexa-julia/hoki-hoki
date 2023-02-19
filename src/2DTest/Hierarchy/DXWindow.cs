using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Trans2D;

namespace Hierarchy {
	/// <summary>
	/// Sample application for the Trans2D library
	/// </summary>
	/// <remarks>The render loop is adapted from the one given in Tom Miller's blog on May 5</remarks>
	public abstract class Form : System.Windows.Forms.Form {
		private int width,height;

		private Device device;
		private PresentParameters pres;

		public Form() {
			//Required for Windows Form Designer support
			InitializeComponent();
			
			width=ClientSize.Width;
			height=ClientSize.Height;
		}

		/// <summary>
		/// Initializes graphics
		/// </summary>
		/// <remarks>This is a very basic initialization procedure.
		/// In a full application you should look through the hardware caps more extensively.</remarks>
		virtual public void Init() {
			//Create presentation parameters
			pres=new PresentParameters();
			pres.Windowed=true;
			pres.SwapEffect=SwapEffect.Discard;
			pres.AutoDepthStencilFormat=DepthFormat.D16;

			//Get some hardware caps
			Caps hardware=Manager.GetDeviceCaps(0,DeviceType.Hardware);
			CreateFlags flags=CreateFlags.SoftwareVertexProcessing;	//Default createflags

			//Use hardware vertex processing if available
			if (hardware.DeviceCaps.SupportsHardwareTransformAndLight) flags=CreateFlags.HardwareVertexProcessing;

			//Turn off event handlers
			Device.IsUsingEventHandlers=false;

			//Create the device and set it up
			device=new Device(0,DeviceType.Hardware,this,flags,pres);
			device.DeviceReset+=new EventHandler(onDeviceReset);
			onDeviceReset(null,null);
		}

		/// <summary>
		/// Sets up the device
		/// </summary>
		virtual protected void onDeviceReset(object sender,EventArgs args) {
			Poly.SetupDevice(device);				//Set device render states
			Poly.SetupCamera(device,width,height);	//Setup an orthogonal view matrix
		}

		/// <summary>
		/// Runs every loop
		/// </summary>
		virtual public void MainLoop() {
			int deviceState;
			if (device.CheckCooperativeLevel(out deviceState)) {
				//Begin the scene
				device.Clear(ClearFlags.Target,System.Drawing.Color.Black.ToArgb(),0.0f,0);
				device.BeginScene();

				render();

				//End the scene
				device.EndScene();
				try {	//This may be unnecessary, device.CheckCooperativeLevel may be sufficient to ensure this won't happen
					device.Present();
				} catch (DeviceLostException) {}
			} else {
				switch (deviceState) {
					case (int)ResultCode.DeviceLost:
						System.Threading.Thread.Sleep(100);	//Nothing we can do, wait a bit
						break;
					case (int)ResultCode.DeviceNotReset:
						device.Reset(pres);					//Reset the device
						break;
				}
			}
		}

		protected abstract void Render() {
		}

		#region Resources
		/// <summary>
		/// Returns a stream of an embedded resource
		/// </summary>
		private Stream getStream(string resourcePath) {
			return GetType().Module.Assembly.GetManifestResourceStream(resourcePath);
		}

		/// <summary>
		/// Creates a new Texture from an embedded resource, wrapped in a Tex that contains the image's dimensions
		/// </summary>
		/// <param name="imgStream">Path to the resource</param>
		public Tex loadTex(String imagePath) {
			//Get the stream
			Stream imgStream=getStream(imagePath);

			//Get the size
			Bitmap bmp=new Bitmap(imgStream);
			int width=bmp.Width;
			int height=bmp.Height;

			//Remake the stream to use with the TextureLoader
			imgStream.Close();
			imgStream=getStream(imagePath);

			//Create the texture
			Tex tex=new Tex(TextureLoader.FromStream(device,imgStream,width,height,1,0,Format.A8R8G8B8,Pool.Default,Filter.None,Filter.None,0),width,height);

			//Close the stream and return the SizeTexture
			imgStream.Close();
			return tex;
		}
		#endregion

		#region Render loop
		/// <summary>
		/// Starts the main loop
		/// </summary>
		virtual public void Begin() {
			Application.Idle+=new EventHandler(onAppIdle);
			Application.Run(this);
		}

		/// <summary>
		/// Render while the application is idle
		/// </summary>
		virtual protected void onAppIdle(object sender, EventArgs e) {
			while (AppStillIdle) MainLoop();
		}

		/// <summary>
		/// Whether the application is still idling
		/// </summary>
		virtual protected bool AppStillIdle {
			get {
				Message msg;
				return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
			}
		}
		#endregion

		#region Native members
		[StructLayout(LayoutKind.Sequential)]
		public struct Message {
			public IntPtr hWnd;
			public int msg;
			public IntPtr wParam;
			public IntPtr lParam;
			public uint time;
			public System.Drawing.Point p;
		}

		[System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
		[DllImport("User32.dll", CharSet=CharSet.Auto)]
		public static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);
		#endregion

		#region Windows Form stuff
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

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
			// 
			// Form
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Name = "Form1";
			this.Text = "Hierarchy Demo";

		}
		#endregion
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			using (Form form=new Form()) {
				form.Show();			//Make sure the window is displayed before trying to create a device or anything
				form.Init();			//Initialize graphics stuff
				form.Begin();			//Start the application and render loop
			}
		}
	}
}
