namespace Meteorite;

public class Transform2D : Transform
{
    public Transform Transform3D => (Transform)this;
    public new vec2 LocalPosition
    {
        get => (vec2)base.LocalPosition;
        set { base.LocalPosition.xy = value; }
    }
    public new vec2 LocalScale
    {
        get => (vec2)base.LocalScale;
        set { base.LocalScale.xy = value; }
    }
    public new float LocalRotation
    {
        get => (float)base.LocalRotation.EulerAngles.z * Raylib.RAD2DEG;
        set { base.LocalRotation = quat.FromAxisAngle(value * Raylib.DEG2RAD, new(0, 0, 1)); }
    }
    public float LocalLayer
    {
        get => base.LocalPosition.z;
        set { base.LocalPosition.z = value; }
    }
    public new vec2 Position
    {
        get => (vec2)base.Position;
        set { base.Position = new(value, base.Position.z); }
    }
    public new vec2 LossyScale
    {
        get => (vec2)base.LossyScale;
        set { base.LossyScale = new(value, base.LossyScale.z); }
    }
    public new float Rotation
    {
        get => (float)base.Rotation.EulerAngles.z * Raylib.RAD2DEG;
        set { base.Rotation = quat.FromAxisAngle(value * Raylib.DEG2RAD, new(0, 0, 1)); }
    }
    public float Layer
    {
        get
        {
            var layer = base.LocalPosition.z;
            var parent = Parent as Transform;

            while (parent != null)
            {
                layer += parent.LocalPosition.z;
                parent = parent.Parent as Transform;
            }

            return layer;
        }
        set
        {
            var parent = Parent as Transform;

            while (parent != null)
            {
                value -= parent.LocalPosition.z;
                parent = parent.Parent as Transform;
            }

            base.LocalPosition.z = value;
        }
    }
}