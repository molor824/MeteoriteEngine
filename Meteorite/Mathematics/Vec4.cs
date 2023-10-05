using System.Globalization;
using System.Text;

namespace Meteorite.Mathematics;

public struct Vec4
{
    public float X, Y, Z, W;

    public Vec4(float scale)
    {
        X = scale;
        Y = scale;
        Z = scale;
        W = scale;
    }

    public Vec4(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public Vec4(Vec2 xy, float z, float w)
    {
        X = xy.X;
        Y = xy.Y;
        Z = z;
        W = w;
    }

    public Vec4(Vec3 xyz, float w)
    {
        X = xyz.X;
        Y = xyz.Y;
        Z = xyz.Z;
        W = w;
    }

    public float this[int index]
    {
        get
        {
            if (index is < 0 or >= 4) throw new IndexOutOfRangeException();
            unsafe
            {
                fixed (float* ptr = &X) return ptr[index];
            }
        }
        set
        {
            if (index is < 0 or >= 4) throw new IndexOutOfRangeException();
            unsafe
            {
                fixed (float* ptr = &X) ptr[index] = value;
            }
        }
    }

    public float Length() => Mathf.Sqrt(LengthSquared());
    public float LengthSquared() => Dot(this);
    public float DistanceTo(Vec4 other) => (this - other).Length();
    public float DistanceToSquared(Vec4 other) => (this - other).LengthSquared();
    public Vec4 Normalized() => this / Length();
    public float Dot(Vec4 other) => X * other.X + Y * other.Y + Z * other.Z + W * other.W;

    public bool IsEqualApprox(Vec4 other, float tolerance = Single.Epsilon) =>
        Mathf.IsEqualApprox(X, other.X, tolerance) && Mathf.IsEqualApprox(Y, other.Y, tolerance) &&
        Mathf.IsEqualApprox(Z, other.Z, tolerance) && Mathf.IsEqualApprox(W, other.W, tolerance);

    public bool IsZeroApprox(float tolerance = Single.Epsilon) =>
        Mathf.IsZeroApprox(X, tolerance) && Mathf.IsZeroApprox(Y, tolerance) && Mathf.IsZeroApprox(Z, tolerance) &&
        Mathf.IsZeroApprox(W, tolerance);

    public Vec4 Abs() => new(Mathf.Abs(X), Mathf.Abs(Y), Mathf.Abs(Z), Mathf.Abs(W));

    public Vec4 Max(Vec4 other) => new(Mathf.Max(X, other.X), Mathf.Max(Y, other.Y), Mathf.Max(Z, other.Z),
        Mathf.Max(W, other.W));

    public Vec4 Min(Vec4 other) => new(Mathf.Min(X, other.X), Mathf.Min(Y, other.Y), Mathf.Min(Z, other.Z),
        Mathf.Min(W, other.W));

    public Vec4 Clamp(Vec4 min, Vec4 max) => new(Mathf.Clamp(X, min.X, max.X), Mathf.Clamp(Y, min.Y, max.Y),
        Mathf.Clamp(Z, min.Z, max.Z), Mathf.Clamp(W, min.W, max.W));

    public Vec4 Lerp(Vec4 a, Vec4 b, float value) => (b - a) * value + a;
    public Vec4 Lerp(Vec4 a, Vec4 b, Vec4 value) => (b - a) * value + a;
    public Vec4 InvLerp(Vec4 a, Vec4 b, Vec4 value) => (value - a) / (b - a);

    public Vec4 Reflect(Vec4 normal)
    {
        var dot = Dot(normal);
        return this - 2f * dot * normal;
    }

    public Vec4 MoveTowards(Vec4 b, float delta)
    {
        var diff = b - this;
        var length = diff.Length();
        if (length > delta) return this + diff / length * b;
        return b;
    }

    public static explicit operator Vec3(Vec4 a) => new(a.X, a.Y, a.Z);
    public static explicit operator Vec2(Vec4 a) => new(a.X, a.Y);
    public static Vec4 operator -(Vec4 a) => new(-a.X, -a.Y, -a.Z, -a.W);
    public static Vec4 operator +(Vec4 a, Vec4 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    public static Vec4 operator -(Vec4 a, Vec4 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
    public static Vec4 operator *(Vec4 a, Vec4 b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
    public static Vec4 operator /(Vec4 a, Vec4 b) => new(a.X / b.X, a.Y / b.Y, a.Z / b.Z, a.W / b.W);
    public static Vec4 operator *(Vec4 a, float b) => new(a.X * b, a.Y * b, a.Z * b, a.W * b);
    public static Vec4 operator /(Vec4 a, float b) => new(a.X / b, a.Y / b, a.Z / b, a.W / b);
    public static Vec4 operator *(float a, Vec4 b) => b * a;
    public static Vec4 operator /(float a, Vec4 b) => new(a / b.X, a / b.Y, a / b.Z, a / b.W);
    public static bool operator ==(Vec4 a, Vec4 b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;
    public static bool operator !=(Vec4 a, Vec4 b) => !(a == b);
    public static bool operator >=(Vec4 a, Vec4 b) => a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z && a.W >= b.W;
    public static bool operator <=(Vec4 a, Vec4 b) => a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z && a.W <= b.W;
    public static bool operator >(Vec4 a, Vec4 b) => !(a <= b);
    public static bool operator <(Vec4 a, Vec4 b) => !(a >= b);

    public bool Equals(Vec4 other) => this == other;
    public override bool Equals(object? obj) => obj is Vec4 v && Equals(v);
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
        builder.Append(seperator);
        builder.Append(W.ToString(format, formatProvider));
        builder.Append(')');
        return builder.ToString();
    }

    public override string ToString() => ToString("G");
    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

    public static Vec4 One => new(1);
    public static Vec4 Zero => new();
    public static Vec4 UnitX => new(1, 0, 0, 0);
    public static Vec4 UnitY => new(0, 1, 0, 0);
    public static Vec4 UnitZ => new(0, 0, 1, 0);
    public static Vec4 UnitW => new(0, 0, 0, 1);
}