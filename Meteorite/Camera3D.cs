using Meteorite.Mathematics;

namespace Meteorite;

public class Camera3D : Transform3D
{
    public CameraProjection Projection = CameraProjection.Orthographic;
    public float Fovy = 90;
    public float Size = 10;
    public float Near = -1000, Far = 1000;
    /// <summary>
    /// Projection matrix updates every render frame instead of everytime when property getter is called
    /// </summary>
    public Mat4x4 ProjectionMatrix => _projectionMatrix;

    private Mat4x4 _projectionMatrix = Mat4x4.Identity;

    public override void Render(float delta)
    {
        var aspect = (float)Game.Width / Game.Height;

        if (Projection == CameraProjection.Perspective)
        {
            _projectionMatrix = Mat4x4.OpenGLPerspective(Fovy, aspect, Near, Far);
            return;
        }

        _projectionMatrix = Mat4x4.OpenGLOrthographic(Size * aspect, Size, Near, Far);
    }

    public Camera3D() { }

    public Camera3D(CameraProjection projection, float fovy)
    {
        Projection = projection;
        Fovy = fovy;
    }
    public static Camera3D FromPerspective(float fovy) => new(CameraProjection.Perspective, fovy);
    public static Camera3D FromOrthographic(float height) => new(CameraProjection.Orthographic, height);
}
public enum CameraProjection
{
    Perspective,
    Orthographic
}