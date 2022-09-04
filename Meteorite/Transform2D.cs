using System.Windows.Markup;

namespace Meteorite;

public class Transform2D : Transform
{
    public Transform Transform3D => (Transform)this;
    public new Vector2 Position
    {
        get => base.Position.Xy;
        set => base.Position.Xy = value;
    }
    public new Vector2 Scale
    {
        get => base.Scale.Xy;
        set => base.Scale.Xy = value;
    }
    public new float Rotation
    {
        get => base.Rotation.ToEulerAngles().Z * MathConst.Rad2Deg;
        set
        {
            var euler = base.Rotation.ToEulerAngles();
            euler.Z = value * MathConst.Deg2Rad;
            base.Rotation = new Quaternion(euler);
        }
    }
    public float LocalLayer
    {
        get => base.Position.Z;
        set => base.Position.Z = value;
    }
    public new Vector2 GlobalPosition
    {
        get => base.GlobalPosition.Xy;
        set
        {
            var pos = base.GlobalPosition;
            pos.Xy = value;
            base.GlobalPosition = pos;
        }
    }
    public new Vector2 LossyScale
    {
        get => base.LossyScale.Xy;
        set
        {
            var scale = base.LossyScale;
            scale.Xy = value;
            base.LossyScale = scale;
        }
    }
    public new float GlobalRotation
    {
        get => base.GlobalRotation.ToEulerAngles().Z * MathConst.Rad2Deg;
        set
        {
            var euler = base.GlobalRotation.ToEulerAngles();
            euler.Z = value * MathConst.Deg2Rad;
            base.GlobalRotation = new Quaternion(euler);
        }
    }
    public float Layer
    {
        get => base.GlobalPosition.Z;
        set
        {
            var pos = base.GlobalPosition;
            pos.Z = value;
            base.GlobalPosition = pos;
        }
    }
}