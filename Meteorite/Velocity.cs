namespace Meteorite;

public class LocalVelocity : Transform
{
    public vec3 Linear;
    public quat Angular;

    public override void Update(float delta)
    {
        Position += Linear * delta;
        Rotation *= Angular * delta;
    }
}