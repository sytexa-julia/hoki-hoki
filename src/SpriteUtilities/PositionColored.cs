using SharpDX.Direct3D9;
using SharpDX;
using System.Globalization;
using System.Reflection;
using System.Text;
using System;
using System.Runtime.InteropServices;

public struct PositionColored
{
    public float X;
    public float Y;
    public float Z;
    public int Color;
    public const VertexFormat Format = VertexFormat.Diffuse | VertexFormat.Position;


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

    public PositionColored(Vector3 value, int c)
    {
        this.X = value.X;
        this.Y = value.Y;
        this.Z = value.Z;
        this.Color = c;
    }

    public PositionColored(float xvalue, float yvalue, float zvalue, int c)
    {
        this.X = xvalue;
        this.Y = yvalue;
        this.Z = zvalue;
        this.Color = c;
    }

    public static int StrideSize => Marshal.SizeOf<PositionColored>();
}
