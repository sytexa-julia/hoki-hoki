using System;

namespace Hierarchy {
	/// <summary>
	/// Provides access to resources for samples
	/// </summary>
	public class Resources {
		/// <summary>
		/// Creates a new Texture from an embedded resource, wrapped in a Tex that contains the image's dimensions
		/// </summary>
		/// <param name="imgStream">Path to the resource</param>
		public static Tex loadTex(String imagePath) {
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

		/// <summary>
		/// Loads an embedded resource stream from the assembly
		/// </summary>
		/// <param name="resourcePath">Path of the resource</param>
		/// <returns>An embedded resource</returns>
		private Stream getStream(string resourcePath) {
			return GetType().Module.Assembly.GetManifestResourceStream(resourcePath);
		}
	}
}