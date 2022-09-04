namespace Meteorite;

public class LocalVelocity : Transform
{
    public Vector3 Linear;
    public Quaternion Angular;

    public override void Update(float delta)
    {
        base.Update(delta);

        Position += Linear * delta;
        Rotation *= Angular * delta;
    }
}