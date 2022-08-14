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
        get => (float)base.Rotation.EulerAngles.z * Raylib.RAD2DEG;
        set { base.Rotation = quat.FromAxisAngle(value * Raylib.DEG2RAD, new(0, 0, 1)); }
    }
    public float Layer
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
        get => (float)base.Rotation.EulerAngles.z * Raylib.RAD2DEG;
        set { base.Rotation = quat.FromAxisAngle(value * Raylib.DEG2RAD, new(0, 0, 1)); }
    }
    public float GlobalLayer
    {
        get
        {
            var layer = base.Position.z;
            var parent = Parent as Transform;

            while (parent != null)
            {
                layer += parent.Position.z;
                parent = parent.Parent as Transform;
            }

            return layer;
        }
        set
        {
            var parent = Parent as Transform;

            while (parent != null)
            {
                value -= parent.Position.z;
                parent = parent.Parent as Transform;
            }

            base.Position.z = value;
        }
    }
}