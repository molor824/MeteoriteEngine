using System.Globalization;
using System.Text;

namespace Meteorite.Mathematics;

public struct Vec2 : IEquatable<Vec2>, IFormattable
{
    public float X, Y;

    public Vec2(float scale)
    {
        X = scale;
        Y = scale;
    }

    public Vec2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public float this[int index]
    {
        get
        {
            if (index is < 0 or >= 2) throw new IndexOutOfRangeException();
            unsafe
            {
                fixed (float* ptr = &X) return ptr[index];
            }
        }
        set
        {
            if (index is < 0 or >= 2) throw new IndexOutOfRangeException();
            unsafe
            {
                fixed (float* ptr = &X) ptr[index] = value;
            }
        }
    }

    public float Length() => Mathf.Sqrt(LengthSquared());
    public float LengthSquared() => Dot(this);
    public float DistanceTo(Vec2 other) => (this - other).Length();
    public float DistanceToSquared(Vec2 other) => (this - other).LengthSquared();
    public Vec2 Normalized() => this / Length();
    public float Dot(Vec2 other) => X * other.X + Y * other.Y;

    public bool IsEqualApprox(Vec2 other, float tolerance = Single.Epsilon) =>
        Mathf.IsEqualApprox(X, other.X, tolerance) && Mathf.IsEqualApprox(Y, other.Y, tolerance);

    public bool IsZeroApprox(float tolerance = Single.Epsilon) =>
        Mathf.IsZeroApprox(X, tolerance) && Mathf.IsZeroApprox(Y, tolerance);

    public Vec2 Abs() => new(Mathf.Abs(X), Mathf.Abs(Y));
    public Vec2 Max(Vec2 other) => new(Mathf.Max(X, other.X), Mathf.Max(Y, other.Y));
    public Vec2 Min(Vec2 other) => new(Mathf.Min(X, other.X), Mathf.Min(Y, other.Y));
    public Vec2 Clamp(Vec2 min, Vec2 max) => new(Mathf.Clamp(X, min.X, max.X), Mathf.Clamp(Y, min.Y, max.Y));
    public Vec2 Lerp(Vec2 a, Vec2 b, float value) => (b - a) * value + a;
    public Vec2 Lerp(Vec2 a, Vec2 b, Vec2 value) => (b - a) * value + a;
    public Vec2 InvLerp(Vec2 a, Vec2 b, Vec2 value) => (value - a) / (b - a);

    public Vec2 Reflect(Vec2 normal)
    {
        var dot = Dot(normal);
        return this - 2f * dot * normal;
    }

    public Vec2 MoveTowards(Vec2 b, float delta)
    {
        var diff = b - this;
        var length = diff.Length();
        if (length > delta) return this + diff / length * b;
        return b;
    }

    public static explicit operator Vec3(Vec2 a) => new(a, 0);
    public static explicit operator Vec4(Vec2 a) => new(a, 0, 0);
    public static explicit operator Quat(Vec2 a) => new(a.X, a.Y, 0, 0);
    public static Vec2 operator -(Vec2 a) => new(-a.X, -a.Y);
    public static Vec2 operator +(Vec2 a, Vec2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Vec2 operator -(Vec2 a, Vec2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Vec2 operator *(Vec2 a, Vec2 b) => new(a.X * b.X, a.Y * b.Y);
    public static Vec2 operator /(Vec2 a, Vec2 b) => new(a.X / b.X, a.Y / b.Y);
    public static Vec2 operator *(Vec2 a, float b) => new(a.X * b, a.Y * b);
    public static Vec2 operator /(Vec2 a, float b) => new(a.X / b, a.Y / b);
    public static Vec2 operator *(float a, Vec2 b) => b * a;
    public static Vec2 operator /(float a, Vec2 b) => new(a / b.X, a / b.Y);
    public static bool operator ==(Vec2 a, Vec2 b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Vec2 a, Vec2 b) => !(a == b);
    public static bool operator >=(Vec2 a, Vec2 b) => a.X >= b.X && a.Y >= b.Y;
    public static bool operator <=(Vec2 a, Vec2 b) => a.X <= b.X && a.Y <= b.Y;
    public static bool operator >(Vec2 a, Vec2 b) => !(a <= b);
    public static bool operator <(Vec2 a, Vec2 b) => !(a >= b);

    public bool Equals(Vec2 other) => this == other;
    public override bool Equals(object? obj) => obj is Vec2 v && Equals(v);
    public string ToString(string? format) => ToString(format, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var seperator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
        var builder = new StringBuilder();
        builder.Append('(');
        builder.Append(X.ToString(format, formatProvider));
        builder.Append(seperator);
        builder.Append(Y.ToString(format, formatProvider));
        builder.Append(')');
        return builder.ToString();
    }

    public override string ToString() => ToString("G");
    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static Vec2 One => new(1);
    public static Vec2 Zero => new();
    public static Vec2 UnitX => new(1, 0);
    public static Vec2 UnitY => new(0, 1);
}