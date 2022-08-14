namespace Meteorite;

using System.Numerics;

public static class GlmExtension
{
    public static vec3 GetScale(this mat4 a) => new(
        glm.Sign(a.m00) * ((vec3)a.Column0).Length,
        glm.Sign(a.m11) * ((vec3)a.Column1).Length,
        glm.Sign(a.m22) * ((vec3)a.Column2).Length
    );
    public static vec3 ToGlm(this Vector3 a) => new(a.X, a.Y, a.Z);
    public static vec2 ToGlm(this Vector2 a) => new(a.X, a.Y);
    public static Vector3 ToSystem(this vec3 a) => new(a.x, a.y, a.z);
    public static Vector2 ToSystem(this vec2 a) => new(a.x, a.y);
    public static vec3 MoveTowards(this vec3 a, vec3 target, float delta)
    {
        if (a == target) return a;

        var diff = target - a;
        var dist = diff.LengthSqr;

        if (dist > delta * delta) return a + diff / dist * delta;

        return target;
    }
    public static Matrix4x4 ToSystem(this mat4 a) => new()
    {
        M11 = a.m00,
        M12 = a.m01,
        M13 = a.m02,
        M14 = a.m03,

        M21 = a.m10,
        M22 = a.m11,
        M23 = a.m12,
        M24 = a.m13,

        M31 = a.m20,
        M32 = a.m21,
        M33 = a.m22,
        M34 = a.m23,

        M41 = a.m30,
        M42 = a.m31,
        M43 = a.m32,
        M44 = a.m33,
    };
}