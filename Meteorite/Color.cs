namespace Meteorite;

public struct Color
{
    public float R, G, B, A;

    public Color White => new(1, 1, 1);
    public Color Black => new(0, 0, 0);
    public Color Red => new(1, 0, 0);
    public Color Green => new(0, 1, 0);
    public Color Blue => new(0, 0, 1);
    public Color Yellow => new(1, 1, 0);
    public Color Cyan => new(0, 1, 1);
    public Color Purple => new(1, 0, 1);

    public static Color FromByte(byte r, byte g, byte b, byte a = 255)
    {
        return new(
            (float)glm.Clamp(r, 0, 255) / 255,
            (float)glm.Clamp(g, 0, 255) / 255,
            (float)glm.Clamp(b, 0, 255) / 255,
            (float)glm.Clamp(a, 0, 255) / 255
        );
    }
    public Color(float r, float g, float b, float a = 1)
    {
        R = glm.Clamp(r, 0, 1);
        G = glm.Clamp(g, 0, 1);
        A = glm.Clamp(a, 0, 1);
        B = glm.Clamp(b, 0, 1);
    }
    public Color(vec2 rg, float b, float a = 1)
    {
        rg = glm.Clamp(rg, vec2.Zero, vec2.Ones);
        R = rg.x;
        G = rg.y;
        B = glm.Clamp(b, 0, 1);
        A = glm.Clamp(a, 0, 1);
    }
    public Color(vec3 rgb, float a = 1)
    {
        rgb = glm.Clamp(rgb, vec3.Zero, vec3.Ones);
        R = rgb.x;
        G = rgb.y;
        B = rgb.z;
        A = glm.Clamp(a, 0, 1);
    }
    public Color(vec4 rgba)
    {
        rgba = glm.Clamp(rgba, vec4.Zero, vec4.Ones);
        R = rgba.x;
        G = rgba.y;
        B = rgba.z;
        A = rgba.w;
    }
    public static explicit operator BColor(Color a) => new()
    {
        R = (byte)(a.R * 255),
        G = (byte)(a.G * 255),
        A = (byte)(a.A * 255),
        B = (byte)(a.B * 255),
    };
    public static explicit operator Raylib_cs.Color(Color a) => new(
        (byte)(a.R * 255),
        (byte)(a.G * 255),
        (byte)(a.B * 255),
        (byte)(a.A * 255)
    );
    public static explicit operator Color(Raylib_cs.Color a) => new(
        (float)a.r / 255,
        (float)a.g / 255,
        (float)a.b / 255,
        (float)a.a / 255
    );
    public static explicit operator vec4(Color a) => new(a.R, a.G, a.B, a.A);
    public static explicit operator vec3(Color a) => new(a.R, a.G, a.B);
    public static explicit operator Color(vec4 a) => new(a);
    public static explicit operator Color(vec3 a) => new(a);
    public override string ToString()
    {
        return $"RGBA({R:0.00}, {G:0.00}, {B:0.00}, {A:0.00})";
    }
}
public struct BColor
{
    public byte R, G, B, A;

    public Color FromFloat(float r, float g, float b, float a = 1)
    {
        return new(
            (byte)(glm.Clamp(r, 0, 1) * 255),
            (byte)(glm.Clamp(g, 0, 1) * 255),
            (byte)(glm.Clamp(b, 0, 1) * 255),
            (byte)(glm.Clamp(a, 0, 1) * 255)
        );
    }
    public BColor(byte r, byte g, byte b, byte a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }
    public BColor(ivec2 rg, byte b, byte a = 255)
    {
        rg = glm.Clamp(rg, ivec2.Zero, new(255));
        R = (byte)rg.x;
        G = (byte)rg.y;
        B = b;
        A = a;
    }
    public BColor(ivec3 rgb, byte a = 255)
    {
        rgb = glm.Clamp(rgb, ivec3.Zero, new(255));
        R = (byte)rgb.x;
        G = (byte)rgb.y;
        B = (byte)rgb.z;
        A = a;
    }
    public BColor(ivec4 rgba)
    {
        rgba = glm.Clamp(rgba, ivec4.Zero, new(255));
        R = (byte)rgba.x;
        G = (byte)rgba.y;
        B = (byte)rgba.z;
        A = (byte)rgba.w;
    }
    public static explicit operator Color(BColor a) => new()
    {
        R = (float)a.R / 255,
        G = (float)a.G / 255,
        A = (float)a.A / 255,
        B = (float)a.B / 255,
    };
    public static explicit operator Raylib_cs.Color(BColor a) => new(a.R, a.G, a.B, a.A);
    public static explicit operator BColor(Raylib_cs.Color a) => new(a.r, a.g, a.b, a.a);
    public static explicit operator ivec3(BColor a) => new(a.R, a.G, a.B);
    public static explicit operator ivec4(BColor a) => new(a.R, a.G, a.B, a.A);
    public static explicit operator BColor(ivec3 a) => new(a);
    public static explicit operator BColor(ivec4 a) => new(a);
    public override string ToString()
    {
        return $"RGBA({R}, {G}, {B}, {A})";
    }
}