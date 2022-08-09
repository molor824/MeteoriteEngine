namespace Meteorite;

using System.Runtime.InteropServices;

public class Material
{
    internal Raylib_cs.Material Raw = Raylib.LoadMaterialDefault();

    public void SetTexture(Texture texture)
    {
        unsafe
        {
            var map = Raw.maps[0];

            map.texture = texture.Raw;

            Raw.maps[0] = map;
        }
    }
    public void SetColor(BColor color)
    {
        unsafe
        {
            var map = Raw.maps[0];

            map.color = (Raylib_cs.Color)color;

            Raw.maps[0] = map;
        }
    }
    public void SetColor(Color color)
    {
        unsafe
        {
            var map = Raw.maps[0];

            map.color = (Raylib_cs.Color)color;

            Raw.maps[0] = map;
        }
    }

    public Material() { }
    public Material(Texture texture)
    {
        MaterialInit(texture, new());
    }
    public Material(Texture texture, BColor color)
    {
        MaterialInit(texture, (Raylib_cs.Color)color);
    }
    public Material(Texture texture, Color color)
    {
        MaterialInit(texture, (Raylib_cs.Color)color);
    }
    void MaterialInit(Texture texture, Raylib_cs.Color color)
    {
        unsafe
        {
            var map = Raw.maps[0];

            map.texture = texture.Raw;
            map.color = color;

            Raw.maps[0] = map;
        }
    }
}