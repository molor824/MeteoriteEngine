namespace Meteorite;

public static class PrimitiveMesh
{
    public static Mesh UIQuad = new(
        new vec3[]
        {
            new(-1f, 1f, 0),
            new(1f, 1f, 0),
            new(1f, -1f, 0),
            new(-1f, -1f, 0)
        },
        new ushort[] { 0, 3, 2, 2, 1, 0 },
        new vec2[]
        {
            new(0, 0),
            new(1, 0),
            new(1, 1),
            new(0, 1)
        }
    );

    public static Mesh Quad = new(
        new vec3[]
        {
            new(-0.5f, 0.5f, 0),
            new(0.5f, 0.5f, 0),
            new(0.5f, -0.5f, 0),
            new(-0.5f, -0.5f, 0)
        },
        new ushort[] { 0, 3, 2, 2, 1, 0 },
        new vec2[]
        {
            new(0, 0),
            new(1, 0),
            new(1, 1),
            new(0, 1)
        }
    );
}