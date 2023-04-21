using OpenTK.Graphics.OpenGL;

namespace Meteorite;

public class MeshRenderer : Transform
{
    public Mesh? Mesh;
    public Texture? Texture;
    public Color Tint = Color.White;
    public RenderOptions RenderOptions;
    
    public MeshRenderer() { }
    public MeshRenderer(Mesh mesh)
    {
        Mesh = mesh;
    }

    public override void Render(float delta)
    {
        if (Mesh == null) return;

        var texture = Texture ?? Texture.Default;

        var projection = Game.MainCamera.ProjectionMatrix;
        var camTransform = Game.MainCamera.GlobalTransformMatrix;
        var globalTransform = GlobalTransformMatrix;

        var transform = projection * camTransform.Inverse * globalTransform;

        Shader.Default.SetVec4(DefaultShader.TintLocation, Tint);
        Shader.Default.SetMat4(DefaultShader.TransformLocation, false, transform);
        Shader.Default.Use();
        
        texture.Bind();
        
        RenderOptions.SetRenderOptions();
        Mesh.Render();
    }
}