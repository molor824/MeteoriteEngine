using OpenTK.Graphics.OpenGL;

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

    public override void Render(float delta)
    {
        var texture = Texture ?? Texture.Default;
        var oldScale = Scale;

        Scale *= texture.Size / texture.PixelsPerUnit;

        var projection = Game.MainCamera.ProjectionMatrix;
        var camTransform = Game.MainCamera.GlobalTransformMatrix;
        var globalTransform = GlobalTransformMatrix;

        var transform = projection * camTransform.Inverse * globalTransform;


        Shader.Default.SetVec4(DefaultShader.TintLocation, Tint);
        Shader.Default.SetMat4(DefaultShader.TransformLocation, false, transform);
        Shader.Default.Use();

        texture.Bind();
        
        RenderOptions.SetRenderOptions();
        PrimitiveMesh.Quad.Render();

        Scale = oldScale;
    }
}