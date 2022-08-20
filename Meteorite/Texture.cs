namespace Meteorite;

using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

public class Texture
{
	Image<Rgba32> _image;

	public Texture(string path)
	{
		_image = Image.Load<Rgba32>(path);
		_image.Mutate(x => x.Flip(FlipMode.Vertical));

		var pixels = new byte[_image.Width * _image.Height];
		_image.CopyPixelDataTo(pixels);

		GL.TexParameter(
			TextureTarget.Texture2D,
			TextureParameterName.TextureMinFilter,
			(int)TextureMinFilter.Nearest
		);
	}
}
