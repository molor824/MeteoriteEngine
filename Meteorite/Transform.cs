using System.Xml.Serialization;
using OpenTK.Mathematics;

namespace Meteorite;

public class Transform : Node
{
    public Vector3 Position;
    public Vector3 Scale = new(1);
    public Quaternion Rotation = Quaternion.Identity;
    public Matrix4 GlobalTransformMatrix
    {
        get
        {
            var matrix = TransformMatrix;
            var parent = Parent;

            while (parent != null)
            {
                // this way you get translation from the parent to child
                if (parent is Transform p) matrix = p.TransformMatrix * matrix;
                parent = parent.Parent;
            }

            return matrix;
        }
    }

    public Matrix4 TransformMatrix
    {
        get
        {
            // CreateTranslation creates translation in last row instead of last column
            // Not sure why, i thought it was supposed to be the last column
            var translation = Matrix4.Identity;
            var rotation = Matrix4.CreateFromQuaternion(Rotation);
            var scale = Matrix4.CreateScale(Scale);

            translation.M14 = Position.X;
            translation.M24 = Position.Y;
            translation.M34 = Position.Z;

            return translation * rotation * scale;
        }
    }
    public Vector3 GlobalPosition
    {
        get
        {
            var position = Position;
            var parent = Parent;

            while (parent != null)
            {
                // position is affected by both rotation and scale
                // it seem to make sense by scaling the position and then rotating it
                if (parent is Transform p)
                {
                    position *= p.Scale;
                    position = Rotation * position;
                    position += p.Position;
                }

                parent = parent.Parent;
            }

            return position;
        }
        set
        {
            var position = value;
            var parent = Parent;

            while (parent != null)
            {
                if (parent is Transform p)
                {
                    // same order but in reverse
                    position -= p.Position;
                    // changing multipication order should reverse it
                    position = Rotation.Inverted() * position;
                    position /= p.Scale;
                }

                parent = parent.Parent;
            }

            Position = position;
        }
    }
    public Vector3 LossyScale
    {
        get
        {
            var scale = Scale;
            var parent = Parent;

            while (parent != null)
            {
                // scale is just multiplied by its parent scale
                // i dont i can make scale change according to its rotation
                // if i add rotation on the effect, it becomes shearing, not scaling
                if (parent is Transform p) scale *= p.Scale;
                parent = parent.Parent;
            }

            return scale;
        }
        set
        {
            var scale = value;
            var parent = Parent;

            while (parent != null)
            {
                // reverse
                if (parent is Transform p) scale /= p.Scale;
                parent = parent.Parent;
            }

            Scale = scale;
        }
    }
    public Quaternion GlobalRotation
    {
        get
        {
            var rotation = Rotation;
            var parent = Parent;

            while (parent != null)
            {
                // rotate the rotation by its parent's rotation
                if (parent is Transform p) rotation = p.Rotation * rotation;
                parent = parent.Parent;
            }

            return rotation;
        }
        set
        {
            var rotation = value;
            var parent = Parent;

            while (parent != null)
            {
                // reverse
                if (parent is Transform p) rotation = rotation * p.Rotation;
                parent = parent.Parent;
            }

            Rotation = rotation;
        }
    }
}