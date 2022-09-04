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
    public mat4 ProjectionMatrix => _projectionMatrix;

    private mat4 _projectionMatrix = mat4.Identity;

    public override void Render(float delta)
    {
        var aspect = (float)Game.Width / Game.Height;
        
        if (Projection == CameraProjection.Perspective)
        {
            var f = MathF.Tan(Fovy * MathConst.Deg2Rad / 2f);
            
            _projectionMatrix.m33 = 0;
            _projectionMatrix.m23 = -1;
            _projectionMatrix.m00 = 1f / (aspect * f);
            _projectionMatrix.m11 = 1f / f;
            _projectionMatrix.m22 = -(Far + Near) / (Far - Near);
            _projectionMatrix.m32 = -(2f * Far * Near) / (Far - Near);

            return;
        }
        
        _projectionMatrix.m00 = 2f / (aspect * Size);
        _projectionMatrix.m11 = 2f / Size;
        _projectionMatrix.m22 = -2f / (Far - Near);
        _projectionMatrix.m32 = -(Far + Near) / (Far - Near);
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