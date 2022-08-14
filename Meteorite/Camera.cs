namespace Meteorite;

public class Camera : Transform
{
    internal Raylib_cs.Camera3D Raw = new() { up = new(0, 1, 0) };

    public Camera() { }
    public Camera(CameraProjection projection, float fovy)
    {
        Raw.projection = (Raylib_cs.CameraProjection)projection;
        Raw.fovy = fovy;
    }
    public vec3 LookDirection = new(0, 0, -1);
    public CameraProjection Projection
    {
        get => (CameraProjection)Raw.projection;
        set { Raw.projection = (Raylib_cs.CameraProjection)value; }
    }
    public float FovY
    {
        get => Raw.fovy;
        set { Raw.fovy = value; }
    }

    internal void CameraUpdate()
    {
        Raw.position = GlobalPosition.ToSystem();
        Raw.target = (GlobalRotation * LookDirection).ToSystem() + Raw.position;
    }
}
public enum CameraProjection
{
    Perspective,
    Orthographic
}