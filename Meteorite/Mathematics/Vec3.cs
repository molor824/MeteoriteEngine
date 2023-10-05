using System.Globalization;
using System.Text;

namespace Meteorite.Mathematics;

public struct Vec3 : IEquatable<Vec3>, IFormattable
{
    public float X, Y, Z;

    public Vec3(float scale)
    {
        X = scale;
        Y = scale;
        Z = scale;
    }

    public Vec3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vec3(Vec2 xy, float z)
    {
        X = xy.X;
        Y = xy.Y;
        Z = z;
    }

    public float this[int index]
    {
        get
        {
            if (index is < 0 or >= 3) throw new IndexOutOfRangeException();
            unsafe
            {
                fixed (float* ptr = &X) return ptr[index];
            }
        }
        set
        {
            if (index is < 0 or >= 3) throw new IndexOutOfRangeException();
            unsafe
            {
                fixed (float* ptr = &X) ptr[index] = value;
            }
        }
    }

    public float Length() => Mathf.Sqrt(LengthSquared());
    public float LengthSquared() => Dot(this);
    public float DistanceTo(Vec3 other) => (this - other).Length();
    public float DistanceToSquared(Vec3 other) => (this - other).LengthSquared();
    public Vec3 Normalized() => this / Length();
    public float Dot(Vec3 other) => X * other.X + Y * other.Y + Z * other.Z;

    public Vec3 Cross(Vec3 other) => new(
        Y * other.Z - Z * other.Y,
        Z * other.X - X * other.Z,
        X * other.Y - Y * other.X
    );

    public bool IsEqualApprox(Vec3 other, float tolerance = Single.Epsilon) =>
        Mathf.IsEqualApprox(X, other.X, tolerance) && Mathf.IsEqualApprox(Y, other.Y, tolerance) &&
        Mathf.IsEqualApprox(Z, other.Z, tolerance);

    public bool IsZeroApprox(float tolerance = Single.Epsilon) =>
        Mathf.IsZeroApprox(X, tolerance) && Mathf.IsZeroApprox(Y, tolerance) && Mathf.IsZeroApprox(Z, tolerance);

    public Vec3 Abs() => new(Mathf.Abs(X), Mathf.Abs(Y), Mathf.Abs(Z));
    public Vec3 Max(Vec3 other) => new(Mathf.Max(X, other.X), Mathf.Max(Y, other.Y), Mathf.Max(Z, other.Z));
    public Vec3 Min(Vec3 other) => new(Mathf.Min(X, other.X), Mathf.Min(Y, other.Y), Mathf.Min(Z, other.Z));

    public Vec3 Clamp(Vec3 min, Vec3 max) => new(Mathf.Clamp(X, min.X, max.X), Mathf.Clamp(Y, min.Y, max.Y),
        Mathf.Clamp(Z, min.Z, max.Z));

    public Vec3 Lerp(Vec3 a, Vec3 b, float value) => (b - a) * value + a;
    public Vec3 Lerp(Vec3 a, Vec3 b, Vec3 value) => (b - a) * value + a;
    public Vec3 InvLerp(Vec3 a, Vec3 b, Vec3 value) => (value - a) / (b - a);

    public Vec3 Reflect(Vec3 normal)
    {
        var dot = Dot(normal);
        return this - 2f * dot * normal;
    }

    public Vec3 MoveTowards(Vec3 b, float delta)
    {
        var diff = b - this;
        var length = diff.Length();
        if (length > delta) return this + diff / length * delta;
        return b;
    }

    public static explicit operator Vec2(Vec3 a) => new(a.X, a.Y);
    public static explicit operator Vec4(Vec3 a) => new(a, 0);
    public static explicit operator Quat(Vec3 a) => new(a.X, a.Y, a.Z, 0);
    public static Vec3 operator -(Vec3 a) => new(-a.X, -a.Y, -a.Z);
    public static Vec3 operator +(Vec3 a, Vec3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vec3 operator -(Vec3 a, Vec3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vec3 operator *(Vec3 a, Vec3 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    public static Vec3 operator /(Vec3 a, Vec3 b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
    public static Vec3 operator *(Vec3 a, float b) => new(a.X * b, a.Y * b, a.Z * b);
    public static Vec3 operator /(Vec3 a, float b) => new(a.X / b, a.Y / b, a.Z / b);
    public static Vec3 operator *(float a, Vec3 b) => b * a;
    public static Vec3 operator /(float a, Vec3 b) => new(a / b.X, a / b.Y, a / b.Z);
    public static bool operator ==(Vec3 a, Vec3 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    public static bool operator !=(Vec3 a, Vec3 b) => !(a == b);
    public static bool operator >=(Vec3 a, Vec3 b) => a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z;
    public static bool operator <=(Vec3 a, Vec3 b) => a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z;
    public static bool operator >(Vec3 a, Vec3 b) => !(a <= b);
    public static bool operator <(Vec3 a, Vec3 b) => !(a >= b);

    public bool Equals(Vec3 other) => this == other;
    public override bool Equals(object? obj) => obj is Vec3 v && Equals(v);
    public string ToString(string? format) => ToString(format, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var seperator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
        var builder = new StringBuilder();
        builder.Append('(');
        builder.Append(X.ToString(format, formatProvider));
        builder.Append(seperator);
        builder.Append(Y.ToString(format, formatProvider));
        builder.Append(seperator);
        builder.Append(Z.ToString(format, formatProvider));
        builder.Append(')');
        return builder.ToString();
    }

    public override string ToString() => ToString("G");
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public static Vec3 One => new(1);
    public static Vec3 Zero => new();
    public static Vec3 UnitX => new(1, 0, 0);
    public static Vec3 UnitY => new(0, 1, 0);
    public static Vec3 UnitZ => new(0, 0, 1);
}