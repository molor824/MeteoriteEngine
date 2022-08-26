namespace Meteorite;

using SixLabors.ImageSharp.PixelFormats;

public struct Color
{
    public static Color Black => new();
    public static Color White => new(1, 1, 1);
    public static Color Red => new(1, 0, 0);
    public static Color Green => new(0, 1, 0);
    public static Color Blue => new(0, 0, 1);
    public static Color Yellow => new(1, 1, 0);
    public static Color Cyan => new(0, 1, 1);
    public static Color Pink => new(1, 0, 1);

    public float R = 0, G = 0, B = 0, A = 1;

	public Color() { }
	public Color(float r = 0, float g = 0, float b = 0, float a = 1)
	{
		R = r;
		G = g;
		B = b;
		A = a;
	}
    public static Color FromByte(byte r = 0, byte g = 0, byte b = 0, byte a = 255)
    {
        return new()
        {
            R = (float)r / 255,
            G = (float)g / 255,
            B = (float)b / 255,
            A = (float)a / 255,
        };
	}
	public Color(vec2 rg, float b = 0, float a = 1)
	{
		R = rg.x;
		G = rg.y;
		B = b;
		A = a;
	}
	public Color(vec3 rgb, float a = 1)
	{
		R = rgb.x;
		G = rgb.y;
		B = rgb.z;
		A = a;
	}
	public Color(vec4 rgba)
	{
		R = rgba.x;
		G = rgba.y;
		B = rgba.z;
		A = rgba.w;
	}
    public static implicit operator Rgba32(Color v) => new(v.R, v.G, v.B, v.A);
}