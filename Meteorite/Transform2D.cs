using Meteorite.Mathematics;

namespace Meteorite;

public class Transform2D : Transform3D
{
    public float Layer
    {
        get => Transform.M34;
        set => Transform.M34 = value;
    }

    public float GlobalLayer
    {
        get => GlobalTransform.M34;
        set
        {
            var t = GlobalTransform;
            t.M34 = value;
            GlobalTransform = t;
        }
    }

    public new Vec2 Position
    {
        get => (Vec2)base.Position;
        set => base.Position = new(value, base.Position.Z);
    }

    public new float Rotation
    {
        get => Transform.GetEulerAngles().Z;
        set
        {
            var euler = Transform.GetEulerAngles();
            base.Rotation = Quat.FromEulerAngles(new(euler.X, euler.Y, value));
        }
    }

    public new Vec2 Scale
    {
        get => (Vec2)base.Scale;
        set => base.Scale = new(value, base.Scale.Z);
    }

    public void ApplyTranslation(Vec2 translation)
    {
        base.ApplyTranslation(new(translation, 0));
    }

    public void ApplyRotation(float rotation)
    {
        base.ApplyRotation(Quat.FromRotationZ(rotation));
    }

    public void ApplyScale(Vec2 scale)
    {
        base.ApplyScale(new(scale, 1));
    }

    public new Vec2 GlobalPosition
    {
        get => (Vec2)base.GlobalPosition;
        set
        {
            var position = base.GlobalPosition;
            base.GlobalPosition = new(value, position.Z);
        }
    }

    public new Vec2 GlobalScale
    {
        get => (Vec2)base.GlobalScale;
        set
        {
            var scale = base.GlobalScale;
            base.GlobalScale = new(value, scale.Z);
        }
    }

    public new float GlobalRotation
    {
        get => GlobalTransform.GetEulerAngles().Z;
        set
        {
            var t = GlobalTransform;
            GlobalTransform = Mat4x4.FromTransformation(t.Translation, Quat.FromRotationZ(value), t.GetScale());
        }
    }

    public void ApplyGlobalTranslation(Vec2 translation)
    {
        base.ApplyGlobalTranslation(new(translation, 0));
    }

    public void ApplyGlobalRotation(float rotation)
    {
        base.ApplyGlobalRotation(Quat.FromRotationZ(rotation));
    }

    public void ApplyGlobalScale(Vec2 scale)
    {
        base.ApplyGlobalScale(new(scale, 1));
    }
}