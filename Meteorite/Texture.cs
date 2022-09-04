using System.Reflection;

namespace Meteorite;

using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Runtime.CompilerServices;
using System;

public class Texture
{
    public vec2 Size => new(_width, _height);
    public int Width => _width;
    public int Height => _height;
    public float PixelsPerUnit = 1;

    int _handle;
    int _width, _height;

    public Texture(string path, TextureParameters? tParams = null)
    {
        var image = Image.Load<RgbaVector>(ResourceLoader.LoadFile(path));

        Upload(image, tParams ?? new());
    }
    public Texture(IEnumerable<Color> pixels, int width, int height, TextureParameters? tParams = null)
    {
        var image = Image.LoadPixelData(pixels.Select(
            p => new RgbaVector(
                p.R,
                p.G,
                p.B,
                p.A
            )
        ).ToArray(), width, height);

        Upload(image, tParams ?? new());
    }
    void Upload(Image<RgbaVector> image, TextureParameters tParams)
    {
        _width = image.Width;
        _height = image.Height;
        
        var pixels = new RgbaVector[_width * _height];
        image.CopyPixelDataTo(pixels);

        _handle = GL.GenTexture();
        Bind();

		GL.TexParameter(
			TextureTarget.Texture2D,
			TextureParameterName.TextureMinFilter,
            (int)tParams.MinFilter
        );
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMagFilter,
            (int)tParams.MagFilter
        );
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureWrapS,
            (int)tParams.WrapMode
        );
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureWrapT,
            (int)tParams.WrapMode
        );
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureBorderColor,
            new[]
            {
                tParams.BorderColor.R,
                tParams.BorderColor.G,
                tParams.BorderColor.B,
                tParams.BorderColor.A
            }
        );

        GL.TexImage2D(
            TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
            _width, _height, 0,
            PixelFormat.Rgba, PixelType.Float, pixels
        );
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        Log.Success("[ID: {0}] Texture loaded!", _handle);
    }

    ~Texture()
    {
        Unload();
    }

    void Unload()
    {
        GL.DeleteTexture(_handle);
    }
    internal void Bind()
    {
        GL.BindTexture(TextureTarget.Texture2D, _handle);
    }
}

public struct TextureParameters
{
    public TextureMinFilter MinFilter = TextureMinFilter.NearestMipmapLinear;
    public TextureMagFilter MagFilter = TextureMagFilter.Linear;
    public TextureWrapMode WrapMode = TextureWrapMode.Repeat;
    public Color BorderColor = Color.Black;

    public TextureParameters() { }
}
