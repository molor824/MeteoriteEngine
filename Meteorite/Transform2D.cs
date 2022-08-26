namespace Meteorite;

public class Transform2D : Transform
{
    public Transform Transform3D => (Transform)this;
    public new vec2 Position
    {
        get => (vec2)base.Position;
        set { base.Position.xy = value; }
    }
    public new vec2 Scale
    {
        get => (vec2)base.Scale;
        set { base.Scale.xy = value; }
    }
    public new float Rotation
    {
        get => (float)base.Rotation.EulerAngles.z * MathConst.Rad2Deg;
        set { base.Rotation = quat.FromAxisAngle(value * MathConst.Deg2Rad, new(0, 0, 1)); }
    }
    public float LocalLayer
    {
        get => base.Position.z;
        set { base.Position.z = value; }
    }
    public new vec2 GlobalPosition
    {
        get => (vec2)base.Position;
        set { base.Position = new(value, base.Position.z); }
    }
    public new vec2 LossyScale
    {
        get => (vec2)base.LossyScale;
        set { base.LossyScale = new(value, base.LossyScale.z); }
    }
    public new float GlobalRotation
    {
        get => (float)base.Rotation.EulerAngles.z * MathConst.Rad2Deg;
        set { base.Rotation = quat.FromAxisAngle(value * MathConst.Deg2Rad, new(0, 0, 1)); }
    }
    public float Layer
    {
        get
        {
            var layer = base.Position.z;
            var parent = Parent;

            while (parent != null)
            {
                if (parent is Transform2D p) layer += p.LocalLayer;
                parent = parent.Parent;
            }

            return layer;
        }
        set
        {
            var parent = Parent;

            while (parent != null)
            {
                if (parent is Transform2D p) value -= p.LocalLayer;
                parent = parent.Parent;
            }

            base.Position.z = value;
        }
    }
}