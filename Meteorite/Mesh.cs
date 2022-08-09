namespace Meteorite;

using System.Runtime.InteropServices;

public class Mesh
{
    internal const int MaxMeshVertexBuffers = 7;
    internal Raylib_cs.Mesh Raw;

    public void Unload()
    {
        unsafe
        {
            GCHandle.FromIntPtr((IntPtr)Raw.vertices).Free();
            GCHandle.FromIntPtr((IntPtr)Raw.indices).Free();
            GCHandle.FromIntPtr((IntPtr)Raw.texcoords).Free();
            if (Raw.colors != null) GCHandle.FromIntPtr((IntPtr)Raw.colors).Free();

            Rlgl.rlUnloadVertexArray(Raw.vaoId);
            if (Raw.vboId != null)
            {
                for (var i = 0; i < MaxMeshVertexBuffers; i++)
                {
                    Rlgl.rlUnloadVertexBuffer(Raw.vboId[i]);
                }
            }
        }
    }
    public Mesh(vec3[] vertices, ushort[] indices)
    {
        MeshInit(vertices, indices, new vec2[vertices.Length], null, false);
    }
    public Mesh(vec3[] vertices, ushort[] indices, vec2[] texcoords)
    {
        MeshInit(vertices, indices, texcoords, null, false);
    }
    public Mesh(vec3[] vertices, ushort[] indices, BColor[] colors)
    {
        MeshInit(vertices, indices, new vec2[vertices.Length], colors, false);
    }
    public Mesh(vec3[] vertices, ushort[] indices, vec2[] texcoords, BColor[] colors)
    {
        MeshInit(vertices, indices, texcoords, colors, false);
    }
    void MeshInit(vec3[] vertices, ushort[] indices, vec2[] texcoords, BColor[]? colors, bool dynamic)
    {
        if (vertices.Length != texcoords.Length || colors != null && vertices.Length != colors.Length)
        {
            Log.Panic(
                "Vertices, Texcoords and Colors length does not match.\n" +
                "Vertices: {0}\n" +
                "TexCoords: {1}\n" +
                "Colors: {2}",
                vertices.Length, texcoords.Length, colors?.Length
            );
        }

        unsafe
        {
            Raw.vertexCount = vertices.Length;
            Raw.triangleCount = indices.Length / 3;

            Raw.vertices = (float*)GCHandle.Alloc(vertices, GCHandleType.Pinned).AddrOfPinnedObject();
            Raw.indices = (ushort*)GCHandle.Alloc(indices, GCHandleType.Pinned).AddrOfPinnedObject();
            Raw.texcoords = (float*)GCHandle.Alloc(texcoords, GCHandleType.Pinned).AddrOfPinnedObject();
            Raw.colors = (byte*)GCHandle.Alloc(colors, GCHandleType.Pinned).AddrOfPinnedObject();

            Raylib.UploadMesh(ref Raw, dynamic);
        }
    }
}