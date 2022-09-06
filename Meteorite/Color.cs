namespace Meteorite;

public struct Color
{
    public static Color Black => new(0, 0, 0);
    public static Color Gray => new(0.5f, 0.5f, 0.5f);
    public static Color DarkGray => new(0.25f, 0.25f, 0.25f);
    public static Color LightGray => new(0.75f, 0.75f, 0.75f);
    public static Color White => new(1, 1, 1);
    public static Color Red => new(1, 0, 0);
    public static Color Green => new(0, 1, 0);
    public static Color Blue => new(0, 0, 1);
    public static Color Yellow => new(1, 1, 0);
    public static Color Cyan => new(0, 1, 1);
    public static Color Pink => new(1, 0, 1);
    
    public float R, G, B, A;

    public Color()
    {
        R = G = B = 0;
        A = 1;
    }

    public Color(float r, float g, float b, float a = 1)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public Color(vec2 rg, float b, float a = 1)
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