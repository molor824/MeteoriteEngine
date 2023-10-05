using System.Globalization;
using System.Text;

namespace Meteorite.Mathematics;

public struct Mat4x4 : IEquatable<Mat4x4>, IFormattable
{
    public float M11, M12, M13, M14, M21, M22, M23, M24, M31, M32, M33, M34, M41, M42, M43, M44;

    public Mat4x4(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31,
        float m32, float m33, float m34, float m41, float m42, float m43, float m44)
    {
        M11 = m11;
        M12 = m12;
        M13 = m13;
        M14 = m14;
        M21 = m21;
        M22 = m22;
        M23 = m23;
        M24 = m24;
        M31 = m31;
        M32 = m32;
        M33 = m33;
        M34 = m34;
        M41 = m41;
        M42 = m42;
        M43 = m43;
        M44 = m44;
    }

    public Mat4x4(Vec4 row1, Vec4 row2, Vec4 row3, Vec4 row4) : this(row1.X, row1.Y, row1.Z, row1.W,
        row2.X, row2.Y, row2.Z, row2.W, row3.X, row3.Y, row3.Z, row3.W, row4.X, row4.Y, row4.Z, row4.W)
    {
    }

    public Mat4x4(Mat3x3 basis, Vec3 translation, Vec4 row4) : this(basis.M11, basis.M21, basis.M31,
        translation.X, basis.M12, basis.M22, basis.M32, translation.Y, basis.M13, basis.M23, basis.M33, translation.Z,
        row4.X, row4.Y, row4.Z, row4.W)
    {
    }

    public static Mat4x4 OpenGLOrthographic(float width, float height, float nearPlane, float farPlane) => new(
        2f / width, 0, 0, 0,
        0, 2f / height, 0, 0,
        0, 0, -2f / (farPlane - nearPlane), (farPlane + nearPlane) / (nearPlane - farPlane),
        0, 0, 0, 1
    );

    public static Mat4x4 OpenGLPerspective(float fieldOfView, float aspectRatio, float nearPlaneDistance,
        float farPlaneDistance)
    {
        var cot = Mathf.Cot(fieldOfView * 0.5f);

        return new(
            cot / aspectRatio, 0, 0, 0,
            0, cot, 0, 0,
            0, 0, (farPlaneDistance + nearPlaneDistance) / (nearPlaneDistance - farPlaneDistance),
            (2.0f * farPlaneDistance * nearPlaneDistance) / (nearPlaneDistance - farPlaneDistance),
            0, 0, -1, 0
        );
    }

    public static Mat4x4 FromTranslation(Vec3 translation) => new(
        1, 0, 0, translation.X,
        0, 1, 0, translation.Y,
        0, 0, 1, translation.Z,
        0, 0, 0, 1
    );

    public static Mat4x4 FromScale(Vec3 scale) => new(
        scale.X, 0, 0, 0,
        0, scale.Y, 0, 0,
        0, 0, scale.Z, 0,
        0, 0, 0, 1
    );

    public static Mat4x4 FromRotation(Quat quaternion)
    {
        var x = quaternion.X;
        var y = quaternion.Y;
        var z = quaternion.Z;
        var w = quaternion.W;

        var xx = x * x;
        var xy = x * y;
        var xz = x * z;
        var xw = x * w;

        var yy = y * y;
        var yz = y * z;
        var yw = y * w;

        var zz = z * z;
        var zw = z * w;

        var result = Identity;

        result.M11 = 1 - 2 * (yy + zz);
        result.M12 = 2 * (xy - zw);
        result.M13 = 2 * (xz + yw);

        result.M21 = 2 * (xy + zw);
        result.M22 = 1 - 2 * (xx + zz);
        result.M23 = 2 * (yz - xw);

        result.M31 = 2 * (xz - yw);
        result.M32 = 2 * (yz + xw);
        result.M33 = 1 - 2 * (xx + yy);

        return result;
    }

    public static Mat4x4 FromTransformation(Vec3 translation, Quat rotation, Vec3 scale)
    {
        var result = FromRotation(rotation) * FromScale(scale);
        result.Translation = translation;
        return result;
    }

    public Mat3x3 ToTransform2D() => Mat3x3.FromTransform3D(this);

    public static Mat4x4 FromTransform2D(in Mat3x3 transform) => new(
        transform.M11, transform.M12, 0, transform.M13,
        transform.M21, transform.M22, 0, transform.M23,
        0, 0, 1, transform.M33,
        0, 0, 0, 1
    );

    public static Mat4x4 FromAxisAngle(Vec3 axis, float angle)
    {
        var kx = axis.X;
        var ky = axis.Y;
        var kz = axis.Z;

        var (s, c) = Mathf.SinCos(angle);

        var skew = new Mat4x4(
            0, -kz, ky, 0,
            kz, 0, -kx, 0,
            -ky, kx, 0, 0,
            0, 0, 0, 0
        );
        return Identity + s * skew + (1 - c) * skew * skew;
    }

    public void ApplyRotation(Quat rotation)
    {
        this = Rotated(rotation);
    }

    public Mat4x4 Rotated(Quat rotation) => FromRotation(rotation) * this;

    public Quat GetRotation()
    {
        ApplyScale(1 / GetScale());
        return Quat.FromRotationMatrix(this);
    }

    public void ApplyTranslation(Vec3 translation)
    {
        M14 += translation.X;
        M24 += translation.Y;
        M34 += translation.Z;
    }

    public Mat4x4 Translated(Vec3 translation)
    {
        var result = this;
        result.ApplyTranslation(translation);
        return result;
    }

    public Vec3 Translation
    {
        get => (Vec3)Column4;
        set
        {
            M14 = value.X;
            M24 = value.Y;
            M34 = value.Z;
        }
    }

    public void ApplyScale(Vec3 scale)
    {
        M11 *= scale.X;
        M21 *= scale.X;
        M31 *= scale.X;
        M12 *= scale.Y;
        M22 *= scale.Y;
        M32 *= scale.Y;
        M13 *= scale.Z;
        M23 *= scale.Z;
        M33 *= scale.Z;
    }

    public Mat4x4 Scaled(Vec3 scale)
    {
        var result = this;
        result.ApplyScale(scale);
        return result;
    }

    public Vec3 GetScale() => new(
        new Vec3(M11, M21, M31).Length(),
        new Vec3(M12, M22, M32).Length(),
        new Vec3(M13, M23, M33).Length()
    );

    public (Vec3 Scale, Quat Rotation) GetScaleAndRotation()
    {
        var scale = GetScale();
        var clone = Scaled(1 / scale);
        return (scale, Quat.FromRotationMatrix(clone));
    }

    public static Mat4x4 FromRotationX(float angle)
    {
        var (s, c) = Mathf.SinCos(angle);
        return new(
            1, 0, 0, 0,
            0, c, -s, 0,
            0, s, c, 0,
            0, 0, 0, 1
        );
    }

    public static Mat4x4 FromRotationY(float angle)
    {
        var (s, c) = Mathf.SinCos(angle);
        return new(
            c, 0, s, 0,
            0, 1, 0, 0,
            -s, 0, c, 0,
            0, 0, 0, 1
        );
    }

    public static Mat4x4 FromRotationZ(float angle)
    {
        var (s, c) = Mathf.SinCos(angle);
        return new(
            c, -s, 0, 0,
            s, c, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1
        );
    }

    public Vec3 GetEulerAngles() => new(
        Mathf.Atan2(M32, M33),
        Mathf.Atan2(-M31, Mathf.Sqrt(M32 * M32 + M33 * M33)),
        Mathf.Atan2(M21, M11)
    );

    public static Mat4x4 FromEuler(float x, float y, float z)
    {
        var (sx, cx) = Mathf.SinCos(x);
        var (sy, cy) = Mathf.SinCos(y);
        var (sz, cz) = Mathf.SinCos(z);
        return new(
            cy * cz, -cy * sz, sy, 0,
            sx * sy * cz + cx * sz, -sx * sy * sz + cx * cz, -sx * cy, 0,
            -cx * sy * cz + sx * sz, cx * cy * sz + sx * cz, cx * cy, 0,
            0, 0, 0, 1
        );
    }

    public static Mat4x4 FromEuler(Vec3 euler) => FromEuler(euler.X, euler.Y, euler.Z);

    public bool IsEqualApprox(Mat4x4 other, float tolerance = Single.Epsilon) =>
        Row1.IsEqualApprox(other.Row1, tolerance) &&
        Row2.IsEqualApprox(other.Row2, tolerance) &&
        Row3.IsEqualApprox(other.Row3, tolerance) &&
        Row4.IsEqualApprox(other.Row4, tolerance);

    public bool IsZeroApprox(float tolerance = Single.Epsilon) => Row1.IsZeroApprox(tolerance) &&
                                                                  Row2.IsZeroApprox(tolerance) &&
                                                                  Row3.IsZeroApprox(tolerance) &&
                                                                  Row4.IsZeroApprox(tolerance);

    public Mat4x4 Transpose() => new(
        M11, M21, M31, M41,
        M12, M22, M32, M42,
        M13, M23, M33, M43,
        M14, M24, M34, M44
    );

    public bool TryInverse(out Mat4x4 result)
    {
        result = default;
        var det = Determinant();
        if (Mathf.IsZeroApprox(det)) return false;

        var invDet = 1 / det;

        result.M11 = (M22 * (M33 * M44 - M34 * M43) -
                      M23 * (M32 * M44 - M34 * M42) +
                      M24 * (M32 * M43 - M33 * M42)) * invDet;

        result.M12 = -(M12 * (M33 * M44 - M34 * M43) -
                       M13 * (M32 * M44 - M34 * M42) +
                       M14 * (M32 * M43 - M33 * M42)) * invDet;

        result.M13 = (M12 * (M23 * M44 - M24 * M43) -
                      M13 * (M22 * M44 - M24 * M42) +
                      M14 * (M22 * M43 - M23 * M42)) * invDet;

        result.M14 = -(M12 * (M23 * M34 - M24 * M33) -
                       M13 * (M22 * M34 - M24 * M32) +
                       M14 * (M22 * M33 - M23 * M32)) * invDet;

        result.M21 = -(M21 * (M33 * M44 - M34 * M43) -
                       M23 * (M31 * M44 - M34 * M41) +
                       M24 * (M31 * M43 - M33 * M41)) * invDet;

        result.M22 = (M11 * (M33 * M44 - M34 * M43) -
                      M13 * (M31 * M44 - M34 * M41) +
                      M14 * (M31 * M43 - M33 * M41)) * invDet;

        result.M23 = -(M11 * (M23 * M44 - M24 * M43) -
                       M13 * (M21 * M44 - M24 * M41) +
                       M14 * (M21 * M43 - M23 * M41)) * invDet;

        result.M24 = (M11 * (M23 * M34 - M24 * M33) -
                      M13 * (M21 * M34 - M24 * M31) +
                      M14 * (M21 * M33 - M23 * M31)) * invDet;

        result.M31 = (M21 * (M32 * M44 - M34 * M42) -
                      M22 * (M31 * M44 - M34 * M41) +
                      M24 * (M31 * M42 - M32 * M41)) * invDet;

        result.M32 = -(M11 * (M32 * M44 - M34 * M42) -
                       M12 * (M31 * M44 - M34 * M41) +
                       M14 * (M31 * M42 - M32 * M41)) * invDet;

        result.M33 = (M11 * (M22 * M44 - M24 * M42) -
                      M12 * (M21 * M44 - M24 * M41) +
                      M14 * (M21 * M42 - M22 * M41)) * invDet;

        result.M34 = -(M11 * (M22 * M34 - M24 * M32) -
                       M12 * (M21 * M34 - M24 * M31) +
                       M14 * (M21 * M32 - M22 * M31)) * invDet;

        result.M41 = -(M21 * (M32 * M43 - M33 * M42) -
                       M22 * (M31 * M43 - M33 * M41) +
                       M23 * (M31 * M42 - M32 * M41)) * invDet;

        result.M42 = (M11 * (M32 * M43 - M33 * M42) -
                      M12 * (M31 * M43 - M33 * M41) +
                      M13 * (M31 * M42 - M32 * M41)) * invDet;

        result.M43 = -(M11 * (M22 * M43 - M23 * M42) -
                       M12 * (M21 * M43 - M23 * M41) +
                       M13 * (M21 * M42 - M22 * M41)) * invDet;

        result.M44 = (M11 * (M22 * M33 - M23 * M32) -
                      M12 * (M21 * M33 - M23 * M31) +
                      M13 * (M21 * M32 - M22 * M31)) * invDet;

        return true;
    }

    public Mat4x4 Inverse()
    {
        TryInverse(out var result);
        return result;
    }

    public float Determinant()
    {
        var det = 0f;
        det += M11 * (
            M22 * (M33 * M44 - M34 * M43) -
            M23 * (M32 * M44 - M34 * M42) +
            M24 * (M32 * M43 - M33 * M42)
        );

        det -= M12 * (
            M21 * (M33 * M44 - M34 * M43) -
            M23 * (M31 * M44 - M34 * M41) +
            M24 * (M31 * M43 - M33 * M41)
        );

        det += M13 * (
            M21 * (M32 * M44 - M34 * M42) -
            M22 * (M31 * M44 - M34 * M41) +
            M24 * (M31 * M42 - M32 * M41)
        );

        det -= M14 * (
            M21 * (M32 * M43 - M33 * M42) -
            M22 * (M31 * M43 - M33 * M41) +
            M23 * (M31 * M42 - M32 * M41)
        );

        return det;
    }

    public bool IsSingular() => Mathf.IsZeroApprox(Determinant());

    public static Mat4x4 operator -(Mat4x4 a) => new(-a.M11, -a.M12, -a.M13, -a.M14, -a.M21, -a.M22, -a.M23, -a.M24,
        -a.M31, -a.M32, -a.M33, -a.M34, -a.M41, -a.M42, -a.M43, -a.M44);

    public static Mat4x4 operator +(Mat4x4 a, Mat4x4 b) => new(a.M11 + b.M11, a.M12 + b.M12, a.M13 + b.M13,
        a.M14 + b.M14, a.M21 + b.M21, a.M22 + b.M22, a.M23 + b.M23, a.M24 + b.M24, a.M31 + b.M31, a.M32 + b.M32,
        a.M33 + b.M33, a.M34 + b.M34, a.M41 + b.M41, a.M42 + b.M42, a.M43 + b.M43, a.M44 + b.M44);

    public static Mat4x4 operator -(Mat4x4 a, Mat4x4 b) => new(a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13,
        a.M14 - b.M14, a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23, a.M24 - b.M24, a.M31 - b.M31, a.M32 - b.M32,
        a.M33 - b.M33, a.M34 - b.M34, a.M41 - b.M41, a.M42 - b.M42, a.M43 - b.M43, a.M44 - b.M44);

    public static Mat4x4 operator *(Mat4x4 a, Mat4x4 b) => new(
        a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41,
        a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42,
        a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43,
        a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44,
        a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41,
        a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42,
        a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43,
        a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44,
        a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41,
        a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42,
        a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43,
        a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44,
        a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41,
        a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42,
        a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43,
        a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44
    );

    public static Mat4x4 operator *(Mat4x4 a, float b) => new(a.M11 * b, a.M12 * b, a.M13 * b, a.M14 * b, a.M21 * b,
        a.M22 * b, a.M23 * b, a.M24 * b, a.M31 * b, a.M32 * b, a.M33 * b, a.M34 * b, a.M41 * b, a.M42 * b, a.M43 * b,
        a.M44 * b);

    public static Mat4x4 operator *(float a, Mat4x4 b) => b * a;

    public static Mat4x4 operator /(Mat4x4 a, float b) => new(a.M11 / b, a.M12 / b, a.M13 / b, a.M14 / b, a.M21 / b,
        a.M22 / b, a.M23 / b, a.M24 / b, a.M31 / b, a.M32 / b, a.M33 / b, a.M34 / b, a.M41 / b, a.M42 / b, a.M43 / b,
        a.M44 / b);

    public static Mat4x4 operator /(float a, Mat4x4 b) => new(a / b.M11, a / b.M12, a / b.M13, a / b.M14, a / b.M21,
        a / b.M22, a / b.M23, a / b.M24, a / b.M31, a / b.M32, a / b.M33, a / b.M34, a / b.M41, a / b.M42, a / b.M43,
        a / b.M44);

    public static Vec3 operator *(Mat4x4 a, Vec3 b) => new(
        a.M11 * b.X + a.M12 * b.Y + a.M13 * b.Z + a.M14,
        a.M21 * b.X + a.M22 * b.Y + a.M23 * b.Z + a.M24,
        a.M31 * b.X + a.M32 * b.Y + a.M33 * b.Z + a.M34
    );

    public static Vec4 operator *(Mat4x4 a, Vec4 b) => new(
        a.M11 * b.X + a.M12 * b.Y + a.M13 * b.Z + a.M14 * b.W,
        a.M21 * b.X + a.M22 * b.Y + a.M23 * b.Z + a.M24 * b.W,
        a.M31 * b.X + a.M32 * b.Y + a.M33 * b.Z + a.M34 * b.W,
        a.M41 * b.X + a.M42 * b.Y + a.M43 * b.Z + a.M44 * b.W
    );

    public float this[int x, int y]
    {
        get
        {
            if (x is < 0 or >= 4 && y is < 0 or >= 4) throw new IndexOutOfRangeException();
            unsafe
            {
                fixed (float* ptr = &M11) return ptr[x + y * 4];
            }
        }
        set
        {
            if (x is < 0 or >= 4 && y is < 0 or >= 4) throw new IndexOutOfRangeException();
            unsafe
            {
                fixed (float* ptr = &M11) ptr[x + y * 4] = value;
            }
        }
    }

    public Vec4 this[int index]
    {
        get
        {
            if (index is < 0 or >= 4) throw new IndexOutOfRangeException();
            unsafe
            {
                fixed (float* ptr = &M11) return ((Vec4*)ptr)[index];
            }
        }
        set
        {
            if (index is < 0 or >= 4) throw new IndexOutOfRangeException();
            unsafe
            {
                fixed (float* ptr = &M11) ((Vec4*)ptr)[index] = value;
            }
        }
    }

    public static bool operator ==(Mat4x4 a, Mat4x4 b) => a.M11 == b.M11 && a.M21 == b.M21 && a.M31 == b.M31 &&
                                                          a.M41 == b.M41 && a.M12 == b.M12 && a.M22 == b.M22 &&
                                                          a.M32 == b.M32 && a.M42 == b.M42 && a.M13 == b.M13 &&
                                                          a.M23 == b.M23 && a.M33 == b.M33 && a.M43 == b.M43 &&
                                                          a.M14 == b.M14 && a.M24 == b.M24 && a.M34 == b.M34 &&
                                                          a.M44 == b.M44;

    public static bool operator !=(Mat4x4 a, Mat4x4 b) => !(a == b);

    public bool Equals(Mat4x4 other) => this == other;
    public override bool Equals(object? obj) => obj is Mat4x4 m && this == m;

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(M11);
        hash.Add(M21);
        hash.Add(M31);
        hash.Add(M41);
        hash.Add(M12);
        hash.Add(M22);
        hash.Add(M32);
        hash.Add(M42);
        hash.Add(M13);
        hash.Add(M23);
        hash.Add(M33);
        hash.Add(M43);
        hash.Add(M14);
        hash.Add(M24);
        hash.Add(M34);
        hash.Add(M44);
        return hash.ToHashCode();
    }

    public override string ToString() => ToString("G");
    public string ToString(string? format) => ToString(format, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var builder = new StringBuilder();

        builder.Append('(');
        builder.Append(Row1.ToString(format, formatProvider));
        builder.Append(Row2.ToString(format, formatProvider));
        builder.Append(Row3.ToString(format, formatProvider));
        builder.Append(Row4.ToString(format, formatProvider));
        builder.Append(')');

        return builder.ToString();
    }

    public static Mat4x4 Identity => new(
        1, 0, 0, 0,
        0, 1, 0, 0,
        0, 0, 1, 0,
        0, 0, 0, 1
    );

    public Vec4 Row1
    {
        get => new(M11, M12, M13, M14);
        set
        {
            M11 = value.X;
            M12 = value.Y;
            M13 = value.Z;
            M14 = value.W;
        }
    }

    public Vec4 Row2
    {
        get => new(M21, M22, M23, M24);
        set
        {
            M21 = value.X;
            M22 = value.Y;
            M23 = value.Z;
            M24 = value.W;
        }
    }

    public Vec4 Row3
    {
        get => new(M31, M32, M33, M34);
        set
        {
            M31 = value.X;
            M32 = value.Y;
            M33 = value.Z;
            M34 = value.W;
        }
    }

    public Vec4 Row4
    {
        get => new(M41, M42, M43, M44);
        set
        {
            M41 = value.X;
            M42 = value.Y;
            M43 = value.Z;
            M44 = value.W;
        }
    }

    public Vec4 Column1
    {
        get => new(M11, M21, M31, M41);
        set
        {
            M11 = value.X;
            M21 = value.Y;
            M31 = value.Z;
            M41 = value.W;
        }
    }

    public Vec4 Column2
    {
        get => new(M12, M22, M32, M42);
        set
        {
            M12 = value.X;
            M22 = value.Y;
            M32 = value.Z;
            M42 = value.W;
        }
    }

    public Vec4 Column3
    {
        get => new(M13, M23, M33, M43);
        set
        {
            M13 = value.X;
            M23 = value.Y;
            M33 = value.Z;
            M43 = value.W;
        }
    }

    public Vec4 Column4
    {
        get => new(M14, M24, M34, M44);
        set
        {
            M14 = value.X;
            M24 = value.Y;
            M34 = value.Z;
            M44 = value.W;
        }
    }
}