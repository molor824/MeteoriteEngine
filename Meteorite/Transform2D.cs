using System.Windows.Markup;

namespace Meteorite;

public class Transform2D : Transform
{
    public Transform Transform3D => this;
    public new vec2 Position
    {
        get => base.Position.xy;
        set => base.Position.xy = value;
    }
    public new vec2 Scale
    {
        get => base.Scale.xy;
        set => base.Scale.xy = value;
    }
    public new float Rotation
    {
        get => (float)base.Rotation.EulerAngles.z * MathConst.Rad2Deg;
        set
        {
            var euler = base.Rotation.EulerAngles;
            euler.z = value * MathConst.Deg2Rad;
            base.Rotation = new quat((vec3)euler);
        }
    }
    public float LocalLayer
    {
        get => base.Position.z;
        set => base.Position.z = value;
    }
    public new vec2 GlobalPosition
    {
        get => base.GlobalPosition.xy;
        set
        {
            var pos = base.GlobalPosition;
            pos.xy = value;
            base.GlobalPosition = pos;
        }
    }
    public new vec2 LossyScale
    {
        get => base.LossyScale.xy;
        set
        {
            var scale = base.LossyScale;
            scale.xy = value;
            base.LossyScale = scale;
        }
    }
    public new float GlobalRotation
    {
        get => (float)base.GlobalRotation.EulerAngles.z * MathConst.Rad2Deg;
        set
        {
            var euler = base.GlobalRotation.EulerAngles;
            euler.z = value * MathConst.Deg2Rad;
            base.GlobalRotation = new quat((vec3)euler);
        }
    }
    public float Layer
    {
        get => base.GlobalPosition.z;
        set
        {
            var pos = base.GlobalPosition;
            pos.z = value;
            base.GlobalPosition = pos;
        }
    }
}