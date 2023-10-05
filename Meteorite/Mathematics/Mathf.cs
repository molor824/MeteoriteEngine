namespace Meteorite.Mathematics;

public static class Mathf
{
    public static bool IsEqualApprox(float a, float b, float tolerance = Single.Epsilon) =>
        Abs(a - b) <= tolerance * Max(Abs(a), Abs(b));
    public static bool IsZeroApprox(float a, float tolerance = Single.Epsilon) => Abs(a) <= tolerance * Abs(a);
    public static float Sign(float a) => a > 0 ? 1 : a < 0 ? -1 : 0;
    public static float Abs(float a) => a < 0 ? -a : a;
    public static float Max(float a, float b) => a >= b ? a : b;
    public static float Min(float a, float b) => a >= b ? b : a;
    public static float Clamp(float a, float min, float max) => a > max ? max : a < min ? min : a;
    public static float Sqrt(float a) => MathF.Sqrt(a);
    public static float Lerp(float a, float b, float value) => (b - a) * value + a;
    public static float InvLerp(float a, float b, float value) => (value - a) / (b - a);
    public static float Sin(float a) => MathF.Sin(a);
    public static float Cos(float a) => MathF.Cos(a);
    public static (float Sin, float Cos) SinCos(float angle) => (Sin(angle), Cos(angle));
    public static float Tan(float a) => MathF.Tan(a);
    public static float Cot(float a) => 1 / MathF.Tan(a);
    public static float Sinh(float a) => MathF.Sinh(a);
    public static float Cosh(float a) => MathF.Cosh(a);
    public static float Tanh(float a) => MathF.Tanh(a);
    public static float Coth(float a) => 1 / MathF.Tanh(a);
    public static float Asin(float a) => MathF.Asin(a);
    public static float Acos(float a) => MathF.Acos(a);
    public static float Atan(float a) => MathF.Atan(a);
    public static float Atan2(float y, float x) => MathF.Atan2(y, x);
    public static float Acot(float a) => 1 / MathF.Atan(a);
    public static float Asinh(float a) => MathF.Asinh(a);
    public static float Acosh(float a) => MathF.Acosh(a);
    public static float Atanh(float a) => MathF.Atanh(a);
    public static float Acoth(float a) => 1 / MathF.Atanh(a);
    public static float MoveTowards(float a, float b, float delta)
    {
        var diff = b - a;
        if (Abs(diff) > delta) return a + Sign(diff) * delta;
        return b;
    }

    public static float CopySign(float x, float y) => MathF.CopySign(x, y);

    public const float PI = 3.14159265359f;
    public const float E = 2.718281828459045f;
    public const float Tau = PI * 2;
    // deg / 180 * PI = deg * (PI / 180)
    public const float DegToRad = PI / 180f;
    // rad / PI * 180 = rad * (180 / PI)
    public const float RadToDeg = 180f / PI;
}