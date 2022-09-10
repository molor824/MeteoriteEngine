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
        new vec3[]
        {
            new(-0.5f, 0.5f, 0),
            new(0.5f, 0.5f, 0),
            new(0.5f, -0.5f, 0),
            new(-0.5f, -0.5f, 0),
        },
        new ushort[] { 0, 3, 2, 2, 1, 0 },
        new vec2[]
        {
            new(0, 0),
            new(1, 0),
            new(1, 1),
            new(0, 1)
        }
    );
    static Shader _shader = Shader.Default;

    public Texture? Texture = null;
    public Color Color = Color.White;

    public override void Render(float delta)
    {
        var texture = Texture ?? MeshRenderer.DefaultTexture;
        var oldScale = Scale;

        Scale *= texture.Size / texture.PixelsPerUnit;

        var projection = Game.MainCamera.ProjectionMatrix;
        var camTransform = Game.MainCamera.GlobalTransformMatrix;
        var globalTransform = GlobalTransformMatrix;
        
        var transform = projection * camTransform.Inverse * globalTransform;

        GL.Disable(EnableCap.CullFace);

        GL.Uniform4(_shader.GetUniformLocation("textureColor"), Color.R, Color.G, Color.B, Color.A);
        _shader.SetMat4("transform", false, transform);
        
        GL.UseProgram(_shader.Program);
        texture.Bind();
        _quad.Render();
        
        Scale = oldScale;

        base.Render(delta);
    }
}