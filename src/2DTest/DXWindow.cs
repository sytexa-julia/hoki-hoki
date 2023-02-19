using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Trans2D;

namespace D2DTest {
	/// <summary>
	/// Test app for the Trans2D library
	/// </summary>
	public class DXWindow : System.Windows.Forms.Form {
		private Device device;
		//private Sprite sprite;
		private PresentParameters pres;

		private VertexBuffer vb;

		#region construct/init
		public DXWindow() {
			InitializeComponent();
			SetStyle(ControlStyles.Opaque | ControlStyles.AllPaintingInWmPaint,true);
		}

		public void Initialize() {
			Device.IsUsingEventHandlers=false;

			//Set up the presentation parameters
			pres=new PresentParameters();
			pres.Windowed=true;
			pres.SwapEffect=SwapEffect.Discard;
			pres.AutoDepthStencilFormat=DepthFormat.D16;

			//Get some hardware caps
			Caps hardware=Manager.GetDeviceCaps(0,DeviceType.Hardware);
			CreateFlags flags=CreateFlags.SoftwareVertexProcessing;	//Default createflags

			//Use hardware vertex processing if available
			if (hardware.DeviceCaps.SupportsHardwareTransformAndLight) flags=CreateFlags.HardwareVertexProcessing;
			
			//Create the device, capture its reset and call the setup method
			device=new Device(0,DeviceType.Hardware,this,flags,pres);
			device.DeviceLost+=new EventHandler(onDeviceLost);
			device.DeviceReset+=new EventHandler(onDeviceReset);
			
			onDeviceReset(this,new EventArgs());

			//Create a sprite
			//sprite=new Sprite(device);

			//Init the timer
			DXUtil.Timer(DirectXTimer.GetElapsedTime);

			//Create things to draw
			initObjects();
		}
		#endregion

		private void initObjects() {
			Vector2[] verts=new Vector2[] {
				new Vector2(0,0),
				new Vector2(50,0),
				new Vector2(0,50)
			};

			CustomVertex.PositionColored[] testVerts=new CustomVertex.PositionColored[3];
			for (int i=0;i<3;i++)
				testVerts[i]=new CustomVertex.PositionColored(verts[i].X,verts[i].Y,1,System.Drawing.Color.White.ToArgb());
			
			vb=new VertexBuffer(typeof(CustomVertex.PositionColored),3,device,Usage.Dynamic|Usage.WriteOnly,CustomVertex.PositionColored.Format,Pool.Default);
			vb.SetData(testVerts,0,LockFlags.None);
		}

		protected override void OnPaint(PaintEventArgs e) {
			int deviceState;
			if (device.CheckCooperativeLevel(out deviceState)) {
				//Begin the scene
				device.Clear(ClearFlags.Target,System.Drawing.Color.Black.ToArgb(),0.0f,0);
				device.BeginScene();

				//poly.Draw();
				device.VertexFormat=CustomVertex.PositionColored.Format;
				device.SetStreamSource(0,vb,0);
				device.DrawPrimitives(PrimitiveType.TriangleList,0,1);
				device.SetStreamSource(0,null,0);

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
			this.Invalidate();
		}

		private void onDeviceLost(object sender, EventArgs e) {
			vb.Dispose();
		}

		private void onDeviceReset(object sender, EventArgs e) {
			setupDevice();
			initObjects();
		}

		/// <summary>
		/// Sets the device after it is created or reset
		/// </summary>
		protected void setupDevice() {
			//Setup renderstates and camera
			Poly.SetupDevice(device);
			Poly.SetupCamera(device,ClientSize.Width,ClientSize.Height);
		}

		#region file/stream handling
		/// <summary>
		/// Returns a stream of an embedded resource
		/// </summary>
		private Stream getStream(string resourcePath) {
			return GetType().Module.Assembly.GetManifestResourceStream(resourcePath);
		}

		/// <summary>
		/// Creates a new Texture from an embedded resource, wrapped in a SizeTexture that contains the image's dimensions
		/// </summary>
		/// <param name="imgStream">Path to </param>
		/// <returns></returns>
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

			//Create the SizeTexture
			Tex tex=new Tex(TextureLoader.FromStream(device,imgStream,width,height,1,0,Format.A8R8G8B8,Pool.Default,Filter.None,Filter.None,0),width,height);

			//Close the stream and return the SizeTexture
			imgStream.Close();
			return tex;
		}

		private void getTextureData(string path,out Stream imgStream,out int width,out int height) {
			//Get the stream
			imgStream=getStream(path);

			//Get the size
			Bitmap bmp=new Bitmap(imgStream);
			width=bmp.Width;
			height=bmp.Height;

			//Remake the stream to use with the TextureLoader
			imgStream.Close();
			imgStream=getStream(path);
		}

		/// <summary>
		/// Gets the contents of an embedded resource
		/// </summary>
		private String readFile(string resourcePath) {
			StreamReader sr=new StreamReader(getStream(resourcePath));
			return sr.ReadToEnd();
		}
		#endregion

		#region windows stuff
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
			this.components = new System.ComponentModel.Container();
			this.Size = new System.Drawing.Size(300,300);
			this.Text = "Trans2D Test";
		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			using (DXWindow dxw=new DXWindow()) {
				dxw.Initialize();
				dxw.Show();
				Application.Run(dxw);
			}
		}
		#endregion
	}
}