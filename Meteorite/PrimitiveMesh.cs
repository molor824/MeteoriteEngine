using Meteorite.Mathematics;

namespace Meteorite;

public static class PrimitiveMesh
{
    public static Mesh Quad = new(
        new Vec3[]
        {
            new(-0.5f, 0.5f, 0),
            new(0.5f, 0.5f, 0),
            new(0.5f, -0.5f, 0),
            new(-0.5f, -0.5f, 0)
        },
        new ushort[] { 0, 3, 2, 2, 1, 0 },
        new Vec2[]
        {
            new(0, 0),
            new(1, 0),
            new(1, 1),
            new(0, 1)
        }
    );
}