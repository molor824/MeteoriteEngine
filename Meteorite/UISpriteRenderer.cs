namespace Meteorite;

public class UISpriteRenderer : UITransform
{
    public Texture? Texture;
    public Color Tint = Color.White;
    public RenderOptions RenderOptions = new()
    {
        CullFace = false
    };

    public override void UIRender(float delta)
    {
        var texture = Texture ?? Texture.Default;

        Shader.Default.SetVec4(DefaultShader.TintLocation, Tint);
        Shader.Default.SetMat4(DefaultShader.TransformLocation, false, GlobalTransformMatrix);
        Shader.Default.Use();

        texture.Bind();

        RenderOptions.DepthTest = false;
        RenderOptions.SetRenderOptions();
        PrimitiveMesh.UIQuad.Render();
    }
}