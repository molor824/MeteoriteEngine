using OpenTK.Graphics.OpenGL;

namespace Meteorite;

public class Camera : Transform
{
    public CameraProjection Projection = CameraProjection.Orthographic;
    public float Fovy = 90;
    public float Size = 10;
    public float Near = -1000, Far = 1000;
    /// <summary>
    /// Projection matrix updates every render frame instead of everytime when property getter is called
    /// </summary>
    public Matrix4 ProjectionMatrix => _projectionMatrix;

    private Matrix4 _projectionMatrix = Matrix4.Identity;

    public override void Render(float delta)
    {
        var aspect = (float)Game.Width / Game.Height;
        
        _projectionMatrix = Matrix4.Identity;

        if (Projection == CameraProjection.Perspective)
        {
            var f = MathF.Tan(Fovy / 2f);

            _projectionMatrix.M44 = 0;
            _projectionMatrix.M43 = -1;
            _projectionMatrix.M11 = 1f / (aspect * f);
            _projectionMatrix.M22 = 1f / f;
            _projectionMatrix.M33 = -(Far + Near) / (Far - Near);
            _projectionMatrix.M34 = -(2f * Far * Near) / (Far - Near);

            return;
        }
        
        _projectionMatrix.M11 = 2f / (aspect * Size);
        _projectionMatrix.M22 = 2f / Size;
        _projectionMatrix.M33 = -2f / (Far - Near);
        _projectionMatrix.M34 = -(Far + Near) / (Far - Near);
    }

    public Camera() { }

    public Camera(CameraProjection projection, float fovy)
    {
        Projection = projection;
        Fovy = fovy;
    }
    public static Camera FromPerspective(float fovy) => new(CameraProjection.Perspective, fovy);
    public static Camera FromOrthographic(float height) => new(CameraProjection.Orthographic, height);
}
public enum CameraProjection
{
    Perspective,
    Orthographic
}