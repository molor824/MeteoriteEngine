using Meteorite.Mathematics;

namespace Meteorite;

public class LocalVelocity : Transform3D
{
    public Vec3 Linear;
    public Vec3 Angular;

    public override void Update(float delta)
    {
        ApplyTranslation(Linear * delta);
        ApplyRotation(Quat.FromEulerAngles(Angular * (delta * Mathf.DegToRad)));
    }
}