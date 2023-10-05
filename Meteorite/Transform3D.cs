using Meteorite.Mathematics;

namespace Meteorite;

public class Transform3D : Node
{
    public Vec3 Position
    {
        get => Transform.Translation;
        set => Transform.Translation = value;
    }

    public Quat Rotation
    {
        get => Transform.GetRotation();
        set => Transform = Mat4x4.FromTransformation(Position, value, Scale);
    }

    public Vec3 Scale
    {
        get => Transform.GetScale();
        set => Transform = Mat4x4.FromTransformation(Position, Rotation, value);
    }

    public Vec3 GlobalPosition
    {
        get => GlobalTransform.Translation;
        set
        {
            var transform = GlobalTransform;
            transform.Translation = value;
            GlobalTransform = transform;
        }
    }

    public Vec3 GlobalScale
    {
        get => GlobalTransform.GetScale();
        set
        {
            var transform = GlobalTransform;
            GlobalTransform = Mat4x4.FromTransformation(transform.Translation, transform.GetRotation(), value);
        }
    }

    public Quat GlobalRotation
    {
        get => GlobalTransform.GetRotation();
        set
        {
            var transform = GlobalTransform;
            GlobalTransform = Mat4x4.FromTransformation(transform.Translation, value, transform.GetScale());
        }
    }

    public void ApplyTranslation(Vec3 translation)
    {
        Transform.ApplyTranslation(translation);
    }

    public void ApplyRotation(Quat rotation)
    {
        var temp = Transform.Translation;
        Transform.Translation = Vec3.Zero;
        Transform.ApplyRotation(rotation);
        Transform.Translation = temp;
    }

    public void ApplyScale(Vec3 scale)
    {
        var (oldScale, rotation) = Transform.GetScaleAndRotation();
        Transform = Mat4x4.FromTransformation(Transform.Translation, rotation, oldScale * scale);
    }

    public void ApplyGlobalTranslation(Vec3 translation)
    {
        GlobalTransform = GlobalTransform.Translated(translation);
    }

    public void ApplyGlobalRotation(Quat rotation)
    {
        var transform = GlobalTransform;
        var temp = transform.Translation;
        transform.Translation = Vec3.Zero;
        transform.ApplyRotation(rotation);
        transform.Translation = temp;
        GlobalTransform = transform;
    }

    public void ApplyGlobalScale(Vec3 scale)
    {
        var transform = GlobalTransform;
        transform.Translation = Vec3.Zero;
        var (oldScale, rotation) = transform.GetScaleAndRotation();
        GlobalTransform = Mat4x4.FromTransformation(transform.Translation, rotation, oldScale * scale);
    }

    public Mat4x4 Transform = Mat4x4.Identity;

    public Mat4x4 GlobalTransform
    {
        get => LocalToGlobalTransform() * Transform;
        set => Transform = LocalToGlobalTransform().Inverse() * value;
    }

    public Mat4x4 LocalToGlobalTransform()
    {
        var transform = Mat4x4.Identity;
        var parent = Parent;
        while (parent is Transform3D p)
        {
            transform = p.Transform * transform;
            parent = parent.Parent;
        }

        return transform;
    }
}