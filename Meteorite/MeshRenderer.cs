using OpenTK.Graphics.OpenGL;

namespace Meteorite;

public class MeshRenderer : Transform
{
    Shader _shader = Shader.Default;

    public Mesh? Mesh;
    public Texture? Texture;
    public Color Tint = Color.White;

    internal static Texture DefaultTexture = new(new[] { Color.White }, 1, 1, new()
    {
        MagFilter = TextureMagFilter.Nearest,
        MinFilter = TextureMinFilter.Nearest,
    });

    public MeshRenderer() { }
    public MeshRenderer(Mesh mesh)
    {
        Mesh = mesh;
    }

    public override void Render(float delta)
    {
        base.Render(delta);

        if (Mesh == null) return;

        var texture = Texture ?? DefaultTexture;

        var projection = Game.MainCamera.ProjectionMatrix;
        var camTransform = Game.MainCamera.GlobalTransformMatrix;
        var globalTransform = GlobalTransformMatrix;

        var transform = projection * camTransform.Inverse * globalTransform;

        GL.Enable(EnableCap.CullFace);

        GL.Uniform4(_shader.GetUniformLocation("textureColor"), Tint.R, Tint.G, Tint.B, Tint.A);
        _shader.SetMat4("transform", false, transform);

        _shader.UseProgram();
        texture.Bind();
        Mesh.Render();
    }
}