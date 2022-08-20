namespace Meteorite;

public struct Color
{
	public float R, G, B, A = 1;

	public Color() { }
	public Color(float r = 0, float g = 0, float b = 0, float a = 1)
	{
		R = r;
		G = g;
		B = b;
		A = a;
	}
	public Color(byte r = 0, byte g = 0, byte b = 0, byte a = 255)
	{
		R = (float)r / 255;
		G = (float)g / 255;
		B = (float)b / 255;
		A = (float)a / 255;
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
}