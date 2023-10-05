using System.Globalization;
using System.Text;

namespace Meteorite.Mathematics;

public struct Mat3x3 : IEquatable<Mat3x3>, IFormattable
{
    public float M11, M12, M13, M21, M22, M23, M31, M32, M33;

    public Mat3x3(float m11, float m12, float m13, float m21, float m22, float m23, float m31, float m32, float m33)
    {
        M11 = m11;
        M12 = m12;
        M13 = m13;
        M21 = m21;
        M22 = m22;
        M23 = m23;
        M31 = m31;
        M32 = m32;
        M33 = m33;
    }

    public Mat3x3(Vec3 row1, Vec3 row2, Vec3 row3) : this(row1.X, row1.Y, row1.Z, row2.X, row2.Y, row2.Z,
        row3.X, row3.Y, row3.Z)
    {
    }

    public static Mat3x3 Identity => new(
        1, 0, 0,
        0, 1, 0,
        0, 0, 1
    );

    public static Mat3x3 FromTranslation(Vec2 translation) => new(
        0, 0, translation.X,
        0, 0, translation.Y,
        0, 0, 1
    );

    public static Mat3x3 FromRotation(float rotation)
    {
        var (s, c) = Mathf.SinCos(rotation);
        return new(
            c, -s, 0,
            s, c, 0,
            0, 0, 1
        );
    }

    public static Mat3x3 FromScale(Vec2 scale) => new(
        scale.X, 0, 0,
        0, scale.Y, 0,
        0, 0, 1
    );

    public static Mat3x3 FromTransformation(Vec2 translation, float rotation, Vec2 scale)
    {
        var result = FromRotation(rotation) * FromScale(scale);
        result.Translation = translation;
        return result;
    }

    public static Mat3x3 FromTransform3D(in Mat4x4 transform) => new(
        transform.M11, transform.M12, transform.M14,
        transform.M21, transform.M22, transform.M24,
        0, 0, 1
    );

    public Vec2 Translation
    {
        get => (Vec2)Column3;
        set
        {
            M13 = value.X;
            M23 = value.Y;
        }
    }

    public void ApplyTranslation(Vec2 translation)
    {
        M13 += translation.X;
        M23 += translation.Y;
    }

    public Mat3x3 Translated(Vec2 translation)
    {
        var result = this;
        result.ApplyTranslation(translation);
        return result;
    }

    public Vec2 GetScale() => new(
        new Vec2(M11, M21).Length(),
        new Vec2(M12, M22).Length()
    );

    public void ApplyScale(Vec2 scale)
    {
        M11 *= scale.X;
        M21 *= scale.X;
        M12 *= scale.Y;
        M22 *= scale.Y;
    }

    public Mat3x3 Scaled(Vec2 scale)
    {
        var result = this;
        result.ApplyScale(scale);
        return result;
    }

    public float GetRotation()
    {
        var invScaleX = 1 / new Vec2(M11, M21).Length();
        return Mathf.Atan2(invScaleX * M21, invScaleX * M11);
    }

    public void ApplyRotation(float angle)
    {
        this = FromRotation(angle) * this;
    }

    public Mat3x3 Rotated(float angle)
    {
        var result = this;
        result.ApplyRotation(angle);
        return result;
    }

    public Mat3x3 Transposed() => new(
        M11, M21, M31,
        M12, M22, M32,
        M13, M23, M33
    );

    public float Determinant() => M11 * (M22 * M33 - M23 * M32) -
                                  M12 * (M21 * M33 - M23 * M31) +
                                  M13 * (M21 * M32 - M22 * M31);

    public bool TryInverse(out Mat3x3 result)
    {
        var det = Determinant();
        if (Mathf.IsZeroApprox(det))
        {
            result = new();
            return false;
        }

        var invDet = 1 / det;

        result.M11 = (M22 * M33 - M23 * M32) * invDet;
        result.M12 = (M13 * M32 - M12 * M33) * invDet;
        result.M13 = (M12 * M23 - M13 * M22) * invDet;
        result.M21 = (M23 * M31 - M21 * M33) * invDet;
        result.M22 = (M11 * M33 - M13 * M31) * invDet;
        result.M23 = (M13 * M21 - M11 * M23) * invDet;
        result.M31 = (M21 * M32 - M22 * M31) * invDet;
        result.M32 = (M12 * M31 - M11 * M32) * invDet;
        result.M33 = (M11 * M22 - M12 * M21) * invDet;

        return true;
    }

    public Mat3x3 Inverse()
    {
        TryInverse(out var result);
        return result;
    }

    public Mat4x4 ToTransform3D() => Mat4x4.FromTransform2D(this);

    public static Mat3x3 operator -(Mat3x3 a) =>
        new(-a.M11, -a.M12, -a.M13, -a.M21, -a.M22, -a.M23, -a.M31, -a.M32, -a.M33);

    public static Mat3x3 operator +(Mat3x3 a, Mat3x3 b) => new(a.M11+b.M11,a.M12+b.M12,a.M13+b.M13,a.M21+b.M21,a.M22+b.M22,a.M23+b.M23,a.M31+b.M31,a.M32+b.M32,a.M33+b.M33);

    public static Mat3x3 operator -(Mat3x3 a, Mat3x3 b) => new(a.M11-b.M11,a.M12-b.M12,a.M13-b.M13,a.M21-b.M21,a.M22-b.M22,a.M23-b.M23,a.M31-b.M31,a.M32-b.M32,a.M33-b.M33);

    public static Vec3 operator *(Mat3x3 a, Vec3 b) => new(
        a.M11 * b.X + a.M12 * b.Y + a.M13 * b.Z,
        a.M21 * b.X + a.M22 * b.Y + a.M23 * b.Z,
        a.M31 * b.X + a.M32 * b.Y + a.M33 * b.Z
    );

    public static Vec2 operator *(Mat3x3 a, Vec2 b) => new(
        a.M11 * b.X + a.M12 * b.Y + a.M13,
        a.M21 * b.X + a.M22 * b.Y + a.M23
    );

    public static Mat3x3 operator *(Mat3x3 a, Mat3x3 b) => new(
        a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31,
        a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32,
        a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33,
        a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31,
        a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32,
        a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33,
        a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31,
        a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32,
        a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33
    );

    public static Mat3x3 operator *(Mat3x3 a, float b) => new(
        a.M11 * b, a.M12 * b, a.M13 * b,
        a.M21 * b, a.M22 * b, a.M23 * b,
        a.M31 * b, a.M32 * b, a.M33 * b
    );

    public static Mat3x3 operator *(float a, Mat3x3 b) => b * a;

    public static Mat3x3 operator /(Mat3x3 a, float b) => new(
        a.M11 / b, a.M12 / b, a.M13 / b,
        a.M21 / b, a.M22 / b, a.M23 / b,
        a.M31 / b, a.M32 / b, a.M33 / b
    );

    public static Mat3x3 operator /(float a, Mat3x3 b) => new(
        a / b.M11, a / b.M12, a / b.M13,
        a / b.M21, a / b.M22, a / b.M23,
        a / b.M31, a / b.M32, a / b.M33
    );

    public static bool operator ==(Mat3x3 a, Mat3x3 b) => a.M11 == b.M11 && a.M12 == b.M12 && a.M13 == b.M13 &&
                                                          a.M21 == b.M21 && a.M22 == b.M22 && a.M23 == b.M23 &&
                                                          a.M31 == b.M31 && a.M32 == b.M32 && a.M33 == b.M33;

    public static bool operator !=(Mat3x3 a, Mat3x3 b) => !(a == b);

    public override bool Equals(object? obj) => obj is Mat3x3 m && m == this;

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(M11);
        hash.Add(M12);
        hash.Add(M13);
        hash.Add(M21);
        hash.Add(M22);
        hash.Add(M23);
        hash.Add(M31);
        hash.Add(M32);
        hash.Add(M33);
        return hash.ToHashCode();
    }

    public bool Equals(Mat3x3 other) => this == other;
    public override string ToString() => ToString("G");
    public string ToString(string? format) => ToString(format, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var builder = new StringBuilder();
        builder.Append('(');
        builder.Append(Row1.ToString(format, formatProvider));
        builder.Append(Row2.ToString(format, formatProvider));
        builder.Append(Row3.ToString(format, formatProvider));
        builder.Append(')');
        
        return builder.ToString();
    }

    public Vec3 Row1
    {
        get => new(M11, M12, M13);
        set
        {
            M11 = value.X;
            M12 = value.Y;
            M13 = value.Z;
        }
    }

    public Vec3 Row2
    {
        get => new(M21, M22, M23);
        set
        {
            M21 = value.X;
            M22 = value.Y;
            M23 = value.Z;
        }
    }

    public Vec3 Row3
    {
        get => new(M31, M32, M33);
        set
        {
            M31 = value.X;
            M32 = value.Y;
            M33 = value.Z;
        }
    }

    public Vec3 Column1
    {
        get => new(M11, M21, M31);
        set
        {
            M11 = value.X;
            M21 = value.Y;
            M31 = value.Z;
        }
    }

    public Vec3 Column2
    {
        get => new(M12, M22, M32);
        set
        {
            M12 = value.X;
            M22 = value.Y;
            M32 = value.Z;
        }
    }

    public Vec3 Column3
    {
        get => new(M13, M23, M33);
        set
        {
            M13 = value.X;
            M23 = value.Y;
            M33 = value.Z;
        }
    }
}