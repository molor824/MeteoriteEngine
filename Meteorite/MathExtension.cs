namespace Meteorite;

public static class MathExtension
{
    public static vec3 MoveTowards(this vec3 a, vec3 target, float delta)
    {
        var diff = target - a;
        var len = diff.LengthSqr;

        if (len > delta * delta) return a + diff.NormalizedSafe * delta;
        return target;
    }
}