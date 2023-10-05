using System.Globalization;

namespace Meteorite.Mathematics;

public struct Quat : IEquatable<Quat>, IFormattable
{
    public float X, Y, Z, W;

    public Quat(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public Quat(Vec3 vector, float scalar)
    {
        X = vector.X;
        Y = vector.Y;
        Z = vector.Z;
        W = scalar;
    }

    public static Quat FromRotationMatrix(Mat4x4 matrix)
    {
        float s;
        Quat result;
        var trace = matrix.M11 + matrix.M22 + matrix.M33;
        if (trace > 0)
        {
            s = 0.5f / Mathf.Sqrt(trace + 1);
            result.X = (matrix.M32 - matrix.M23) * s;
            result.Y = (matrix.M13 - matrix.M31) * s;
            result.Z = (matrix.M21 - matrix.M12) * s;
            result.W = 0.25f / s;
            return result;
        }

        if (matrix.M11 > matrix.M22 && matrix.M11 > matrix.M33)
        {
            s = 2 * Mathf.Sqrt(1 + matrix.M11 - matrix.M22 - matrix.M33);
            result.X = 0.25f * s;
            result.Y = (matrix.M12 + matrix.M21) / s;
            result.Z = (matrix.M13 + matrix.M31) / s;
            result.W = (matrix.M32 - matrix.M23) / s;
            return result;
        }

        if (matrix.M22 > matrix.M33)
        {
            s = 2 * Mathf.Sqrt(1 + matrix.M22 - matrix.M11 - matrix.M33);
            result.X = (matrix.M12 - matrix.M21) / s;
            result.Y = 0.25f * s;
            result.Z = (matrix.M23 + matrix.M32) / s;
            result.W = (matrix.M12 - matrix.M21) / s;
            return result;
        }

        s = 2 * Mathf.Sqrt(1 + matrix.M33 - matrix.M11 - matrix.M22);
        result.X = (matrix.M13 + matrix.M31) / s;
        result.Y = (matrix.M23 + matrix.M32) / s;
        result.Z = 0.25f * s;
        result.W = (matrix.M21 - matrix.M12) / s;
        return result;
    }

    public Vec3 GetEulerAngles()
    {
        Vec3 euler;

        var sinrCosp = 2 * (W * X + Y * Z);
        var cosrCosp = 1 - 2 * (X * X + Y * Y);
        euler.X = Mathf.Atan2(sinrCosp, cosrCosp);

        var sinp = 2 * (W * Y - Z * X);
        euler.Y = Mathf.Abs(sinp) >= 1 ? Mathf.CopySign(Mathf.PI / 2, sinp) : Mathf.Asin(sinp);

        var sinyCosp = 2 * (W * Z + X * Y);
        var cosyCosp = 1 - 2 * (Y * Y + Z * Z);
        euler.Z = Mathf.Atan2(sinyCosp, cosyCosp);

        return euler;
    }

    public static Quat FromAxisAngle(Vec3 axis, float angle)
    {
        angle *= 0.5f;
        var (sin, cos) = Mathf.SinCos(angle);
        return new(axis * angle, cos);
    }

    public static Quat FromEulerAngles(float x, float y, float z)
    {
        x *= 0.5f;
        y *= 0.5f;
        z *= 0.5f;
        var cy = Mathf.Cos(z);
        var sy = Mathf.Sin(z);
        var cp = Mathf.Cos(y);
        var sp = Mathf.Sin(y);
        var cr = Mathf.Cos(x);
        var sr = Mathf.Sin(x);

        return new(
            cr * cp * cy + sr * sp * sy,
            sr * cp * cy - cr * sp * sy,
            cr * sp * cy + sr * cp * sy,
            cr * cp * sy - sr * sp * cy
        );
    }

    public static Quat FromEulerAngles(Vec3 euler) => FromEulerAngles(euler.X, euler.Y, euler.Z);

    public static Quat FromRotationX(float angle)
    {
        angle *= 0.5f;
        return new(Mathf.Sin(angle), 0, 0, Mathf.Cos(angle));
    }

    public static Quat FromRotationY(float angle)
    {
        angle *= 0.5f;
        return new(0, Mathf.Sin(angle), 0, Mathf.Cos(angle));
    }

    public static Quat FromRotationZ(float angle)
    {
        angle *= 0.5f;
        return new(0, 0, Mathf.Sin(angle), Mathf.Cos(angle));
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

    public Quat Conjugate() => new(-X, -Y, -Z, W);

    public Quat Inverse()
    {
        var inverseSqrLen = 1 / LengthSquared();
        return new(-X * inverseSqrLen, -Y * inverseSqrLen, -Z * inverseSqrLen, W * inverseSqrLen);
    }

    public Quat Normalized() => this / Length();
    public float Length() => Mathf.Sqrt(LengthSquared());
    public float LengthSquared() => Dot(this);
    public float DistanceTo(Quat other) => (this - other).Length();
    public float DistanceToSquared(Quat other) => (this - other).LengthSquared();
    public float Dot(Quat other) => X * other.X + Y * other.Y + Z * other.Z + W * other.W;

    public bool IsEqualApprox(Quat other, float tolerance = Single.Epsilon) =>
        Mathf.IsEqualApprox(X, other.X, tolerance) && Mathf.IsEqualApprox(Y, other.Y, tolerance) &&
        Mathf.IsEqualApprox(Z, other.Z, tolerance) && Mathf.IsEqualApprox(W, other.W, tolerance);

    public bool IsZeroApprox(float tolerance = Single.Epsilon) =>
        Mathf.IsZeroApprox(X, tolerance) && Mathf.IsZeroApprox(Y, tolerance) && Mathf.IsZeroApprox(Z, tolerance) &&
        Mathf.IsZeroApprox(W, tolerance);

    public Quat Lerp(Quat other, float value) => new(
        Mathf.Lerp(X, other.X, value), Mathf.Lerp(Y, other.Y, value), Mathf.Lerp(Z, other.Z, value),
        Mathf.Lerp(W, other.W, value)
    );

    public Quat Lerp(Quat other, Quat value) => new(
        Mathf.Lerp(X, other.X, value.X),
        Mathf.Lerp(Y, other.Y, value.Y), Mathf.Lerp(Z, other.Z, value.Z), Mathf.Lerp(W, other.W, value.W)
    );

    public Quat InvLerp(Quat other, Quat value) => new(
        Mathf.InvLerp(X, other.X, value.X), Mathf.InvLerp(Y, other.Y, value.Y), Mathf.InvLerp(Z, other.Z, value.Z),
        Mathf.InvLerp(W, other.W, value.W)
    );

    public Quat Slerp(Quat other, float value)
    {
        const float threshold = 0.999995f;

        var dot = Dot(other);

        if (dot < 0)
        {
            other = -other;
            dot = -dot;
        }

        if (dot > threshold)
            return Lerp(other, value).Normalized();

        var theta0 = Mathf.Acos(dot);
        var theta = theta0 * value;
        var sinTheta0 = Mathf.Sin(theta0);
        var sinTheta = Mathf.Sin(theta);
        var s0 = Mathf.Cos(theta) - dot * sinTheta / sinTheta0;
        var s1 = sinTheta / sinTheta0;

        return (s0 * this + s1 * other).Normalized();
    }

    public static Quat Identity => new(0, 0, 0, 1);

    public static explicit operator Vec4(Quat other) => new(other.X, other.Y, other.Z, other.W);
    public static explicit operator Vec3(Quat other) => new(other.X, other.Y, other.Z);
    public static explicit operator Vec2(Quat other) => new(other.X, other.Y);
    public static Quat operator -(Quat a) => new(-a.X, -a.Y, -a.Z, -a.W);
    public static Quat operator +(Quat a, Quat b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    public static Quat operator -(Quat a, Quat b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);

    public static Vec3 operator *(Quat a, Vec3 b)
    {
        var x2 = a.X + a.X;
        var y2 = a.Y + a.Y;
        var z2 = a.Z + a.Z;

        var wx2 = a.W * x2;
        var wy2 = a.W * y2;
        var wz2 = a.W * z2;
        var xx2 = a.X * x2;
        var xy2 = a.X * y2;
        var xz2 = a.X * z2;
        var yy2 = a.Y * y2;
        var yz2 = a.Y * z2;
        var zz2 = a.Z * z2;

        return new Vec3(
            b.X * (1.0f - yy2 - zz2) + b.Y * (xy2 - wz2) + b.Z * (xz2 + wy2),
            b.X * (xy2 + wz2) + b.Y * (1.0f - xx2 - zz2) + b.Z * (yz2 - wx2),
            b.X * (xz2 - wy2) + b.Y * (yz2 + wx2) + b.Z * (1.0f - xx2 - yy2)
        );
    }

    public static Quat operator *(Quat a, Quat b)
    {
        Quat ans;

        var q1x = a.X;
        var q1y = a.Y;
        var q1z = a.Z;
        var q1w = a.W;

        var q2x = b.X;
        var q2y = b.Y;
        var q2z = b.Z;
        var q2w = b.W;

        // cross(av, bv)
        var cx = q1y * q2z - q1z * q2y;
        var cy = q1z * q2x - q1x * q2z;
        var cz = q1x * q2y - q1y * q2x;

        var dot = q1x * q2x + q1y * q2y + q1z * q2z;

        ans.X = q1x * q2w + q2x * q1w + cx;
        ans.Y = q1y * q2w + q2y * q1w + cy;
        ans.Z = q1z * q2w + q2z * q1w + cz;
        ans.W = q1w * q2w - dot;

        return ans;
    }
    public static Quat operator /(Quat a, Quat b) => a * b.Inverse();
    public static Quat operator *(Quat a, float b) => new(a.X * b, a.Y * b, a.Z * b, a.W * b);
    public static Quat operator /(Quat a, float b) => new(a.X / b, a.Y / b, a.Z / b, a.W / b);
    public static Quat operator *(float a, Quat b) => b * a;
    public static Quat operator /(float a, Quat b) => new(a / b.X, a / b.Y, a / b.Z, a / b.W);
    public static bool operator ==(Quat a, Quat b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.W == b.W;
    public static bool operator !=(Quat a, Quat b) => !(a == b);
    public bool Equals(Quat other) => this == other;
    public override bool Equals(object? obj) => obj is Quat q && this == q;
    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

    public override string ToString() => ToString("G");
    public string ToString(string? format) => ToString(format, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider) =>
        "Quaternion" + new Vec4(X, Y, Z, W).ToString(format, formatProvider);
}