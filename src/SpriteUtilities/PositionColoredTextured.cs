using SharpDX.Direct3D9;
using SharpDX;
using System.Globalization;
using System.Reflection;
using System.Text;
using System;
using System.Runtime.InteropServices;

public struct PositionColoredTextured
{
    public float X;
    public float Y;
    public float Z;
    public int Color;
    public float Tu;
    public float Tv;
    public const VertexFormat Format = VertexFormat.Diffuse | VertexFormat.Position | VertexFormat.Texture1;

    public Vector3 Position
    {
        get
        {
            Vector3 position = new Vector3(this.X, this.Y, this.Z);
            return position;
        }
        set
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Z = value.Z;
        }
    }

    public PositionColoredTextured(Vector3 value, int c, float u, float v)
    {
        this.X = value.X;
        this.Y = value.Y;
        this.Z = value.Z;
        this.Color = c;
        this.Tu = u;
        this.Tv = v;
    }

    public PositionColoredTextured(
      float xvalue,
      float yvalue,
      float zvalue,
      int c,
      float u,
      float v)
    {
        this.X = xvalue;
        this.Y = yvalue;
        this.Z = zvalue;
        this.Color = c;
        this.Tu = u;
        this.Tv = v;
    }

    public static int StrideSize => Marshal.SizeOf<PositionColoredTextured>();
}