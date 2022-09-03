using OpenTK.Graphics.OpenGL;

namespace Meteorite;

/// <summary>
/// 2D sprite with texture and color.
/// Position Z index acts as depth.
/// Sprite acts as quad mesh and renders just as same as 3D.
/// </summary>
public class Sprite : Transform2D
{
    static Mesh? _quad;
    static Texture? _default;

    public Texture? Texture = null;
    public Color Color = Color.White;

    public override void Added()
    {
        _quad ??= new(
            new Vertex[]
            {
                new(new(-0.5f, 0.5f, 0), new(0, 0)),
                new(new(0.5f, 0.5f, 0), new(1, 0)),
                new(new(0.5f, -0.5f, 0), new(1, 1)),
                new(new(-0.5f, -0.5f, 0), new(0, 1)),
            },
            new ushort[] { 0, 3, 2, 2, 1, 0 }
        );
        _default ??= new(new Color[] { Color.White }, 1, 1);

        base.Added();
    }
    public override void Render(float delta)
    {
        var texture = Texture ?? _default;
        var oldScale = Scale;

        Scale *= texture!.Size / texture.PixelsPerUnit;

        var transform = Game.MainCamera.WorldToCamera * GlobalTransformMatrix;
        
        texture.Bind();
        GL.Uniform4(_quad!.Shader.GetUniformLocation("textureColor"), Color.R, Color.G, Color.B, Color.A);
        _quad.Render(transform);

        Scale = oldScale;

        base.Render(delta);
    }
}