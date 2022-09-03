namespace Meteorite;

public class Camera : Transform
{
    public CameraProjection Projection = CameraProjection.Orthographic;
    public float Fovy = 70;
    public float Size = 10;
    public float Near = -1000, Far = 1000;
    public mat4 ProjectionMatrix
    {
        get
        {
            var ratio = (float)Game.Width / Game.Height;

            return Projection == CameraProjection.Perspective
                ? mat4.Perspective(Fovy, ratio, Near, Far)
                : mat4.Ortho(ratio / -2 * Size, ratio / 2 * Size, Size / -2, Size / 2, Near, Far);
        }
    }
    public mat4 WorldToCamera => GlobalTransformMatrix * ProjectionMatrix;

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