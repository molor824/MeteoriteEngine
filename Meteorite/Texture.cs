namespace Meteorite;

using System.Runtime.InteropServices;

public class Texture : IDisposable
{
    public vec2 Size => new(Raw.width, Raw.height);
    public float PixelsPerUnit;
    internal Raylib_cs.Texture2D Raw;

    void IDisposable.Dispose()
    {
        Unload();
    }
    public void Unload()
    {
        if (Raw.id == 0) return;

        Raylib.UnloadTexture(Raw);
        Raw.id = 0;
    }
    internal Texture() { }
    public Texture(int width, int height, Color[] pixels)
    {
        unsafe
        {
            fixed (void* ptr = pixels)
            {
                TextureInit(width, height, ptr, 1, PixelFormat.PIXELFORMAT_UNCOMPRESSED_R32G32B32A32);
            }
        }
    }
    public Texture(int width, int height, BColor[] pixels)
    {
        unsafe
        {
            fixed (void* ptr = pixels)
            {
                TextureInit(width, height, ptr, 1, PixelFormat.PIXELFORMAT_UNCOMPRESSED_R8G8B8A8);
            }
        }
    }
    public Texture(string path)
    {
        Raw = Raylib.LoadTexture(path);
    }
    unsafe void TextureInit(int width, int height, void* data, int mipmaps, PixelFormat format)
    {
        var image = new Raylib_cs.Image()
        {
            width = width,
            height = height,
            data = data,
            mipmaps = mipmaps,
            format = format,
        };
        Raw = Raylib.LoadTextureFromImage(image);
    }
}