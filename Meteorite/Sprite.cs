using OpenTK.Graphics.OpenGL;

namespace Meteorite;

/// <summary>
/// 2D sprite with texture and color.
/// Position Z index acts as depth.
/// Sprite acts as quad mesh and renders just as same as 3D.
/// </summary>
public class Sprite : Transform2D
{
    static Mesh _quad = new(
        new Vertex[]
        {
            new(new(-0.5f, 0.5f, 0), new(0, 0)),
            new(new(0.5f, 0.5f, 0), new(1, 0)),
            new(new(0.5f, -0.5f, 0), new(1, 1)),
            new(new(-0.5f, -0.5f, 0), new(0, 1)),
        },
        new ushort[] { 0, 3, 2, 2, 1, 0 }
    );
    static Texture _default = new(new[] { Color4.White }, 1, 1);
    static Shader _shader = Shader.Default;

    public Texture? Texture = null;
    public Color4 Color = Color4.White;

    public override void Render(float delta)
    {
        var texture = Texture ?? _default;
        var oldScale = Scale;

        Scale *= texture.Size / texture.PixelsPerUnit;

        var projection = Game.MainCamera.ProjectionMatrix;
        var camTransform = Game.MainCamera.GlobalTransformMatrix;
        var globalTransform = GlobalTransformMatrix;
        
        camTransform.Invert();

        var transform = projection * camTransform * globalTransform;
        
        GL.Uniform4(_shader.GetUniformLocation("textureColor"), Color);
        _shader.SetMat4("transform", true, ref transform);
        
        GL.UseProgram(_shader.Program);
        texture.Bind();
        _quad.Render();
        
        Scale = oldScale;

        base.Render(delta);
    }
}