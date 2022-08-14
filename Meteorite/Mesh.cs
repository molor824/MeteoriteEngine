namespace Meteorite;

using System.Runtime.InteropServices;

public class Mesh : IDisposable
{
    internal const int MaxMeshVertexBuffers = 7;
    internal Raylib_cs.Mesh Raw;

    void IDisposable.Dispose()
    {
        Unload();
    }
    public void Unload()
    {
        unsafe
        {
            Raylib.UnloadMesh(ref Raw);
        }
    }
    public Mesh(IEnumerable<vec3> vertices, IEnumerable<ushort> indices)
    {
        MeshInit(vertices, indices, new vec2[vertices.Count()], null, false);
    }
    public Mesh(IEnumerable<vec3> vertices, IEnumerable<ushort> indices, IEnumerable<vec2> texcoords)
    {
        MeshInit(vertices, indices, texcoords, null, false);
    }
    public Mesh(IEnumerable<vec3> vertices, IEnumerable<ushort> indices, IEnumerable<BColor> colors)
    {
        MeshInit(vertices, indices, new vec2[vertices.Count()], colors, false);
    }
    public Mesh(IEnumerable<vec3> vertices, IEnumerable<ushort> indices, IEnumerable<vec2> texcoords, IEnumerable<BColor> colors)
    {
        MeshInit(vertices, indices, texcoords, colors, false);
    }
    internal Mesh(Raylib_cs.Mesh raw)
    {
        Raw = raw;
    }
    void MeshInit(IEnumerable<vec3> vertices, IEnumerable<ushort> indices, IEnumerable<vec2> texcoords, IEnumerable<BColor>? colors, bool dynamic)
    {
        if (vertices.Count() != texcoords.Count() || colors != null && vertices.Count() != colors.Count())
        {
            Log.Panic(
                "Vertices, Texcoords and Colors length does not match.\n" +
                "Vertices: {0}\n" +
                "TexCoords: {1}\n" +
                "Colors: {2}",
                vertices.Count(), texcoords.Count(), colors?.Count()
            );
        }

        unsafe
        {
            Raw.vertexCount = vertices.Count();
            Raw.triangleCount = indices.Count() / 3;

            Raw.vertices = (float*)FastAlloc.AllocWith(vertices);
            Raw.indices = FastAlloc.AllocWith(indices);
            Raw.texcoords = (float*)FastAlloc.AllocWith(texcoords);
            if (colors != null) Raw.colors = (byte*)FastAlloc.AllocWith(colors);

            Raylib.UploadMesh(ref Raw, dynamic);
        }
    }
}