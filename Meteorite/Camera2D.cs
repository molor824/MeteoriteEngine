using Meteorite.Mathematics;

namespace Meteorite;

public class Camera2D : Transform2D
{
    public float Size = 10;
    public float Far = 1000, Near = -1000;
    public Mat4x4 ProjectionMatrix => _projectionMatrix;

    private Mat4x4 _projectionMatrix;

    public override void Render(float delta)
    {
        var aspect = (float)Game.Width / Game.Height;

        _projectionMatrix = Mat4x4.OpenGLOrthographic(aspect * Size, Size, Near, Far);
    }
}