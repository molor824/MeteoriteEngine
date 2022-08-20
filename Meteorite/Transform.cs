namespace Meteorite;

using System.Collections.ObjectModel;

public class Transform : Node
{
    public vec3 Position;
    public vec3 Scale = new(1);
    public quat Rotation = quat.Identity;
    public mat4 GlobalTransformMatrix
    {
        get
        {
            var matrix = TransformMatrix;
            var parent = Parent as Transform;

            while (parent != null)
            {
                // this way you get translation from the parent to child
                matrix = parent.TransformMatrix * matrix;
                parent = parent.Parent as Transform;
            }

            return matrix;
        }
    }
    public mat4 TransformMatrix =>
        mat4.Translate(Position) *
        glm.ToMat4(Rotation) *
        mat4.Scale(Scale)
    ;
    public vec3 GlobalPosition
    {
        get
        {
            var position = Position;
            var parent = Parent as Transform;

            while (parent != null)
            {
                // position is affected by both rotation and scale
                // it seem to make sense by scaling the position and then rotating it
                position *= parent.Scale;
                position = Rotation * position;
                position += parent.Position;
                parent = parent.Parent as Transform;
            }

            return position;
        }
        set
        {
            var position = value;
            var parent = Parent as Transform;

            while (parent != null)
            {
                // same order but in reverse
                position -= parent.Position;
                // changing multipication order should reverse it
                position = position * Rotation;
                position /= parent.Scale;
                parent = parent.Parent as Transform;
            }

            Position = position;
        }
    }
    public vec3 LossyScale
    {
        get
        {
            var scale = Scale;
            var parent = Parent as Transform;

            while (parent != null)
            {
                // scale is just multiplied by its parent scale
                // i dont i can make scale change according to its rotation
                // if i add rotation on the effect, it becomes shearing, not scaling
                scale *= parent.Scale;
                parent = parent.Parent as Transform;
            }

            return scale;
        }
        set
        {
            var scale = value;
            var parent = Parent as Transform;

            while (parent != null)
            {
                // reverse
                scale /= parent.Scale;
                parent = parent.Parent as Transform;
            }

            Scale = scale;
        }
    }
    public quat GlobalRotation
    {
        get
        {
            var rotation = Rotation;
            var parent = Parent as Transform;

            while (parent != null)
            {
                // rotate the rotation by its parent's rotation
                rotation = parent.Rotation * rotation;
                parent = parent.Parent as Transform;
            }

            return rotation;
        }
        set
        {
            var rotation = value;
            var parent = Parent as Transform;

            while (parent != null)
            {
                // reverse
                rotation = rotation * parent.Rotation;
                parent = parent.Parent as Transform;
            }

            Rotation = rotation;
        }
    }
}