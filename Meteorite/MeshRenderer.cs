namespace Meteorite;

public class MeshRenderer : Transform3D
{
    public Mesh? Mesh;
    public Texture? Texture;
    public Color Tint = Color.White;
    public RenderOptions RenderOptions;

    internal static int TransformLocation = DefaultShaders.Shader3D.GetUniformLocation("transform");
    internal static int TintLocation = DefaultShaders.Shader3D.GetUniformLocation("tint");

    public MeshRenderer() { }
    public MeshRenderer(Mesh mesh)
    {
        Mesh = mesh;
    }

    public override void Render(float delta)
    {
        if (Mesh == null) return;

        var texture = Texture ?? Texture.Default;

        var projection = Game.MainCamera3D.ProjectionMatrix;
        var camTransform = Game.MainCamera3D.GlobalTransform;
        var globalTransform = GlobalTransform;

        var transform = projection * camTransform.Inverse() * globalTransform;

        DefaultShaders.Shader3D.SetVec4(TransformLocation, Tint);
        DefaultShaders.Shader3D.SetMat4(TintLocation, true, transform);
        DefaultShaders.Shader3D.Use();
        
        texture.Bind();
        
        RenderOptions.SetRenderOptions();
        Mesh.Render();
    }
}