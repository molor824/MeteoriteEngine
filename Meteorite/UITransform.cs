namespace Meteorite;

/// <summary>
/// Transformation used for GUI.
/// <para>Position and Scaling contains absolute and normalized version.</para>
/// <para>Absolute: Measured in pixel units. Great for offsetting the UI by absolute value.</para>
/// <para>Normalized: Measured in normal of screen size. Great for centering, cornering the UI.</para>
/// <example>
/// X button on top right:
/// <code>
/// Absolute position: (offset, offset)
/// Normalized position: (0, 1)
/// Absolute scale: (some scale, some scale)
/// Normalized scale: (0, 0)
/// Pivot: (0, 1)
/// </code>
/// </example>
/// </summary>
public class UITransform : Node
{
    // will have absolute position and normal position
    // same applies to scaling
    // there will be pivot
    /// <summary>
    /// Position in pixels.
    /// (0, 0) being top left and (width, height) being bottom right.
    /// </summary>
    public vec2 AbsolutePosition;
    /// <summary>
    /// Position in normals.
    /// (0, 0) being top left and (1, 1) being bottom right.
    /// Absolute position is added on top of this.
    /// </summary>
    public vec2 NormalizedPosition;
    /// <summary>
    /// Scale in pixels.
    /// (0, 0) meaning 0 scale and (width, height) will cover the entire screen.
    /// </summary>
    public vec2 AbsoluteScale;
    /// <summary>
    /// Scale in normals.
    /// (0, 0) meaning 0 scale and (1, 1) will cover the entire screen.
    /// Absolute scale is added on top of this.
    /// </summary>
    public vec2 NormalizedScale;
    /// <summary>
    /// Pivot of the position point.
    /// (0, 0) meaning top left of the current UI and (1, 1) meaning bottom right of the current UI.
    /// </summary>
    public vec2 Pivot = new(0.5f);

    public mat4 TransformMatrix
    {
        get
        {
            var pos = (NormalizedPosition - new vec2(0.5f)) * 2 + AbsolutePosition / Game.WindowSize;
            var scale = NormalizedScale + AbsoluteScale / Game.WindowSize;
            var pivot = (Pivot - new vec2(0.5f)) * -2;
            var transform = mat4.Scale(scale.x, scale.y, 1) * mat4.Translate(pivot.x, pivot.y, 0);

            Log.Print("{0}: {1}", Name, pivot);

            transform.m30 = pos.x;
            transform.m31 = -pos.y;

            return transform;
        }
    }
    public mat4 GlobalTransformMatrix
    {
        get
        {
            var matrix = TransformMatrix;
            var parent = Parent;

            while (parent != null)
            {
                // this way you get translation from the parent to child
                if (parent is UITransform p) matrix = p.TransformMatrix * matrix;
                parent = parent.Parent;
            }

            return matrix;
        }
    }
}