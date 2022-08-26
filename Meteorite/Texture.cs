namespace Meteorite;

using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Runtime.CompilerServices;

public class Texture
{
    public vec2 Size => new(_image.Width, _image.Height);
    public float PixelsPerUnit = 1;

    Image<Rgba32> _image;
    int _handle;

    public Texture(string path)
	{
		_image = Image.Load<Rgba32>(path);
		_image.Mutate(x => x.Flip(FlipMode.Vertical));
    }
    public Texture(Color[] pixels, int width, int height)
    {
        _image = Image.LoadPixelData<Rgba32>(Unsafe.As<Rgba32[]>(pixels), width, height);
    }
    void Upload(
        TextureMinFilter minFilter = TextureMinFilter.NearestMipmapLinear,
        TextureMagFilter magFilter = TextureMagFilter.Linear,
        TextureWrapMode wrapS = TextureWrapMode.Repeat,
        TextureWrapMode wrapT = TextureWrapMode.Repeat,
        Color? borderColor = null
    )
    {
        var pixels = new Rgba32[_image.Width * _image.Height];
		_image.CopyPixelDataTo(pixels);

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
            _image.Width, _image.Height, 0,
            PixelFormat.Rgba, PixelType.Float, pixels
        );

        _handle = GL.GenTexture();
    }
    internal void Bind(TextureUnit unit = TextureUnit.Texture0)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, _handle);
    }
}
