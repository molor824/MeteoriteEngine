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

    public Texture(string path)
    {
        var asmPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var image = Image.Load<RgbaVector>(Path.Join(asmPath, path));

        Upload(image);
    }
    public Texture(Color[] pixels, int width, int height)
    {
        var image = Image.LoadPixelData<RgbaVector>(Unsafe.As<RgbaVector[]>(pixels), width, height);

        Upload(image);
    }
    public void Upload(
        Image<RgbaVector> image,
        TextureMinFilter minFilter = TextureMinFilter.NearestMipmapLinear,
        TextureMagFilter magFilter = TextureMagFilter.Linear,
        TextureWrapMode wrapS = TextureWrapMode.Repeat,
        TextureWrapMode wrapT = TextureWrapMode.Repeat,
        Color? borderColor = null
    )
    {
        var pixels = new RgbaVector[image.Width * image.Height];
        image.CopyPixelDataTo(pixels);

		GL.TexParameter(
			TextureTarget.Texture2D,
			TextureParameterName.TextureMinFilter,
            (int)minFilter
        );
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureMagFilter,
            (int)magFilter
        );
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureWrapS,
            (int)wrapS
        );
        GL.TexParameter(
            TextureTarget.Texture2D,
            TextureParameterName.TextureWrapT,
            (int)wrapT
        );

        if (borderColor is Color col)
        {
            GL.TexParameter(
                TextureTarget.Texture2D,
                TextureParameterName.TextureBorderColor,
                new float[] { col.R, col.G, col.B, col.A }
            );
        }

        GL.TexImage2D(
            TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
            image.Width, image.Height, 0,
            PixelFormat.Rgba, PixelType.Float, pixels
        );

        _handle = GL.GenTexture();
        
        Log.Print("[ID: {0}] Succesfully loaded texture!", _handle);
    }
    internal void Bind(TextureUnit unit = TextureUnit.Texture0)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, _handle);
    }
}
