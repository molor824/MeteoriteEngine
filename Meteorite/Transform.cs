namespace Meteorite;

using System.Collections.ObjectModel;

public class Transform : GameObject
{
    public vec3 LocalPosition;
    public vec3 LocalScale = new(1);
    public quat LocalRotation = quat.Identity;
    public mat4 Matrix
    {
        get
        {
            var matrix = LocalMatrix;
            var parent = Parent as Transform;

            while (parent != null)
            {
                // this way you get translation from the parent to child
                matrix = parent.LocalMatrix * matrix;
                parent = parent.Parent as Transform;
            }

            return matrix;
        }
    }
    public mat4 LocalMatrix =>
        mat4.Translate(LocalPosition) *
        glm.ToMat4(LocalRotation) *
        mat4.Scale(LocalScale)
    ;
    public vec3 Position
    {
        get
        {
            var position = LocalPosition;
            var parent = Parent as Transform;

            while (parent != null)
            {
                // position is affected by both rotation and scale
                // it seem to make sense by scaling the position and then rotating it
                position *= parent.LocalScale;
                position = LocalRotation * position;
                position += parent.LocalPosition;
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
                position -= parent.LocalPosition;
                // changing multipication order should reverse it
                position = position * LocalRotation;
                position /= parent.LocalScale;
                parent = parent.Parent as Transform;
            }

            LocalPosition = position;
        }
    }
    public vec3 LossyScale
    {
        get
        {
            var scale = LocalScale;
            var parent = Parent as Transform;

            while (parent != null)
            {
                // scale is just multiplied by its parent scale
                // i dont i can make scale change according to its rotation
                // if i add rotation on the effect, it becomes shearing, not scaling
                scale *= parent.LocalScale;
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
                scale /= parent.LocalScale;
                parent = parent.Parent as Transform;
            }

            LocalScale = scale;
        }
    }
    public quat Rotation
    {
        get
        {
            var rotation = LocalRotation;
            var parent = Parent as Transform;

            while (parent != null)
            {
                // rotate the rotation by its parent's rotation
                rotation = parent.LocalRotation * rotation;
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
                rotation = rotation * parent.LocalRotation;
                parent = parent.Parent as Transform;
            }

            LocalRotation = rotation;
        }
    }
}