namespace Meteorite;

public static class MathExtension
{
    public static Vector3 MoveTowards(this Vector3 a, Vector3 target, float delta)
    {
        var diff = target - a;
        var len = diff.LengthSquared;

        if (len > delta * delta) return a + target.Normalized() * delta;
        return target;
    }
}