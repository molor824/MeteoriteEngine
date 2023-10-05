using OpenTK.Graphics.OpenGL;
using Meteorite.Mathematics;

namespace Meteorite;

/// <summary>
/// 2D sprite with texture and color.
/// Position Z index acts as depth.
/// Sprite acts as quad mesh and renders just as same as 3D.
/// </summary>
public class SpriteRenderer : Transform2D
{
    public Texture? Texture;
    public Color Tint = Color.White;

    public RenderOptions RenderOptions = new()
    {
        CullFace = false
    };

    internal static int TransformLocation = DefaultShaders.Shader2D.GetUniformLocation("transform");
    internal static int TintLocation = DefaultShaders.Shader2D.GetUniformLocation("tint");

    public override void Render(float delta)
    {
        var texture = Texture ?? Texture.Default;

        var projection = Game.MainCamera2D.ProjectionMatrix;
        var camTransform = Game.MainCamera2D.GlobalTransform;
        var globalTransform = GlobalTransform;

        var transform = projection * camTransform.Inverse() * globalTransform;

        DefaultShaders.Shader2D.SetMat4(TransformLocation, true, transform);
        DefaultShaders.Shader2D.SetVec4(TintLocation, Tint);
        DefaultShaders.Shader2D.Use();

        texture.Bind();

        RenderOptions.SetRenderOptions();
        PrimitiveMesh.Quad.Render();
    }
}