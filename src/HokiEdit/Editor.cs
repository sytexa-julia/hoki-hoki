using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using D3D=Microsoft.DirectX.Direct3D;
using SpriteUtilities;
using SpriteMenu;
using FloatMath;

namespace HokiEdit {
	/// <summary>
	/// Summary description for Editor.
	/// </summary>
	public class Editor : System.Windows.Forms.Form {
		#region dx
		private Device device;
		private Sprite sprite;
		private Matrix
			world,
			view,
			proj;
		#endregion

		#region window
		private int
			width,	//Size of form client area
			height;
		private Cursor
			regCursor,
			scrollCursor,
			noCursor;
		private SettingsForm
			settingsForm;
		#endregion

		private const float
			nodeSpace=50;	//Space between nodes when drawing
		
		#region workspace control
		private Tool
			tool,		//Currently selected tool
			toolSave;	//Saves the tool while None is selected (so that windows can be used without accidentally manipulating the map)
		private ArrayList
			update,		//Updateable objects
			nodes,
			lines,
			polys,
			pads,
			springs,
			launchers,
			sprites,
			elements;	//All map elements
		private bool
			down,		//Whether the mouse is down
			dragging,	//Whether the selection is being dragged
			linking,	//Whether a line is currently being linked from node to node
			boxing,		//Whether a selection box is being created
			scrolling,	//Whether the workspace is being scrolled
			shift,		//Whether shift key is down
			ctrl,		//Whether the control key is down
			launcherDown;	//Whether previewLauncherFrom is set
		private Vector2
			lastMouse,	//Position the mouse was in at the time of the last mousemove
			drawLast;	//Last workspace coordinate of the mouse while drawing
		private Node
			drawNode,	//Last node drawn by the pencil
			fromNode,	//Node a line is being connected from
			triNode0,	//First two nodes clicked on to make a poly
			triNode1;
		private float
			drawDist,	//Distance the mouse has moved since a node was last placed
			xScroll,
			yScroll;
		private int
			xScrollDir,
			yScrollDir,
			xScrollCenter,
			yScrollCenter,
			xScrollStart,
			yScrollStart,
			triIndex;
		private MouseEventArgs
			lastMouseEvent;
		private Node
			lastNode;
		private String
			clipboard="",		//Copied elements as a compiled map string
			undelete="",		//State saved before a delete
			author="Anonymous",
			ghost="",
			theme="";
		private bool
			drawing,			//Whether the user is presently drawing
			canUndelete;		//Whether it is possible to undelete at this time
		private int
			deleteX,			//Where the last deletion occurred
			deleteY;
		#endregion

		#region sprites/textures
		private SpriteObject
			//Layers
			root,			//Base layer, contains all others
			toolbar,		//Tool selection buttons
			gridLayer,		//8x8 grid showing node snap positions
			lowPreview,		//Preview of pads (beneath workspace)
			workspace,		//Map elements
			padLayer,		//Sublayer of workspace, pads
			nodeLayer,		//Sublayer of workspace, nodes
			lineLayer,		//Sublayer of workspace, lines
			springLayer,	//Sublayer of workspace, springs
			polyLayer,		//Sublayer of workspace, polys
			launcherLayer,	//Sublayer of workspace, launchers
			spriteLayer,	//Sublayer of workspace, sprites
			drawLayer,		//Sublayer of workspace, pencil lines
			preview,		//Preview of lines and polys connecting nodes
			selector,		//Selection box layer
			ui,				//User interface (windows, buttons, etc)
			cursor,			//Cursor
			//Other objects
			toolbarBG;		//Background of toolbar layer
		private Texture
			triTex;
		private SizeTexture
			skin;
		private SpriteTexture
			selectorIconTex,//Button images
			nodeIconTex,
			lineIconTex,
			polyIconTex,
			padIconTex,
			springIconTex,
			launcherIconTex,
			pencilIconTex,
			spriteIconTex,
			toolbarBGTex,	//Background of toolbar area
			nodeTex,		//Actual nodes in workspace
			springTex,		//Springs in workspace
			launcherTex,	//Launchers in workspace
			spriteTex;		//SpriteMarkers in workspace
		private ButtonTexture
			buttonTex,
			menuButtonTex;
		private SliderTexture
			sliderTex;
		private WindowTexture
			windowTex;
		private D3D.Font
			labelFont,
			textFont;
		private SpriteButton
			selectorButton,		//Toolbar buttons
			nodeButton,
			lineButton,
			polyButton,
			padButton,
			springButton,
			launcherButton,
			pencilButton,
			spriteButton,
			aboutOkButton,
			launcherOkButton;
		private SpriteWindow
			aboutWin,
			launcherWin;
		private SpriteSlider
			frequencySlider,	//These are for Launchers
			offsetSlider;
		private SpriteText
			toolText,
			frequencyText,		//For launchers
			offsetText;
		private SpriteBox
			selectBox;
		private SpriteLine
			toolBackground,		//Sort-of-a-hack background for the tool text
			previewLine;
		private SpriteTriangle
			previewTri;
		private Pad
			previewPad;
		private Spring
			previewSpring;
		private Launcher
			previewLauncherFrom,
			previewLauncherTo,
			editLauncher;
		private SpriteMarker
			previewMarker;
		private SpriteArrow
			launcherArrow;
		#endregion

		#region form
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.MenuItem helpMenu;
		private System.Windows.Forms.MenuItem aboutCommand;
		private System.Windows.Forms.MenuItem undeleteCommand;
		private System.Windows.Forms.MenuItem exitCommand;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.OpenFileDialog openDialog;
		private System.Windows.Forms.SaveFileDialog fileDialog;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem fileMenu;
		private System.Windows.Forms.MenuItem newCommand;
		private System.Windows.Forms.MenuItem saveCommand;
		private System.Windows.Forms.MenuItem saveAsCommand;
		private System.Windows.Forms.MenuItem openCommand;
		private System.Windows.Forms.MenuItem editMenu;
		private System.Windows.Forms.MenuItem copyCommand;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem settingsCommand;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem vertFlipCommand;
		private System.Windows.Forms.MenuItem horzFlipCommand;
		private System.Windows.Forms.MenuItem pasteCommand;
		#endregion

		#region construct/init
		public Editor() {
			InitializeComponent();

			//Prevent the form drawing sequence from covering up the DX area
			SetStyle(ControlStyles.Opaque|ControlStyles.AllPaintingInWmPaint,true);

			//Store cursors
			string path="HokiEdit.cursors.";
			regCursor=Cursors.Arrow;
			scrollCursor=Cursors.SizeAll;
			noCursor=new Cursor(getStream(path+"empty.cur"));
			this.Cursor=noCursor;

			//Put client size in more convenient variables
			width=ClientSize.Width;
			height=ClientSize.Height;

			//Set default tool
			tool=Tool.Select;

			//Set default state
			down=false;
			boxing=false;
			dragging=false;

			//Cannot undelete yet
			setUndelete(false);

			//Create last mouse vector
			lastMouse=new Vector2();

			//Make arraylists
			update=new ArrayList();
			nodes=new ArrayList();
			lines=new ArrayList();
			polys=new ArrayList();
			pads=new ArrayList();
			springs=new ArrayList();
			launchers=new ArrayList();
			sprites=new ArrayList();
			elements=new ArrayList();

			//Set up the save file dialog
			fileDialog.Filter="map files (*.map)|*.map";
			fileDialog.InitialDirectory=Application.StartupPath;
			fileDialog.CreatePrompt=false;		//Prompts get fucked up. Skip them.
			fileDialog.OverwritePrompt=false;
			fileDialog.FileOk+=new CancelEventHandler(onFileOk);

			//Set up the open file dialog
			openDialog.Filter="map files (*.map)|*.map";
			openDialog.InitialDirectory=Application.StartupPath;
			openDialog.FileOk+=new CancelEventHandler(onOpenFile);

			//Capture menu events
			newCommand.Click+=new EventHandler(onNew);
			saveCommand.Click+=new EventHandler(onSave);
			saveAsCommand.Click+=new EventHandler(onSaveAs);
			openCommand.Click+=new EventHandler(onOpen);
			copyCommand.Click+=new EventHandler(onCopy);
			pasteCommand.Click+=new EventHandler(onPaste);
			aboutCommand.Click+=new EventHandler(onAbout);
			undeleteCommand.Click+=new EventHandler(onUndelete);
			exitCommand.Click+=new EventHandler(onExit);
			settingsCommand.Click+=new EventHandler(onSettings);
			vertFlipCommand.Click+=new EventHandler(onVertFlip);
			horzFlipCommand.Click+=new EventHandler(onHorzFlip);

			//Make a settings window and capture its events
			settingsForm=new SettingsForm();
			settingsForm.OkButton.Click+=new EventHandler(OkButton_Click);
			settingsForm.CancelButton.Click+=new EventHandler(CancelButton_Click);

			//Center the map
			center(0,0);
		}

		public void InitializeGraphics() {
			//Turn off event handling
			Device.IsUsingEventHandlers=false;

			//Set up the presentation parameters
			PresentParameters pParams=new PresentParameters();
			pParams.SwapEffect=SwapEffect.Discard;
			pParams.Windowed=true;
			pParams.BackBufferCount=1;

			//Get some hardware caps
			Caps Hardware=Manager.GetDeviceCaps(0,DeviceType.Hardware);
			Caps localCaps=Hardware;
			CreateFlags flags=CreateFlags.SoftwareVertexProcessing;	//Default createflags

			//Use Hardware vertex processing if available
			if (Hardware.DeviceCaps.SupportsHardwareTransformAndLight) flags=CreateFlags.HardwareVertexProcessing;

			//Create the device
			device=new Device(0,DeviceType.Hardware,this,flags,pParams);

			//Use point filtering by default
			device.SamplerState[0].MinFilter=TextureFilter.Point;
			device.SamplerState[0].MagFilter=TextureFilter.Point;

			//Basic renderstates
			device.RenderState.Lighting=false;
			device.RenderState.CullMode=Cull.None;

			//Blending states
			if (Hardware.TextureOperationCaps.SupportsModulate) {
				device.TextureState[0].ColorOperation=TextureOperation.Modulate;	//Required for vertex color blending
				device.TextureState[0].AlphaOperation=TextureOperation.Modulate;	//Required for pixel and vertex alpha blending
			}
			device.TextureState[0].ColorArgument1=TextureArgument.TextureColor;		//Required for vertex color and alpha blending
			device.TextureState[0].ColorArgument2=TextureArgument.Diffuse;			//Required for vertex color blending
			device.TextureState[0].AlphaArgument1=TextureArgument.TextureColor;		//Required for pixel alpha blending
			device.TextureState[0].AlphaArgument2=TextureArgument.Diffuse;			//Required for vertex alpha blending

			if (Hardware.SourceBlendCaps.SupportsSourceAlpha)
				device.RenderState.SourceBlend=Blend.SourceAlpha;					//Required for alpha blending
			if (Hardware.DestinationBlendCaps.SupportsInverseSourceAlpha)
				device.RenderState.DestinationBlend=Blend.InvSourceAlpha;			//Required for alpha blending
			device.RenderState.AlphaBlendEnable=true;								//Required for alpha blending
			device.RenderState.AlphaTestEnable=false;

			//Other Device states
			device.VertexFormat=CustomVertex.PositionColoredTextured.Format;

			//Set up matrices
			world=Matrix.Identity;
			view=Matrix.Identity;
			proj=Matrix.OrthoOffCenterLH(0.0f,width,height,0.0f,0.0f,1.0f);

			//Copy them to the device
			device.Transform.World=world;
			device.Transform.View=view;
			device.Transform.Projection=proj;

			//Create a sprite using the device
			sprite=new Sprite(device);

			//Make fonts
			labelFont=new D3D.Font(device,new System.Drawing.Font("Lucida Console",12));
			textFont=new D3D.Font(device,new System.Drawing.Font("Lucida Console",10));

			//Load textures
			string basePath="HokiEdit.textures.";
			skin=loadSizeTexture(basePath+"skin.png");	//Master skin - all textures are somewhere in this image

			//Workspace textures
			nodeTex=loadTexture(skin,basePath+"workspace.node.dat");
			springTex=loadTexture(skin,basePath+"workspace.spring.dat");
			launcherTex=loadTexture(skin,basePath+"workspace.launcher.dat");
			spriteTex=loadTexture(skin,basePath+"workspace.sprite.dat");

			//Menu textures - window
			string path=basePath+"menu.window.";
			windowTex=new WindowTexture(
				loadTexture(skin,path+"top.dat"),
				loadTexture(skin,path+"bottom.dat"),
				loadTexture(skin,path+"left.dat"),
				loadTexture(skin,path+"right.dat"),
				loadTexture(skin,path+"tlc.dat"),
				loadTexture(skin,path+"trc.dat"),
				loadTexture(skin,path+"blc.dat"),
				loadTexture(skin,path+"brc.dat"),
				loadTexture(skin,path+"background.dat")
			);

			//Menu slider
			path=basePath+"menu.slider.";
			sliderTex=new SliderTexture(
				loadTexture(skin,path+"end.dat"),
				loadTexture(skin,path+"end.dat"),
				loadTexture(skin,path+"knob.dat"),
				loadTexture(skin,path+"groove.dat")
			);

			//Menu button
			path=basePath+"menu.button.";
			menuButtonTex=new ButtonTexture(
				loadTexture(skin,path+"left.dat"),
				loadTexture(skin,path+"middle.dat"),
				loadTexture(skin,path+"right.dat")
			);

			//Button icon textures
			path=basePath+"toolbar.";
			selectorIconTex=loadTexture(skin,path+"selector.dat");
			pencilIconTex=loadTexture(skin,path+"pencil.dat");
			nodeIconTex=loadTexture(skin,path+"node.dat");
			lineIconTex=loadTexture(skin,path+"line.dat");
			polyIconTex=loadTexture(skin,path+"poly.dat");
			padIconTex=loadTexture(skin,path+"pad.dat");
			springIconTex=loadTexture(skin,path+"spring.dat");
			launcherIconTex=loadTexture(skin,path+"launcher.dat");
			spriteIconTex=loadTexture(skin,path+"sprite.dat");
			toolbarBGTex=loadTexture(skin,path+"toolbar.dat");

			//Button texture
			path=basePath+"button.";
			buttonTex=new ButtonTexture(null,loadTexture(skin,path+"tool.dat"),null);	//No ends, button does not stretch

			//Triangle texture
			triTex=TextureLoader.FromStream(device,getStream("HokiEdit.textures.workspace.tri.bmp"),0,0,1,Usage.Dynamic,Format.A8R8G8B8,Pool.Default,Filter.None,Filter.None,0);

			//Make layers
			root=new SpriteObject(device,null);			//All drawn objects are children of this layer
			toolbar=new SpriteObject(device,null);		//Toolbar buttons
			lowPreview=new SpriteObject(device,null);	//Pad previews
			workspace=new SpriteObject(device,null);	//Map elements
			drawLayer=new SpriteObject(device,null);	//Pencil lines
			polyLayer=new SpriteObject(device,null);	//Polygons
			padLayer=new SpriteObject(device,null);		//Pads
			nodeLayer=new SpriteObject(device,null);	//Nodes
			lineLayer=new SpriteObject(device,null);	//Lines
			springLayer=new SpriteObject(device,null);	//Springs
			launcherLayer=new SpriteObject(device,null);//Launchers
			spriteLayer=new SpriteObject(device,null);	//Sprites
			preview=new SpriteObject(device,null);		//Line/poly previews
			selector=new SpriteObject(device,null);		//Selection box
			ui=new SpriteObject(device,null);			//Windows

			SpriteTexture cursorTex=loadTexture(skin,basePath+"cursor.dat");

			cursor=new SpriteObject(device,cursorTex);	//Cursor image

			//Configure layers
			workspace.X=32;						//Keep right of toolbar
			preview.X=lowPreview.X=workspace.X;	//Align with workspace
			cursor.Origin=new Vector2(8,8);		//Mouse button is at the center of the image

			//Make a grid layer
			gridLayer=new SpriteObject(device,null);
			SpriteLine gridLine;
			for (int i=(int)workspace.X;i<width;i+=8) {
				gridLine=new SpriteLine(device);
				gridLine.X=i;
				gridLine.Height=height;
				gridLine.Tint=System.Drawing.Color.LightGray;
				gridLayer.Add(gridLine);
			}
			for (int i=(int)workspace.Y;i<height;i+=8) {
				gridLine=new SpriteLine(device);
				gridLine.X=workspace.X;
				gridLine.Y=i;
				gridLine.Width=width;
				gridLine.Tint=System.Drawing.Color.LightGray;
				gridLayer.Add(gridLine);
			}

			//Add all workspace layers
			workspace.Add(polyLayer);
			workspace.Add(padLayer);
			workspace.Add(drawLayer);
			workspace.Add(springLayer);
			workspace.Add(launcherLayer);
			workspace.Add(spriteLayer);
			workspace.Add(nodeLayer);
			workspace.Add(lineLayer);

			//Put layers on the root
			root.Add(gridLayer);
			root.Add(lowPreview);
			root.Add(workspace);
			root.Add(preview);
			root.Add(toolbar);
			root.Add(selector);
			root.Add(ui);
			root.Add(cursor);

			//Make toolbar background
			toolbarBG=new SpriteObject(device,toolbarBGTex);
			toolbarBG.Height=height;

			//Make toolbar buttons (identical bases + loaded image)
			selectorButton=new SpriteButton(device,sprite,null,buttonTex,this);
			selectorButton.Add(new SpriteObject(device,selectorIconTex));

			pencilButton=new SpriteButton(device,sprite,null,buttonTex,this);
			pencilButton.Add(new SpriteObject(device,pencilIconTex));
			pencilButton.Y=selectorButton.Height;

			nodeButton=new SpriteButton(device,sprite,null,buttonTex,this);
			nodeButton.Add(new SpriteObject(device,nodeIconTex));
			nodeButton.Y=pencilButton.Y+pencilButton.Height;

			lineButton=new SpriteButton(device,sprite,null,buttonTex,this);
			lineButton.Add(new SpriteObject(device,lineIconTex));
			lineButton.Y=nodeButton.Y+nodeButton.Height;

			polyButton=new SpriteButton(device,sprite,null,buttonTex,this);
			polyButton.Add(new SpriteObject(device,polyIconTex));
			polyButton.Y=lineButton.Y+lineButton.Height;

			padButton=new SpriteButton(device,sprite,null,buttonTex,this);
			padButton.Add(new SpriteObject(device,padIconTex));
			padButton.Y=polyButton.Y+polyButton.Height;

			springButton=new SpriteButton(device,sprite,null,buttonTex,this);
			springButton.Add(new SpriteObject(device,springIconTex));
			springButton.Y=padButton.Y+padButton.Height;

			launcherButton=new SpriteButton(device,sprite,null,buttonTex,this);
			launcherButton.Add(new SpriteObject(device,launcherIconTex));
			launcherButton.Y=springButton.Y+springButton.Height;
			
			spriteButton=new SpriteButton(device,sprite,null,buttonTex,this);
			spriteButton.Add(new SpriteObject(device,spriteIconTex));
			spriteButton.Y=launcherButton.Y+launcherButton.Height;

			//Add the buttons to the toolbar layer
			toolbar.Add(toolbarBG);
			toolbar.Add(selectorButton);
			toolbar.Add(pencilButton);
			toolbar.Add(nodeButton);
			toolbar.Add(lineButton);
			toolbar.Add(polyButton);
			toolbar.Add(padButton);
			toolbar.Add(springButton);
			toolbar.Add(launcherButton);
			toolbar.Add(spriteButton);

			//Capture toolbar button events
			selectorButton.Press+=new MouseEventHandler(selectorButton_Press);
			selectorButton.RollOver+=new MouseEventHandler(selectorButton_RollOver);
			nodeButton.Press+=new MouseEventHandler(nodeButton_Press);
			nodeButton.RollOver+=new MouseEventHandler(nodeButton_RollOver);
			lineButton.Press+=new MouseEventHandler(lineButton_Press);
			lineButton.RollOver+=new MouseEventHandler(lineButton_RollOver);
			polyButton.Press+=new MouseEventHandler(polyButton_Press);
			polyButton.RollOver+=new MouseEventHandler(polyButton_RollOver);
			padButton.Press+=new MouseEventHandler(padButton_Press);
			padButton.RollOver+=new MouseEventHandler(padButton_RollOver);
			springButton.Press+=new MouseEventHandler(springButton_Press);
			springButton.RollOver+=new MouseEventHandler(springButton_RollOver);
			launcherButton.Press+=new MouseEventHandler(launcherButton_Press);
			launcherButton.RollOver+=new MouseEventHandler(launcherButton_RollOver);
			pencilButton.Press+=new MouseEventHandler(pencilButton_Press);
			pencilButton.RollOver+=new MouseEventHandler(pencilButton_RollOver);
			spriteButton.Press+=new MouseEventHandler(spriteButton_Press);
			spriteButton.RollOver+=new MouseEventHandler(spriteButton_RollOver);

			//Make tool info textbox
			toolText=new SpriteText(device,sprite,textFont,width,15);
			toolText.X=4;
			toolText.Y=height-toolText.Height;
			
			//Make a background for the tool info
			toolBackground=new SpriteLine(device);
			toolBackground.X=0;
			toolBackground.Y=toolText.Y+toolText.Height/2-2;
			toolBackground.Width=width;
			toolBackground.Thickness=toolText.Height+4;
			toolBackground.Tint=System.Drawing.Color.White;

			//Add both
			toolbar.Add(toolBackground);
			toolbar.Add(toolText);
			
			//Create a selection box
			selectBox=new SpriteBox(device);
			selectBox.Visible=false;
			selector.Add(selectBox);

			//Create a preview line
			previewLine=new SpriteLine(device);
			preview.Add(previewLine);

			//Create a preview pad
			previewPad=new Pad(device,sprite,labelFont,PadType.End);
			previewPad.Visible=false;
			lowPreview.Add(previewPad);
			
			//Create a preview triangle
			previewTri=new SpriteTriangle(device,triTex);
			previewTri.Visible=false;
			previewTri.Alpha=100;
			lowPreview.Add(previewTri);

			//Create a preview spring
			previewSpring=new Spring(device,springTex);
			previewSpring.X=previewSpring.Y=float.NegativeInfinity;
			lowPreview.Add(previewSpring);

			//Create preview launchers and the directional arrow
			previewLauncherFrom=new Launcher(device,launcherTex);
			previewLauncherFrom.X=previewLauncherFrom.Y=float.NegativeInfinity;
			lowPreview.Add(previewLauncherFrom);

			previewLauncherTo=new Launcher(device,launcherTex);
			previewLauncherTo.X=previewLauncherTo.Y=float.NegativeInfinity;
			lowPreview.Add(previewLauncherTo);

			previewMarker=new SpriteMarker(device,spriteTex,sprite,textFont,this);
			previewMarker.X=previewMarker.Y=float.NegativeInfinity;
			lowPreview.Add(previewMarker);

			launcherArrow=new SpriteArrow(device,Vector2.Empty,Vector2.Empty);
			launcherArrow.Visible=false;
			launcherArrow.Thickness=2;
			lowPreview.Add(launcherArrow);

			//Create about window
			aboutWin=new SpriteWindow(device,sprite,textFont,windowTex);
			aboutWin.Text="About";
			aboutWin.Width=250;
			aboutWin.Height=175;
			aboutWin.X=(int)(width-aboutWin.Width)/2;
			aboutWin.Y=(int)(height-aboutWin.Height)/2;
			aboutWin.Visible=false;

			//Create the window's text
			SpriteText text=new SpriteText(device,sprite,textFont,(int)aboutWin.TargetClientWidth,(int)150);
			text.Format=DrawTextFormat.Center;
			text.Text=
				"HokiEdit"
				+"\n"
				+"\nOfficial Hoki Hoki map editor"
				+"\nMay be freely redistributed"
				+"\n"
				+"\nCreated by Max Abernethy 2004";

			text.Y=20;
			aboutWin.AddContent(text);

			//Create an ok button for it
			aboutOkButton=new SpriteButton(device,sprite,textFont,menuButtonTex,this);
			aboutOkButton.Width=100;
			aboutOkButton.Text="OK";
			aboutOkButton.X=(int)(aboutWin.TargetClientWidth-aboutOkButton.Width)/2;
			aboutOkButton.Y=aboutWin.TargetClientHeight-aboutOkButton.Height-30;
			aboutOkButton.Press+=new MouseEventHandler(onAboutOk);
			aboutWin.AddContent(aboutOkButton);
			aboutWin.AddLockable(aboutOkButton);

			//Add it to the screen and the update list
			ui.Add(aboutWin);
			update.Add(aboutWin);
			aboutWin.Open();	//Allow proper display
			aboutWin.Lock();	//Prevent accidental use

			//Create a launcher window (set launcher frequency and offset)
			launcherWin=new SpriteWindow(device,sprite,textFont,windowTex);
			launcherWin.Text="Launcher settings";
			launcherWin.Width=300;
			launcherWin.Height=145;
			launcherWin.X=(int)(width-launcherWin.Width)/2;
			launcherWin.Y=(int)(height-launcherWin.Height)/2;
			launcherWin.Visible=false;

			//Add a launch frequency slider
			frequencySlider=new SpriteSlider(device,this,sliderTex);
			frequencySlider.Width=200;
			frequencySlider.X=(int)(launcherWin.ClientWidth-frequencySlider.Width)/2;
			frequencySlider.Y=15;
			frequencySlider.Change+=new EventHandler(onFrequencyChange);
			launcherWin.AddContent(frequencySlider);
			launcherWin.AddLockable(frequencySlider);

			//Add a label for the frequency slider
			frequencyText=new SpriteText(device,sprite,textFont,200,16);
			frequencyText.X=(int)(launcherWin.ClientWidth-frequencyText.Width)/2;
			frequencyText.Y=frequencySlider.Y+(int)frequencySlider.Height;
			launcherWin.AddContent(frequencyText);

			//Add a launch offset slider
			offsetSlider=new SpriteSlider(device,this,sliderTex);
			offsetSlider.Width=200;
			offsetSlider.X=frequencySlider.X;
			offsetSlider.Y=frequencyText.Y+(int)frequencyText.Height+10;
			offsetSlider.Change+=new EventHandler(onOffsetChange);
			launcherWin.AddContent(offsetSlider);
			launcherWin.AddLockable(offsetSlider);

			//Add a label for the offset slider
			offsetText=new SpriteText(device,sprite,textFont,200,16);
			offsetText.X=(int)(launcherWin.ClientWidth-offsetText.Width)/2;
			offsetText.Y=offsetSlider.Y+(int)offsetSlider.Height;
			launcherWin.AddContent(offsetText);

			//Add an ok button
			launcherOkButton=new SpriteButton(device,sprite,textFont,menuButtonTex,this);
			launcherOkButton.Width=100;
			launcherOkButton.Text="OK";
			launcherOkButton.X=(int)(launcherWin.ClientWidth-launcherOkButton.Width)/2;
			launcherOkButton.Y=offsetText.Y+(int)offsetText.Height+10;
			launcherOkButton.Press+=new MouseEventHandler(onLauncherOk);
			launcherWin.AddContent(launcherOkButton);
			launcherWin.AddLockable(launcherOkButton);

			//Add it to the screen and the update list
			ui.Add(launcherWin);
			update.Add(launcherWin);
			launcherWin.Open();	//Allow proper display
			launcherWin.Lock();	//Prevent accidental use

			//Initialize the timer
			DXUtil.Timer(DirectXTimer.GetElapsedTime);
		}
		#endregion

		#region drawing
		protected override void OnPaint(PaintEventArgs e) {
			//Updates
			float et=DXUtil.Timer(DirectXTimer.GetElapsedTime);
			foreach (Updateable u in update) u.Update(et);

			//Scroll
			int scrollSpeed=384;
			if (shift) scrollSpeed=1100;
			xScroll+=xScrollDir*et*scrollSpeed;
			yScroll+=yScrollDir*et*scrollSpeed;
			updateScroll();

			//Drawing
			device.Clear(ClearFlags.Target,Color.White.ToArgb(),0,0);
			device.BeginScene();

			root.Draw();

			device.EndScene();
			device.Present();

			this.Invalidate();
		}
		#endregion

		#region compilation
		/// <summary>
		/// Converts the entire map into a formatted string
		/// </summary>
		private string compile() {
			return compile(false);
		}

		/// <summary>
		/// Converts the entire map into a formatted string
		/// </summary>
		/// <param name="selectedOnly">If true, only selected elements will be added to the string</param>
		private string compile(bool selectedOnly) {
			int a,b;
			return compile(selectedOnly,out a,out b);
		}

		/// <summary>
		/// Converts the map into a formatted string
		/// </summary>
		/// <param name="selectedOnly">If true, only selected elements will be added to the string</param>
		/// <param name="xOffset">Value added to every x-coordinate to normalize them</param>
		/// <param name="yOffset">Value added to every y-coordinate to normalize them</param>
		private string compile(bool selectedOnly,out int xOffset,out int yOffset) {
			//Get minimum position to find offset (normalize so that the smallest x- and y-coords are 0)
			int minX,minY;
			if (nodes.Count==0) minX=minY=0;
			else {
				minX=minY=int.MaxValue;
				foreach (Node n in nodes) {
					if (n.Selected() || !selectedOnly) {
						int nx=(int)Math.Round(n.X),ny=(int)Math.Round(n.Y);
						if (nx<minX) minX=nx;
						if (ny<minY) minY=ny;
					}
				}
				foreach (SpriteMarker s in sprites) {
					if (s.Selected() || !selectedOnly) {
						int sx=(int)Math.Round(s.X),sy=(int)Math.Round(s.Y);
						if (sx<minX) minX=sx;
						if (sy<minY) minY=sy;
					}
				}
			}

			xOffset=-minX;
			yOffset=-minY;

			//Get counts
			int nodeCount=0,polyCount=0;
			if (selectedOnly) {	//Only count selected elements
				foreach (Node n in nodes) if (n.Selected()) nodeCount++;
				foreach (Triangle t in polys) if (t.Selected()) polyCount++;
			} else {			//Count all
				nodeCount=nodes.Count;
				polyCount=polys.Count;
			}
			
			//Output headers, node mode directive
			string map=
				"\n#NODECOUNT "+nodes.Count
				+"\n#WALLCOUNT "+lines.Count
				+"\n#POLYCOUNT "+polys.Count
				+"\n#AUTHOR "+author
				+"\n#THEME "+settingsForm.Theme.Text
				+((settingsForm.Ghost.Text.Length>0 && !settingsForm.Ghost.Text.Equals("<None>"))?("\n#GHOST "+settingsForm.Ghost.Text):"")
				+"\n>NODES";

			//List nodes
			int nodeid=0;
			foreach (Node n in nodes) {
				if (n.Selected() || !selectedOnly) {
					n.ID=nodeid++;
					map+=n.Compile(xOffset,yOffset);
				}
			}

			//List polys
			map+="\n>TRIANGLES";
			foreach (Triangle t in polys)
				if (t.Selected() || !selectedOnly)
					map+=t.Compile();

			//List lines
			map+="\n>LINES";
			foreach (Line l in lines)
				if (l.Selected() || !selectedOnly)
					map+=l.Compile();

			//List pads
			map+="\n>PADS";
			foreach (Pad p in pads)				
				if (p.Selected() || !selectedOnly)
					map+=p.Compile(xOffset,yOffset);

			//List springs
			map+="\n>SPRINGS";
			foreach (Spring s in springs)
				if (s.Selected() || !selectedOnly)
					map+=s.Compile(xOffset,yOffset);

			//List launchers
			if (launchers.Count>0) {
				map+="\n>LAUNCHERS";
				foreach (Launcher l in launchers) {
					if ((l.Selected() || !selectedOnly) && l.Sender)
						map+=l.Compile(xOffset,yOffset);
				}

				//List catchers
				map+="\n>CATCHERS";
				foreach (Launcher l in launchers)
					if ((l.Selected() || !selectedOnly) && !l.Sender)
						map+=l.Compile(xOffset,yOffset);	//Second loop through: compile the catchers
			}

			//List sprites
			map+="\n>SPRITES";
			foreach (SpriteMarker s in sprites)
				if (s.Selected() || !selectedOnly)
					map+=s.Compile(xOffset,yOffset);	//Second loop through: compile the catchers

			return map;
		}
		#endregion

		#region workspace management
		private void updateScroll() {
			int xOff=-(int)xScroll;
			int yOff=-(int)yScroll;
			workspace.X=xOff-xOff%8;
			preview.X=workspace.X;
			lowPreview.X=workspace.X;

			workspace.Y=yOff-yOff%8;
			preview.Y=workspace.Y;
			lowPreview.Y=workspace.Y;

			updatePad(lastMouse.X,lastMouse.Y); //Keep the preview pad updated
			if (linking) updatePreviewLine(lastMouseEvent);
		}

		/// <summary>
		/// Converts screen coordinates to rounded node coordinates
		/// </summary>
		/// <param name="x">X-position in screen coords</param>
		/// <param name="y">Y-position in screen coords</param>
		private Vector2 nodeCoord(int x,int y) {
			return workspaceCoord(x-x%8,y-y%8);
		}

		private Vector2 workspaceCoord(int x,int y) {
			Vector2 nodePos=new Vector2(x,y);
			workspace.GlobalToLocal(ref nodePos);
			return nodePos;
		}

		/// <summary>
		/// Creates a new node and returns it
		/// </summary>
		/// <param name="x">X-position in screen coordinates</param>
		/// <param name="y">Y-position in screen coordinates</param>
		private Node makeNode(int x,int y) {

			//Snap to grid and localize to workspace
			Vector2 nodePos=nodeCoord(x,y);

			//Create the node
			Node n=new Node(device,nodeTex,this);
			n.X=nodePos.X;
			n.Y=nodePos.Y;
			nodeLayer.Add(n);	//Add to the nodelayer so that its coordinates are relevant, remove on collision

			//make sure it doesn't collide with anything
			bool collision=false;
			foreach (SpriteObject other in nodes) {
				if (detectCollision(n,other)) {
					//Prepare the preview line
					lastNode=(Node)other;
					setPreviewLine((Node)other);

					//Record the collision, do not create
					collision=true;
					break;
				}
			}
			foreach (Pad other in pads) {
				if (detectCollision(n,other)) {
					collision=true;
					break;
				}
			}
			
			if (collision) {
				nodeLayer.Remove(n);
				return null;	//No node could be created
			} else {
				//Add the node to the workspace, node, and element lists
				elements.Add(n);	//Is a MapElement
				nodes.Add(n);		//Is a Node

				//Update linking information
				setPreviewLine(n);

				//Make a new line
				if (linking && lastNode!=null) {	
					makeLine(lastNode,n);
				}
				lastNode=n;

				//Capture node delete event
				n.Delete+=new EventHandler(OnNodeDelete);

				//The map has changed, so we can't undelete
				setUndelete(false);

				return n;	//The node is created, so return it
			}
		}

		private Line makeLine(Node from,Node to) {
			Line l=new Line(device,from,to);
			lineLayer.Add(l);
			elements.Add(l);
			lines.Add(l);

			//Capture line delete event
			l.Delete+=new EventHandler(OnLineDelete);

			//The map has changed, so we can't undelete
			setUndelete(false);

			return l;
		}

		private Triangle makeTriangle(Node a,Node b,Node c) {
			Triangle tri=new Triangle(device,triTex,a,b,c);
			tri.Alpha=100;
			polys.Add(tri);
			elements.Add(tri);
			polyLayer.Add(tri);

			//Capture delete event
			tri.Delete+=new EventHandler(OnPolyDelete);

			//The map has changed, so we can't undelete
			setUndelete(false);

			return tri;
		}

		private Pad makePad(PadType type,int x,int y) {
			Pad p=new Pad(device,sprite,labelFont,type);
			p.X=x;
			p.Y=y;
			padLayer.Add(p);	//Add it to the pad layer so that its coordinates are relevant - remove on collision

			//Test for collision before making
			bool collide=false;
			foreach (SpriteObject other in nodes)	if (detectCollision(p,other)) collide=true;
			foreach (SpriteObject other in pads)	if (detectCollision(p,other)) collide=true;

			if (collide) {
				padLayer.Remove(p);	//There was a collision, remove from the pad layer
				return null;		//Return null, since nothing was created
			} else {
				//Add to lists and layers
				pads.Add(p);
				elements.Add(p);

				//Capture delete event
				p.Delete+=new EventHandler(OnPadDelete);
				
				//The map has changed, so we can't undelete
				setUndelete(false);

				return p;
			}
		}

		private Spring makeSpring(int x,int y,int turns) {
			Spring s=new Spring(device,springTex);
			s.X=x;
			s.Y=y;
			for (int i=0;i<turns;i++) s.Rotate();

			springs.Add(s);
			elements.Add(s);
			springLayer.Add(s);

			s.Delete+=new EventHandler(OnSpringDelete);

			return s;
		}

		/// <summary>
		/// Determines if a and b's width/height boxes overlap.
		/// Assumes that their origins are topleft corner.
		/// They can be in different coordinate spaces, collision is based on screen space
		/// </summary>
		private bool detectCollision(SpriteObject a,SpriteObject b) {
			Vector2 tPos=new Vector2(0,0);
			a.GlobalToLocal(ref tPos);
			Vector2 bPos=new Vector2(0,0);
			b.LocalToGlobal(ref bPos);			//Convert b's position to screen coordinates
			a.GlobalToLocal(ref bPos);			//Convert b's position to a's coordinate space
			return (Math.Round(bPos.X)>Math.Round(-b.Width) && Math.Round(bPos.X)<Math.Round(a.Width) && Math.Round(bPos.Y)>Math.Round(-b.Height) && Math.Round(bPos.Y)<Math.Round(a.Height));
		}

		/// <summary>
		/// Deselects all elements
		/// </summary>
		private void deselectAll() {
			foreach (MapElement m in elements) m.Deselect();
		}

		/// <summary>
		/// Deletes all selected elements
		/// </summary>
		private void deleteSelection() {
			int xOff,yOff;
			undelete=compile(false,out xOff,out yOff);
			setUndelete(true);
			deleteX=(int)xScroll+xOff;
			deleteY=(int)yScroll+yOff;

			MapElement m;
			for (int i=elements.Count-1;i>=0;i--) {
				m=(MapElement)elements[i];
				if (m.Selected()) m.TriggerDelete();
			}
		}

		/// <summary>
		/// If the mouse is hovering over a node, the node is returned. Otherwise, null is returned.
		/// </summary>
		/// <param name="e">The arguments from the mouse event</param>
		private Node hoverNode(MouseEventArgs e) {
			Vector2 m=new Vector2(e.X,e.Y);
			foreach (Node n in nodes) if (n.PtInShape(m)) return n;
			return null;
		}

		/// <summary>
		/// If the mouse is hovering over a line, the line is returned. Otherwise, null is returned.
		/// </summary>
		/// <param name="e">The arguments from the mouse event</param>
		private Line hoverLine(MouseEventArgs e) {
			Vector2 m=new Vector2(e.X,e.Y);
			foreach (Line l in lines) if (l.PtInShape(m)) return l;
			return null;
		}

		/// <summary>
		/// If the mouse is hovering over a pad, the line is returned. Otherwise, null is returned.
		/// </summary>
		/// <param name="e">The arguments from the mouse event</param>
		private Pad hoverPad(MouseEventArgs e) {
			Vector2 m=new Vector2(e.X,e.Y);
			foreach (Pad p in pads) if (p.PtInShape(m)) return p;
			return null;
		}

		private Spring hoverSpring(MouseEventArgs e) {
			Vector2 m=new Vector2(e.X,e.Y);
			foreach (Spring s in springs) if (s.PtInShape(m)) return s;
			return null;
		}

		private Launcher hoverLauncher(MouseEventArgs e) {
			Vector2 m=new Vector2(e.X,e.Y);
			foreach (Launcher l in launchers) if (l.PtInShape(m)) return l;
			return null;
		}

		private SpriteMarker hoverMarker(MouseEventArgs e) {
			Vector2 m=new Vector2(e.X,e.Y);
			foreach (SpriteMarker sm in sprites) if (sm.PtInShape(m)) return sm;
			return null;
		}

		private Triangle hoverTri(MouseEventArgs e) {
			Vector2 m=new Vector2(e.X,e.Y);
			foreach (Triangle t in polys) if (t.PtInShape(m)) return t;
			return null;
		}

		private void setPreviewLine(Node fromNode) {
			previewLine.X=fromNode.X+fromNode.Width/2;
			previewLine.Y=fromNode.Y+fromNode.Height/2;
		}

		private void updatePreviewLine(MouseEventArgs e) {
			Vector2 mCoord=workspaceCoord((int)cursor.X,(int)cursor.Y);
			previewLine.Width=mCoord.X-previewLine.X;
			previewLine.Height=mCoord.Y-previewLine.Y;

			if (tool==Tool.Node) {
				previewLine.Width+=4;
				previewLine.Height+=4;
			}
		}

		private void stopLinking() {
			previewLine.Visible=false;
			linking=false;
			fromNode=null;
		}

		private void moveSelection(float xc,float yc) {
			foreach (MapElement m in elements) if (m.Selected()) m.Move(new Vector2(xc,yc));
			foreach (Node n in nodes) n.Update();
			
			//The map has changed, so we can't undelete
			setUndelete(false);
		}

		private void updatePad(float x,float y) {
			Vector2 m=new Vector2(x-x%8,y-y%8);
			preview.GlobalToLocal(ref m);
			previewPad.X=m.X;
			previewPad.Y=m.Y;
		}

		private void updateSpring(float x,float y) {
			Vector2 m=new Vector2(x-x%8,y-y%8);
			preview.GlobalToLocal(ref m);
			previewSpring.X=m.X;
			previewSpring.Y=m.Y;
		}

		private void updateLauncher(float x,float y) {
			if (launcherDown) {
				Vector2 m=new Vector2(x,y);
				preview.GlobalToLocal(ref m);
				previewLauncherTo.SetPos(m.X,m.Y);

				launcherArrow.SetPos(new Vector2(previewLauncherFrom.DrawX,previewLauncherFrom.DrawY),new Vector2(previewLauncherTo.DrawX,previewLauncherTo.DrawY));
			} else {
				Vector2 m=new Vector2(x-x%8,y-y%8);
				preview.GlobalToLocal(ref m);
				previewLauncherFrom.X=m.X;
				previewLauncherFrom.Y=m.Y;
			}
		}

		private void updateSprite(float x,float y) {
			Vector2 m=new Vector2(x-x%4,y-y%4);
			preview.GlobalToLocal(ref m);
			previewMarker.X=m.X;
			previewMarker.Y=m.Y;
		}

		#endregion

		#region button event handling
		private void selectorButton_Press(object sender, MouseEventArgs e) {
			setTool(Tool.Select);
		}

		private void nodeButton_Press(object sender, MouseEventArgs e) {
			setTool(Tool.Node);
		}

		private void lineButton_Press(object sender, MouseEventArgs e) {
			setTool(Tool.Line);
		}

		private void polyButton_Press(object sender, MouseEventArgs e) {
			setTool(Tool.Polygon);
		}

		private void padButton_Press(object sender, MouseEventArgs e) {
			setTool(Tool.Pad);
		}
		
		private void selectorButton_RollOver(object sender, MouseEventArgs e) {
			toolText.Text="Select tool - Select elements by clicking them or dragging a box around them.";
		}

		private void nodeButton_RollOver(object sender, MouseEventArgs e) {
			toolText.Text="Node tool - Click to create nodes.";
		}

		private void lineButton_RollOver(object sender, MouseEventArgs e) {
			toolText.Text="Wall tool - Click on two nodes to make a wall between them.";
		}

		private void polyButton_RollOver(object sender, MouseEventArgs e) {
			toolText.Text="Polygon tool - Click on three nodes to define an in-bounds polygon.";
		}

		private void padButton_RollOver(object sender, MouseEventArgs e) {
			toolText.Text="Pad tool - Click to create special pads, right-click to rotate pad type.";
		}

		private void springButton_Press(object sender, MouseEventArgs e) {
			setTool(Tool.Spring);
		}

		private void springButton_RollOver(object sender, MouseEventArgs e) {
			toolText.Text="Spring tool - Click to make springs, right-click to rotate";
		}

		private void launcherButton_Press(object sender, MouseEventArgs e) {
			setTool(Tool.Launcher);
		}

		private void pencilButton_Press(object sender, MouseEventArgs e) {
			setTool(Tool.Pencil);
		}

		private void pencilButton_RollOver(object sender, MouseEventArgs e) {
			toolText.Text="Pencil tool - Hold mouse to draw walls";
		}

		private void spriteButton_Press(object sender, MouseEventArgs e) {
			setTool(Tool.Sprite);
		}

		private void spriteButton_RollOver(object sender, MouseEventArgs e) {
			toolText.Text="Sprite tool - click to make sprites, mousewheel to change ID, shift+mousewheel to change depth";
		}

		private void launcherButton_RollOver(object sender, MouseEventArgs e) {
			toolText.Text="Launcher tool - Click to make mine launchers, right-click to rotate";
		}
		#endregion

		#region form event handling
		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove (e);

			//Scroll
			if (scrolling) {
				int xDist=(int)e.X-xScrollCenter;
				xDist-=xDist%8;
				
				int yDist=(int)e.Y-yScrollCenter;
				yDist-=yDist%8;

				xScroll=xScrollStart-xDist;
				yScroll=yScrollStart-yDist;
			}

			if (!scrolling) {
				//Align cursor image with mouse position
				cursor.X=e.X;
				cursor.Y=e.Y;

				//Switch to regular cursor if the mouse moves out of the workspace or using a window
				if (cursor.X<32 || tool==Tool.None)	toWinCursor();
				else								toSpriteCursor();
			}

			if (linking) updatePreviewLine(e);
			
			switch (tool) {

				case Tool.Select:
					if (dragging) {
						moveSelection(e.X-e.X%8-lastMouse.X,e.Y-e.Y%8-lastMouse.Y);
					} else if (down) {
						boxing=true;	//The user is dragging the mouse
						selectBox.Width=e.X-selectBox.X;
						selectBox.Height=e.Y-selectBox.Y;
					}
					break;
				case Tool.Pencil:
					if (drawing) {
						Vector2 oldDraw=drawLast;
						drawLast=workspaceCoord(e.X,e.Y);

						//Draw a line
						SpriteLine l=new SpriteLine(device);
						l.X=oldDraw.X;
						l.Y=oldDraw.Y;
						l.Width=drawLast.X-oldDraw.X;
						l.Height=drawLast.Y-oldDraw.Y;
						l.Thickness=2;
						l.Tint=Color.FromArgb(50,82,118);
						drawLayer.Add(l);

						//Add the distance
						drawDist+=Vector2.Length(drawLast-oldDraw);
						if (drawDist>=nodeSpace) {
							Node newNode=makeNode(e.X,e.Y);
							if (newNode==null) newNode=hoverNode(e);
							makeLine(drawNode,newNode);
							drawNode=newNode;
							drawDist-=nodeSpace;
						}
					}
					break;
				case Tool.Node:
					//Round off the node tool's position
					cursor.X-=cursor.X%8;
					cursor.Y-=cursor.Y%8;
					break;
				case Tool.Pad:
					updatePad(e.X,e.Y);
					break;
				case Tool.Polygon:
					Vector2 m=new Vector2(e.X,e.Y);
					lowPreview.GlobalToLocal(ref m);
					if (triIndex>0) {
						previewTri.SetPosition(triIndex,m);
						if (triIndex==1) {
							float ang=FMath.Angle(new Vector2(triNode0.X,triNode0.Y),m)+FMath.PI/2;
							previewTri.SetPosition(2,new Vector2(m.X+2*FMath.Cos(ang),m.Y+2*FMath.Sin(ang)));
						}
					}
					break;
				case Tool.Spring:
					updateSpring(e.X,e.Y);
					break;
				case Tool.Launcher:
					updateLauncher(e.X,e.Y);
					break;
				case Tool.Sprite:
					updateSprite(e.X,e.Y);
					break;
			}

			//Update the preview line
			if (lastNode!=null && !linking) setPreviewLine(lastNode);

			//Record new position
			lastMouse.X=e.X-e.X%8;
			lastMouse.Y=e.Y-e.Y%8;
			lastMouseEvent=e;

			//Note the position on the bottom
			if (e.X>32) {
				Vector2 pos=nodeCoord(e.X,e.Y);
				toolText.Text=(int)(pos.X/8)+","+(int)(pos.Y/8);
				if (linking) {
					//Create some space (tabs don't work)
					int diff=20-toolText.Text.Length;
					for (int i=0;i<diff;i++) toolText.Text+=" ";
					
					//Write out the size of the line
					toolText.Text+="Wall: "+(int)(previewLine.Width/8)+","+(int)(previewLine.Height/8);
				}
			}
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown (e);

			if (e.X<32) return;	//HACK, don't do anything unless the mouse is in the workspace

			//For middle mouse button, begin scrolling and return
			if (e.Button==MouseButtons.Middle) {
				scrolling=true;
				xScrollDir=yScrollDir=0;
				xScrollCenter=e.X;
				yScrollCenter=e.Y;
				xScrollStart=(int)xScroll;
				yScrollStart=(int)yScroll;
				toWinCursor();
				Cursor.Current=scrollCursor;
				return;
			}

			//Record mouse state
			down=true;

			Node h; //Local hover node storage

			switch (tool) {
				case Tool.Select:
					//Reset the select box
					selectBox.X=e.X;
					selectBox.Y=e.Y;
					selectBox.Width=selectBox.Height=0;
					selectBox.Visible=true;

					//Begin dragging if applicable
					h=hoverNode(e);
					if (h!=null) {
						if (e.Button==MouseButtons.Right) {
							if (h.Selected()) deleteSelection();	//Right click on selection=delete selection
							else	h.TriggerDelete();				//Right click on node=delete node
						} else if (!h.Selected()) {			//Mouse was over a deselected node. Deselect all and select the node.
							if (!shift) deselectAll();		//Deselect others if not shift-clicking
							h.Select();
							dragging=true;
						} else if (shift) h.Deselect();
						else dragging=true;					//Dragging is on because the mouse is now over a selected node.
					} else {
						Line l=hoverLine(e);
						if (l!=null) {
							if (e.Button==MouseButtons.Right) {
								if (l.Selected()) 
									deleteSelection();
								else
									l.TriggerDelete();
							} else if (!l.Selected()) {		//Mouse was over a deselected line. Deselect all others and select the line.
								if (!shift) deselectAll();	//Deselect others if not shift-clicking
								l.Select();
								dragging=true;
							} else if (shift) l.Deselect();
							else dragging=true;
						} else {								Spring hs=hoverSpring(e);
							if (hs!=null) {
								if (e.Button==MouseButtons.Right) {
									if (hs.Selected()) 
										deleteSelection();		//Right click on selection=delete selection
									else
										hs.TriggerDelete();		//Right click on pad=delete pad
								} else if (!hs.Selected()) {
									//select the pad
									if (!shift) deselectAll();
									hs.Select();
									dragging=true;
								} else if (shift) hs.Deselect();
								else dragging=true;
							} else {
								Launcher hl=hoverLauncher(e);
								if (hl!=null) {
									if (e.Button==MouseButtons.Right) {
										if (hl.Selected()) 
											deleteSelection();		//Right click on selection=delete selection
										else
											hl.TriggerDelete();		//Right click on pad=delete pad
									} else if (!hl.Selected()) {
										//select the pad
										if (!shift) deselectAll();
										hl.Select();
										dragging=true;
									} else if (shift) hl.Deselect();
									else dragging=true;
								} else {
									SpriteMarker hm=hoverMarker(e);
									if (hm!=null) {
										if (e.Button==MouseButtons.Right) {
											if (hm.Selected())
												deleteSelection();
											else
												hm.TriggerDelete();
										} else if (!hm.Selected()) {
											//select the sprite
											if (!shift) deselectAll();
											hm.Select();
											dragging=true;
										} else if (shift) hm.Deselect();
										else dragging=true;
									} else {
										Pad hp=hoverPad(e);
										if (hp!=null) {
											if (e.Button==MouseButtons.Right) {
												if (hp.Selected()) 
													deleteSelection();		//Right click on selection=delete selection
												else
													hp.TriggerDelete();		//Right click on pad=delete pad
											} else if (!hp.Selected()) {
												//select the pad
												if (!shift) deselectAll();
												hp.Select();
												dragging=true;
											} else if (shift) hp.Deselect();
											else dragging=true;
										} else {
											Triangle ht=hoverTri(e);
											if (ht!=null) {
												if (e.Button==MouseButtons.Right) {
													if (ht.Selected())
														deleteSelection();
													else
														ht.TriggerDelete();
												} else if (!ht.Selected()) {
													//Select the poly
													if (!shift) deselectAll();
													ht.Select();
													dragging=true;
												} else if (shift) ht.Deselect();
												else dragging=true;
											} else if (!shift) deselectAll();
										}
									}
								}
							}
						}
					}
					break;
				case Tool.Pencil:
					drawing=true;
					drawLast=workspaceCoord(e.X,e.Y);
					drawNode=makeNode(e.X,e.Y);
					if (drawNode==null) drawNode=hoverNode(e);
					drawDist=0;
					break;
				case Tool.Node:
					if (e.Button==MouseButtons.Right) {
						h=hoverNode(e);
						if (h!=null) h.TriggerDelete();
					} else
						makeNode(e.X,e.Y);
					break;
				case Tool.Line:
					if (e.Button==MouseButtons.Right) stopLinking();	//Right click=cancel
					else {
						h=hoverNode(e);
						if (linking && h!=null) {
							makeLine(fromNode,h);

							if (shift) {
								//Relay to next
								fromNode=h;
								setPreviewLine(fromNode);
								updatePreviewLine(e);
							} else stopLinking();	//Terminate
						} else if (h!=null) {
							//Clicked on a node, start a new link
							fromNode=h;
							linking=true;
							previewLine.Visible=true;
							setPreviewLine(fromNode);
							updatePreviewLine(e);
						}
					}
					break;
				case Tool.Pad:
					if (e.Button==MouseButtons.Right) {
						//Cycle type
						switch (previewPad.Type) {
							case PadType.End:
								previewPad.Type=PadType.Heal;
								break;
							case PadType.Heal:
								previewPad.Type=PadType.Start;
								break;
							case PadType.Start:
								previewPad.Type=PadType.End;
								break;
						}
					} else
						//Create a new pad
						makePad(previewPad.Type,(int)previewPad.X,(int)previewPad.Y);
					break;
				case Tool.Polygon:
					if (e.Button==MouseButtons.Right) {
						triIndex=0;
						resetPreviewTri();
					} else {
						h=hoverNode(e);
						if (h!=null) {
							previewTri.SetPosition(triIndex,new Vector2(h.X+4,h.Y+4));
							if (triIndex==0) {
								previewTri.Visible=true;
								triNode0=h;
								triIndex=1;
							} else if (triIndex==1) {
								triNode1=h;
								triIndex=2;
							} else if (triIndex==2) {
								//Make a new triangle
								makeTriangle(triNode0,triNode1,h);

								//Hide preview
								triIndex=0;
								resetPreviewTri();

								//If shift is down, start a new triangle automatically
								if (shift) {
									previewTri.Visible=true;
									triNode0=triNode1;
									triNode1=h;
									previewTri.SetPosition(0,new Vector2(triNode0.X+4,triNode0.Y+4));
									previewTri.SetPosition(1,new Vector2(triNode1.X+4,triNode1.Y+4));
									previewTri.SetPosition(2,new Vector2(h.X,h.Y));
									triIndex=2;
								}
							}
						}
					}
					break;
				case Tool.Spring:
					if (e.Button==MouseButtons.Left)
						makeSpring((int)previewSpring.X,(int)previewSpring.Y,previewSpring.Turns);
					else if (e.Button==MouseButtons.Right)
						previewSpring.Rotate();
					break;
				case Tool.Launcher:
					if (e.Button==MouseButtons.Left) {
						if (launcherDown) { //Finish the launcher
							//Duplicate the previews
							Launcher a=new Launcher(device,launcherTex);
							a.X=previewLauncherFrom.X;
							a.Y=previewLauncherFrom.Y;
							a.Turns=previewLauncherFrom.Turns;
							a.Sender=true;

							Launcher b=new Launcher(device,launcherTex);
							b.X=previewLauncherTo.X;
							b.Y=previewLauncherTo.Y;
							b.Turns=previewLauncherTo.Turns;

							//Connect the two
							a.Other=b;

							//Add to lists, a above b (so that the selection arrow shows on top)
							launchers.Add(b);
							launchers.Add(a);
							launcherLayer.Add(b);
							launcherLayer.Add(a);
							elements.Add(b);
							elements.Add(a);

							//Capture the delete events
							a.Delete+=new EventHandler(onLauncherDelete);
							b.Delete+=new EventHandler(onLauncherDelete);

							//Reset the previews
							launcherArrow.Visible=false;
							previewLauncherTo.Visible=false;
							launcherDown=false;
						} else {	//Move to step 2
							launcherDown=true;

							previewLauncherTo.Visible=true;
							previewLauncherTo.Other=previewLauncherFrom;
							previewLauncherTo.X=previewLauncherFrom.X;
							previewLauncherTo.Y=previewLauncherFrom.Y;
							previewLauncherTo.Turns=previewLauncherFrom.Turns+4;

							launcherArrow.Visible=true;
							updateLauncher(e.X,e.Y);
						}
					} else if (e.Button==MouseButtons.Right && !launcherDown)
						previewLauncherFrom.Rotate();					
					break;
				case Tool.Sprite:
					if (e.Button==MouseButtons.Left) {
						//Duplicate the preview
						SpriteMarker m=new SpriteMarker(device,spriteTex,sprite,textFont,this);
						m.X=previewMarker.X;
						m.Y=previewMarker.Y;
						m.Index=previewMarker.Index;
						m.Depth=previewMarker.Depth;

						sprites.Add(m);
						spriteLayer.Add(m);
						elements.Add(m);

						m.Delete+=new EventHandler(onSpriteDelete);
					}
					break;
			}
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp (e);
			
			//For middle mouse button, stop scrolling and return
			if (e.Button==MouseButtons.Middle) {
				scrolling=false;
				toSpriteCursor();
				Cursor.Current=noCursor;
				return;
			}

			//Record mouse state
			down=false;
			dragging=false;

			switch (tool) {
				case Tool.Select:
					if (boxing) {	//Was boxing
						//Stop boxing
						selectBox.Visible=false;
						boxing=false;

						//Convert the box's coordinates to useful numbers
						selectBox.Normalize();

						//Get node coordinates of top left and bottom right corners of the select box
						Vector2 tl=nodeCoord((int)selectBox.X,(int)selectBox.Y);
						Vector2 br=nodeCoord((int)(selectBox.X+selectBox.Width),(int)(selectBox.Y+selectBox.Height));

						foreach (MapElement m in elements) if (m.InBox(selectBox)) m.Select();

					}
					break;
				case Tool.Pencil:
					drawing=false;
					drawLayer.Clear();
					break;
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e) {
			base.OnMouseWheel (e);

			if (tool==Tool.Sprite) {
				int dir=e.Delta>0?1:-1;
				if (shift) previewMarker.Depth+=dir*0.05f;
				else previewMarker.Index+=dir;
			}
		}

		protected override void OnDoubleClick(EventArgs e) {
			if (tool==Tool.Select) {
				Launcher l=hoverLauncher(lastMouseEvent);
				if (l!=null) {	//Edit this launcher
					//Get the sender of the pair l is part of
					if (l.Sender) editLauncher=l;
					else editLauncher=l.Other;

					launcherWin.Unlock();							//Unlock the window so its children can be used
					frequencySlider.Value=editLauncher.Frequency;	//Set the slider to the current frequency
					offsetSlider.Value=editLauncher.Offset;			//...and offset.
					launcherWin.Visible=true;						//Show the window
					toolSave=tool;	//Store the tool so it can be set to None
					tool=Tool.None;
				}
			} 
		}

		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave (e);

			//Hide the sprite cursor
			toWinCursor();
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			switch (e.KeyCode) {
				case Keys.ControlKey:
					ctrl=true;
					break;
				case Keys.ShiftKey:
					shift=true;

					if (tool==Tool.Node && lastNode!=null) {
						linking=true;
						previewLine.Visible=true;
					}

					break;
				case Keys.Delete:
					deleteSelection();
					break;
				case Keys.W:
					if (shift)	moveSelection(0,-48);
					else		moveSelection(0,-8);
					break;
				case Keys.A:
					if (shift)	moveSelection(-48,0);
					else		moveSelection(-8,0);
					break;
				case Keys.S:
					if (!ctrl) {
						if (shift)	moveSelection(0,48);
						else		moveSelection(0,8);
					}
					break;
				case Keys.D:
					if (shift)	moveSelection(48,0);
					else		moveSelection(8,0);
					break;
				case Keys.Z:
					if (!ctrl) setTool(Tool.Select);
					break;
				case Keys.X:
					if (!ctrl) setTool(Tool.Node);
					break;
				case Keys.C:
					if (!ctrl) setTool(Tool.Line);
					break;
				case Keys.V:
					if (ctrl) onPaste(this,null);
					else setTool(Tool.Polygon);
					break;
				case Keys.B:
					setTool(Tool.Pad);
					break;
				case Keys.N:
					if (!ctrl) setTool(Tool.Spring);
					break;
				case Keys.M:
					setTool(Tool.Launcher);
					break;
				case Keys.Oemcomma:
					setTool(Tool.Pencil);
					break;
				case Keys.OemPeriod:
					setTool(Tool.Sprite);
					break;
			}
			
			base.OnKeyDown (e);
		}

		protected override void OnKeyUp(KeyEventArgs e) {
			switch (e.KeyCode) {
				case Keys.ControlKey:
					ctrl=false;
					break;
				case Keys.ShiftKey:
					shift=false;

					if (tool==Tool.Node) {
						stopLinking();
					}

					break;
			}

			base.OnKeyUp (e);
		}

		private void onFileOk(object sender, CancelEventArgs e) {
			onSave(null,null);
		}
		#endregion

		#region helpers

		private void setUndelete(bool status) {
			undeleteCommand.Enabled=canUndelete=status;
		}

		private void center(int x,int y) {
			xScroll=x-32;
			yScroll=0;
		}

		/// <summary>
		/// Makes elements given in the map string format
		/// </summary>
		/// <param name="mapData">A string that fully adheres to the map format</param>
		/// <param name="offset">Block distance to move each element from its stated position</param>
		/// <param name="select">Whether the elements should be selected on creation</param>
		private void constructElements(String mapData,int xOffset,int yOffset,bool select) {
			constructElements(mapData,xOffset,yOffset,select,false);
		}

		/// <summary>
		/// Makes elements given in the map string format
		/// </summary>
		/// <param name="mapData">A string that fully adheres to the map format</param>
		/// <param name="offset">Block distance to move each element from its stated position</param>
		/// <param name="select">Whether the elements should be selected on creation</param>
		/// <param name="visit">Whether the map should scroll to the location of the constructed elements</param>
		private void constructElements(String mapData,int xOffset,int yOffset,bool select,bool visit) {
			//Clear selection if necessary
			if (select) deselectAll();

			//Locals
			String[] data;
			MapModes mode=MapModes.Meta;
			Node[] nodeTable=null;
			int currentNode=0;
			int currentCatcher=launchers.Count;

			//Read from the file
			String[] map=mapData.Split('\n');

			//Iterate through the lines
			for (int i=0;i<map.Length;i++) {
				String line=map[i];				//Get the line
				if (line.Length==0) continue;	//Ignore blank lines

				//See if the mode needs to be changed
				if (line[0]=='>') {
					switch (line) {
						case ">NODES":
							mode=MapModes.Nodes;
							break;
						case ">LINES":
							mode=MapModes.Walls;
							break;
						case ">TRIANGLES":
							mode=MapModes.Polygons;
							break;
						case ">PADS":
							mode=MapModes.Pads;
							break;
						case ">SPRINGS":
							mode=MapModes.Springs;
							break;
						case ">LAUNCHERS":
							mode=MapModes.Launchers;
							break;
						case ">CATCHERS":
							mode=MapModes.Catchers;
							break;
						case ">SPRITES":
							mode=MapModes.Sprites;
							break;
					}
				} else {
					switch (mode) {
						case MapModes.Meta:		//Obtain information about the map
							data=line.Split(' ');
							switch (data[0]) {
								case "#NODECOUNT":
									int nodeCount=int.Parse(data[1]);	//This line defines the number of nodes there will be
									nodeTable=new Node[nodeCount];
									break;
								case "#AUTHOR":
									author=settingsForm.Author.Text=data[1];
									break;
								case "#GHOST":
									settingsForm.Ghost.Text=data[1];
									break;
								case "#THEME":
									settingsForm.Theme.Text=data[1];
									break;
							}
							break;
						case MapModes.Nodes:	//Record node positions
							data=line.Split(',');

							//Create a new node
							Node n=makeNode((int.Parse(data[0])+xOffset)*8+(int)workspace.X,(int.Parse(data[1])+yOffset)*8+(int)workspace.Y);
							if (n!=null) {
								if (select) n.Select();
								nodeTable[currentNode++]=n;
							}
							break;
						case MapModes.Walls:	//Create walls and determine how many walls are at each node
							data=line.Split(',');

							Node a=nodeTable[int.Parse(data[0])],b=nodeTable[int.Parse(data[1])];

							if (a!=null && b!=null) {	//It's possible that some nodes could not be created due to collision
								Line l=makeLine(a,b);	//Create a new line
								if (select) l.Select();
							}

							break;
						case MapModes.Polygons:
							data=line.Split(',');

							Node c=nodeTable[int.Parse(data[0])],d=nodeTable[int.Parse(data[1])],e=nodeTable[int.Parse(data[2])];

							if (c!=null && d!=null && e!=null) {	//It's possible that some nodes could not be created due to collision
                                Triangle t=makeTriangle(c,d,e);		//Create a new triangle
								if (select) t.Select();
							}

							break;
						case MapModes.Pads:
							data=line.Split(',');

							//Create a new pad
							Pad p=makePad((PadType)int.Parse(data[2]),(int.Parse(data[0])+xOffset)*8,(int.Parse(data[1])+yOffset)*8);
							if (p!=null && select) p.Select();
							break;
						case MapModes.Springs:
							data=line.Split(',');

							//Create a new spring
							Spring s=makeSpring(int.Parse(data[0])*8,int.Parse(data[1])*8,int.Parse(data[2]));
							if (select) s.Select();
							break;
						case MapModes.Launchers:
							data=line.Split(',');

							//Create a new launcher
							Launcher launcher=new Launcher(device,launcherTex);
							launcher.X=int.Parse(data[0])*8;
							launcher.Y=int.Parse(data[1])*8;
							launcher.Turns=int.Parse(data[2]);
							launcher.Frequency=float.Parse(data[3])/100;
							launcher.Offset=float.Parse(data[4])/100;
							launcher.Sender=true;

							launchers.Add(launcher);
							elements.Add(launcher);

							//Capture the delete event
							launcher.Delete+=new EventHandler(onLauncherDelete);

							break;
						case MapModes.Catchers:
							data=line.Split(',');

							//Create a new catcher
							Launcher catcher=new Launcher(device,launcherTex);
							catcher.X=int.Parse(data[0])*8;
							catcher.Y=int.Parse(data[1])*8;
							catcher.Turns=int.Parse(data[2]);
							catcher.Sender=false;
							
							Launcher sender=(Launcher)launchers[currentCatcher++];
							sender.Other=catcher;
							catcher.Other=sender;
							
							launchers.Add(catcher);
							elements.Add(catcher);

							//Capture the delete event
							catcher.Delete+=new EventHandler(onLauncherDelete);
							
							launcherLayer.Add(catcher);
							launcherLayer.Add(sender);

							break;
						case MapModes.Sprites:
							data=line.Split(',');

							//Create a new sprite
							SpriteMarker marker=new SpriteMarker(device,spriteTex,sprite,textFont,this);
							marker.X=float.Parse(data[0])*4-spriteTex.Width/2;
							marker.Y=float.Parse(data[1])*4-spriteTex.Height/2;
							marker.Index=int.Parse(data[2]);
							marker.Depth=float.Parse(data[3]);
							
							marker.Delete+=new EventHandler(onSpriteDelete);

							sprites.Add(marker);
							spriteLayer.Add(marker);
							elements.Add(marker);

							if (select) marker.Select();

							break;
					}
				}
			}
		}

		/// <summary>
		/// Removes everything from the map
		/// </summary>
		private void clearMap() {
			fileDialog.FileName="";
			settingsForm.Theme.Text="default.theme";
			settingsForm.Ghost.Text="<None>";
			author=settingsForm.Author.Text="Anonymous";
			theme="";
			ghost="";

			while (nodes.Count!=0)		((Node)nodes[0]).TriggerDelete();
			while (pads.Count!=0)		((Pad)pads[0]).TriggerDelete();
			while (springs.Count!=0)	((Spring)springs[0]).TriggerDelete();
			while (launchers.Count!=0)	((Launcher)launchers[0]).TriggerDelete();
			while (sprites.Count!=0)	((SpriteMarker)sprites[0]).TriggerDelete();
		}

		/// <summary>
		/// Sets the tool in use
		/// </summary>
		/// <param name="tool"></param>
		private void setTool(Tool tool) {
			if (tool==Tool.None) return;	//Skip it; the user is trying to use a window

			//Clear old tool
			if (linking) {
				linking=false;
				previewLine.Visible=false;
			}

			switch(this.tool) {
				case Tool.Pad:
					previewPad.Visible=false;
					cursor.Visible=true;
					break;
				case Tool.Spring:
					previewSpring.Visible=false;
					cursor.Visible=true;
					break;
				case Tool.Launcher:
					previewLauncherFrom.Visible=false;
					previewLauncherTo.Visible=false;
					launcherArrow.Visible=false;
					launcherDown=false;
					cursor.Visible=true;
					break;
				case Tool.Sprite:
					previewMarker.Visible=false;
					cursor.Visible=true;
					break;
			}

			if (previewTri.Visible) {
				resetPreviewTri();
			}

			this.tool=tool;

			if (tool!=Tool.Select) deselectAll();

			switch (tool) {
				case Tool.Select:
					cursor.Frame=0;
					break;
				case Tool.Node:
					cursor.Frame=1;
					break;
				case Tool.Line:
					cursor.Frame=2;
					break;
				case Tool.Polygon:
					cursor.Frame=3;
					break;
				case Tool.Pencil:
					cursor.Frame=7;
					break;
				case Tool.Pad:
					previewPad.Visible=true;
					updatePad(lastMouse.X,lastMouse.Y);
					cursor.Visible=false;
					break;
				case Tool.Spring:
					previewSpring.Visible=true;
					updateSpring(lastMouse.X,lastMouse.Y);
					cursor.Visible=false;
					break;
				case Tool.Launcher:
					previewLauncherFrom.Visible=true;
					updateLauncher(lastMouse.X,lastMouse.Y);
					cursor.Visible=false;
					break;
				case Tool.Sprite:
					previewMarker.Visible=true;
					updateSprite(lastMouse.X,lastMouse.Y);
					cursor.Visible=false;
					break;
			}
		}

		/// <summary>
		/// Clears the preview triangle
		/// </summary>
		private void resetPreviewTri() {
			previewTri.SetPosition(0,Vector2.Empty);
			previewTri.SetPosition(1,Vector2.Empty);
			previewTri.SetPosition(2,Vector2.Empty);
			previewTri.Visible=false;
		}
		#endregion

		#region MapElement event handling

		private void OnNodeDelete(object sender, EventArgs e) {
			nodes.Remove(sender);
			nodeLayer.Remove((TransformedObject)sender);
			elements.Remove(sender);
		}

		private void OnLineDelete(object sender, EventArgs e) {
			lines.Remove(sender);
			lineLayer.Remove((TransformedObject)sender);
			elements.Remove(sender);
		}

		private void OnPadDelete(object sender, EventArgs e) {
			pads.Remove(sender);
			padLayer.Remove((TransformedObject)sender);
			elements.Remove(sender);
		}

		private void OnPolyDelete(object sender, EventArgs e) {
			polys.Remove(sender);
			polyLayer.Remove((TransformedObject)sender);
			elements.Remove(sender);
		}

		private void OnSpringDelete(object sender, EventArgs e) {
			springs.Remove(sender);
			springLayer.Remove((TransformedObject)sender);
			elements.Remove(sender);
		}

		private void onLauncherDelete(object sender, EventArgs e) {
			launchers.Remove(sender);
			launcherLayer.Remove((TransformedObject)sender);
			elements.Remove(sender);
		}

		private void onSpriteDelete(object sender, EventArgs e) {
			sprites.Remove(sender);
			spriteLayer.Remove((TransformedObject)sender);
			elements.Remove(sender);
		}

		#endregion

		#region cursor
		/// <summary>
		/// Hides the windows cursor and makes the SpriteObject cursor visible
		/// </summary>
		private void toSpriteCursor() {
			if (tool!=Tool.Pad && tool!=Tool.Spring && tool!=Tool.Launcher && tool!=Tool.Sprite)
				cursor.Visible=true;	//Make the cursor visible except when using pad tool
			this.Cursor=noCursor;
		}

		/// <summary>
		/// Hides the SpriteObject cursor and shows the windows cursor
		/// </summary>
		private void toWinCursor() {
			cursor.Visible=false;
			this.Cursor=regCursor;
		}
		#endregion

		#region file/stream handling
		/// <summary>
		/// Returns a stream of an embedded resource
		/// </summary>
		private Stream getStream(string resourcePath) {
			return GetType().Module.Assembly.GetManifestResourceStream(resourcePath);
		}

		/// <summary>
		/// Creates a SpriteTexture from a SizeTexture and an embedded datafile
		/// </summary>
		/// <param name="tex">A SizeTexture containing the Direct3D Texture to use</param>
		/// <param name="dataPath">Path to a datafile describing how the SpriteTexture should use the image (see the SpriteTexture documentation for more information)</param>
		/// <returns></returns>
		private SpriteTexture loadTexture (SizeTexture tex,string dataPath) {
			return new SpriteTexture(device,tex.Tex,readFile(dataPath),tex.Width,tex.Height);
		}

		/// <summary>
		/// Creates a new Texture from an embedded resource, wrapped in a SizeTexture that contains the image's dimensions
		/// </summary>
		/// <param name="imgStream">Path to </param>
		/// <returns></returns>
		private SizeTexture loadSizeTexture(String imagePath) {
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
			SizeTexture s=new SizeTexture(TextureLoader.FromStream(device,imgStream,width,height,1,0,Format.A8R8G8B8,Pool.Default,Filter.None,Filter.None,0),width,height);

			//Close the stream and return the SizeTexture
			imgStream.Close();
			return s;
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
		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.fileDialog = new System.Windows.Forms.SaveFileDialog();
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.fileMenu = new System.Windows.Forms.MenuItem();
			this.newCommand = new System.Windows.Forms.MenuItem();
			this.openCommand = new System.Windows.Forms.MenuItem();
			this.saveCommand = new System.Windows.Forms.MenuItem();
			this.saveAsCommand = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.exitCommand = new System.Windows.Forms.MenuItem();
			this.editMenu = new System.Windows.Forms.MenuItem();
			this.copyCommand = new System.Windows.Forms.MenuItem();
			this.pasteCommand = new System.Windows.Forms.MenuItem();
			this.undeleteCommand = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.settingsCommand = new System.Windows.Forms.MenuItem();
			this.helpMenu = new System.Windows.Forms.MenuItem();
			this.aboutCommand = new System.Windows.Forms.MenuItem();
			this.openDialog = new System.Windows.Forms.OpenFileDialog();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.vertFlipCommand = new System.Windows.Forms.MenuItem();
			this.horzFlipCommand = new System.Windows.Forms.MenuItem();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.fileMenu,
																					 this.editMenu,
																					 this.helpMenu});
			// 
			// fileMenu
			// 
			this.fileMenu.Index = 0;
			this.fileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.newCommand,
																					 this.openCommand,
																					 this.saveCommand,
																					 this.saveAsCommand,
																					 this.menuItem2,
																					 this.exitCommand});
			this.fileMenu.Text = "File";
			// 
			// newCommand
			// 
			this.newCommand.Index = 0;
			this.newCommand.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.newCommand.ShowShortcut = false;
			this.newCommand.Text = "New (Ctrl+N)";
			// 
			// openCommand
			// 
			this.openCommand.Index = 1;
			this.openCommand.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.openCommand.ShowShortcut = false;
			this.openCommand.Text = "Open (Ctrl+O)";
			// 
			// saveCommand
			// 
			this.saveCommand.Index = 2;
			this.saveCommand.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.saveCommand.ShowShortcut = false;
			this.saveCommand.Text = "Save (Ctrl+S)";
			// 
			// saveAsCommand
			// 
			this.saveAsCommand.Index = 3;
			this.saveAsCommand.ShowShortcut = false;
			this.saveAsCommand.Text = "Save As";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 4;
			this.menuItem2.ShowShortcut = false;
			this.menuItem2.Text = "-";
			// 
			// exitCommand
			// 
			this.exitCommand.Index = 5;
			this.exitCommand.Shortcut = System.Windows.Forms.Shortcut.CtrlQ;
			this.exitCommand.ShowShortcut = false;
			this.exitCommand.Text = "Exit (Ctrl+Q)";
			// 
			// editMenu
			// 
			this.editMenu.Index = 1;
			this.editMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.copyCommand,
																					 this.pasteCommand,
																					 this.undeleteCommand,
																					 this.menuItem3,
																					 this.vertFlipCommand,
																					 this.horzFlipCommand,
																					 this.menuItem1,
																					 this.settingsCommand});
			this.editMenu.Text = "Edit";
			// 
			// copyCommand
			// 
			this.copyCommand.Index = 0;
			this.copyCommand.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.copyCommand.ShowShortcut = false;
			this.copyCommand.Text = "Copy (Ctrl+C)";
			// 
			// pasteCommand
			// 
			this.pasteCommand.Index = 1;
			this.pasteCommand.ShowShortcut = false;
			this.pasteCommand.Text = "Paste (Ctrl+V)";
			// 
			// undeleteCommand
			// 
			this.undeleteCommand.Index = 2;
			this.undeleteCommand.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
			this.undeleteCommand.ShowShortcut = false;
			this.undeleteCommand.Text = "Undelete (Ctrl+Z)";
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 6;
			this.menuItem1.Text = "-";
			// 
			// settingsCommand
			// 
			this.settingsCommand.Index = 7;
			this.settingsCommand.Text = "Settings...";
			// 
			// helpMenu
			// 
			this.helpMenu.Index = 2;
			this.helpMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.aboutCommand});
			this.helpMenu.Text = "Help";
			// 
			// aboutCommand
			// 
			this.aboutCommand.Index = 0;
			this.aboutCommand.Text = "About";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 3;
			this.menuItem3.Text = "-";
			// 
			// vertFlipCommand
			// 
			this.vertFlipCommand.Index = 4;
			this.vertFlipCommand.Text = "Flip Vertical";
			// 
			// horzFlipCommand
			// 
			this.horzFlipCommand.Index = 5;
			this.horzFlipCommand.Text = "Flip Horizontal";
			// 
			// Editor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(792, 553);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Menu = this.mainMenu;
			this.Name = "Editor";
			this.Text = "HokiEdit";

		}
		#endregion

		#region construct/destruct
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

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			using (Editor editor=new Editor()) {
				editor.Show();
				editor.InitializeGraphics();
				Application.Run(editor);
			}
		}
		#endregion

		#region menu events
		

		private void onVertFlip(object sender, EventArgs e) {
			flip(false);
		}

		private void onHorzFlip(object sender, EventArgs e) {
			flip(true);
		}

		private void flip(bool horizontal) {
			//Loop one: find the bounds
			float min=float.PositiveInfinity,max=float.NegativeInfinity;
			bool usable=false;
			foreach (Node n in nodes) {
				if (n.Selected()) {
					usable=true;	//There's at least one node, so we can use this
					min=Math.Min(min,(horizontal?n.X:n.Y));
					max=Math.Max(max,(horizontal?n.X:n.Y));
				}
			}

			if (!usable) return;

			//Loop two: flip
			foreach (Node n in nodes) {
				if (n.Selected()) {
					if (horizontal)	n.X=min+max-n.X;
					else			n.Y=min+max-n.Y;
				}
			}
		}

		private void onNew(object sender, EventArgs e) {
			clearMap();
		}

		private void onSave(object sender, EventArgs e) {
			if (fileDialog.FileName.Length>0) {
				//Compile the map
				String map=compile();

				//Write it to a textfile
				try {
					StreamWriter sw=new StreamWriter(fileDialog.FileName);
					sw.Write(map);
					sw.Close();	
				} catch (Exception ex) {
					System.Diagnostics.Trace.WriteLine("FILE ERROR - Map not saved");	//TODO: visible error report
				}
			}
			else fileDialog.ShowDialog(this);
		}

		private void onSaveAs(object sender, EventArgs e) {
			fileDialog.ShowDialog(this);
		}

		private void onOpen(object sender, EventArgs e) {
			openDialog.ShowDialog(this);
			ctrl=false;
		}

		private void onCopy(object sender, EventArgs e) {
			clipboard=compile(true);
		}

		private void onPaste(object sender, EventArgs e) {
			//Determine which offsets to use
			int xOff,yOff;
			if (sender is System.Windows.Forms.MenuItem) {	//If selected from the menu, use the top right corner of the screen
				xOff=(int) 4;
				yOff=(int) 0;
			} else {				//Otherwise, use the mouse position
				Vector2 m=workspaceCoord((int)lastMouse.X,(int)lastMouse.Y);
				xOff=(int)(m.X/8);
				yOff=(int)(m.Y/8);
			}

			//Paste
			constructElements(clipboard,xOff,yOff,true);
		}

		private void onAbout(object sender, EventArgs e) {
			aboutWin.Visible=true;
			aboutWin.Unlock();
			tool=toolSave;
			tool=Tool.None;
		}

		private void onOpenFile(object sender, CancelEventArgs e) {
			//Clear the existing map
			clearMap();

			//Clear the settings
			settingsForm.Ghost.Text="default.theme";
			settingsForm.Ghost.Text="<None>";
			
			//Load a new map
			try {
				//Read the file specified
				StreamReader sr=new StreamReader(openDialog.FileName);
				constructElements(sr.ReadToEnd(),0,0,false);
				sr.Close();

				//Store the name of the file being operated on so the user doesn't have to save-as
				fileDialog.FileName=openDialog.FileName;
			} catch (IOException ex) {
				System.Diagnostics.Trace.WriteLine("FILE ERROR - map not loaded - "+ex.Message);	//TODO: visible error report
			}

			//Recenter
			center(0,0);
		}

		private void onSettings(object sender, EventArgs e) {
			settingsForm.Show();
		}

		private void onUndelete(object sender, EventArgs e) {
			clearMap();
			constructElements(undelete,0,0,false,true);
			center(deleteX,deleteY);
		}
		
		private void onExit(object sender, EventArgs e) {
			Application.Exit();
		}
		#endregion
		
		/// <summary>
		/// Different states the map parsing method can be in. These determine how it interprets the information in the file.
		/// </summary>
		private enum MapModes {
			Meta,
			Nodes,
			Walls,
			Polygons,
			Pads,
			Springs,
			Launchers,
			Catchers,
			Sprites
		}

		#region SpriteWindow element event handlers
		private void onLauncherOk(object sender, MouseEventArgs e) {
			launcherWin.Visible=false;
			launcherWin.Lock();			//Lock the window to prevent accidental use of its interface
			tool=toolSave;
		}

		private void onFrequencyChange(object sender, EventArgs e) {
			frequencyText.Text="Frequency: "+(int)(frequencySlider.Value*100)+"%";
			editLauncher.Frequency=frequencySlider.Value;
		}

		private void onOffsetChange(object sender, EventArgs e) {
			offsetText.Text="Offset: "+(int)(offsetSlider.Value*100)+"%";
			editLauncher.Offset=offsetSlider.Value;
		}

		private void onAboutOk(object sender, MouseEventArgs e) {
			aboutWin.Visible=false;
			aboutWin.Lock();
			tool=toolSave;
		}
		#endregion

		private void OkButton_Click(object sender, EventArgs e) {
			author=settingsForm.Author.Text;
			ghost=settingsForm.Ghost.Text=="<None>" ? "" : settingsForm.Ghost.Text;
			theme=settingsForm.Theme.Text=="default.theme" ? "" : settingsForm.Theme.Text;
			settingsForm.Hide();
		}

		private void CancelButton_Click(object sender, EventArgs e) {
			settingsForm.Author.Text=author;
			settingsForm.Ghost.Text=ghost=="" ? "<None>" : ghost;
			settingsForm.Theme.Text=theme=="" ? "default.theme" : theme;
			settingsForm.Hide();
		}
	}
}