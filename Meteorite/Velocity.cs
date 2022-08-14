namespace Meteorite;

public class Velocity : Transform
{
    public vec3 Linear;
    public quat Angular;

    public override void Update(float delta)
    {
        base.Update(delta);

        Position += Linear * delta;
        Rotation *= Angular * delta;
    }
}