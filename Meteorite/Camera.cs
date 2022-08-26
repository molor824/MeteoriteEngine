namespace Meteorite;

public class Camera : Transform
{
    public CameraProjection Projection = CameraProjection.Orthographic;
    public float Fovy = 10;
    public float Near = 0.01f, Far = 1000;
    public mat4 WorldToCamera => (
        Projection == CameraProjection.Perspective ?
        mat4.PerspectiveFov(Fovy, Game.Main.Width, Game.Main.Height, Near, Far) :
        mat4.Ortho(Game.Main.Width / -2, Game.Main.Width / 2, Game.Main.Height / -2, Game.Main.Height / 2)
    ) * GlobalTransformMatrix.Inverse;

    public Camera() { }
    public Camera(CameraProjection projection, float fovy) { }
    public static Camera FromPerspective(float fovy) => new(CameraProjection.Perspective, fovy);
    public static Camera FromOrthographic(float height) => new(CameraProjection.Orthographic, height);
}
public enum CameraProjection
{
    Perspective,
    Orthographic
}