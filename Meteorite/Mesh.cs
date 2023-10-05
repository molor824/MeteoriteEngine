using Meteorite.Mathematics;

namespace Meteorite;

using OpenTK.Graphics.OpenGL;

public class Mesh
{
    int _vao, _vertexBuffer, _colorBuffer, _texBuffer, _ebo, _indexCount;

    public int VAO => _vao;
    public int VertexBuffer => _vertexBuffer;
    public int ColorBuffer => _colorBuffer;
    public int TextureBuffer => _texBuffer;
    public int EBO => _ebo;
    public int IndexCount => _indexCount;

    public Mesh(Vec3[] vertices, ushort[] indices)
    {
        Upload(vertices, indices, null, null);
    }
    public Mesh(Vec3[] vertices, ushort[] indices, Vec2[]? texCoords = null, Color[]? vertexColors = null)
	{
        Upload(vertices, indices, texCoords, vertexColors);
    }
    public Mesh(Vec3[] vertices, ushort[] indices, Color[]? vertexColors = null, Vec2[]? texCoords = null)
	{
        Upload(vertices, indices, texCoords, vertexColors);
    }

    public void Render()
    {
		GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, _indexCount, DrawElementsType.UnsignedShort, 0);
	}
    void Upload(Vec3[] vertices, ushort[] indices, Vec2[]? texCoords, Color[]? vertexColors)
    {
        _indexCount = indices.Length;

        // Note to self: Always bind vertex array first
        _vao = GL.GenVertexArray();
		GL.BindVertexArray(_vao);

		unsafe
		{
			_ebo = GL.GenBuffer();

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
			GL.BufferData(
				BufferTarget.ElementArrayBuffer,
                sizeof(ushort) * indices.Length,
                indices,
				BufferUsageHint.StaticDraw
			);

			_vertexBuffer = GL.GenBuffer();

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(
				BufferTarget.ArrayBuffer,
                sizeof(Vec3) * vertices.Length,
                vertices,
				BufferUsageHint.StaticDraw
			);

			GL.VertexAttribPointer(
                0, 3, VertexAttribPointerType.Float, false, 0, 0
			);
			GL.EnableVertexAttribArray(0);

            if (texCoords != null)
            {
                _texBuffer = GL.GenBuffer();

                GL.BindBuffer(BufferTarget.ArrayBuffer, _texBuffer);
                GL.BufferData(
                    BufferTarget.ArrayBuffer,
                    sizeof(Vec2) * texCoords.Length,
                    texCoords,
                    BufferUsageHint.StaticDraw
                );

                GL.VertexAttribPointer(
                    1, 2, VertexAttribPointerType.Float, false, 0, 0
                );
                GL.EnableVertexAttribArray(1);
            }
            if (vertexColors != null)
            {
                _colorBuffer = GL.GenBuffer();

                GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
                GL.BufferData(
                    BufferTarget.ArrayBuffer,
                    sizeof(Color) * vertexColors.Length,
                    vertexColors,
                    BufferUsageHint.StaticDraw
                );

                GL.VertexAttribPointer(
                    2, 4, VertexAttribPointerType.Float, false, 0, 0
                );
                GL.EnableVertexAttribArray(2);
            }
		}

		Log.Success("[ID: {0}] Mesh loaded!", _vao);
	}
	void Unload()
	{
        if (_colorBuffer != 0) GL.DeleteBuffer(_colorBuffer);
		GL.DeleteBuffer(_vertexBuffer);
		GL.DeleteBuffer(_ebo);
		GL.DeleteBuffer(_vao);
	}

	~Mesh()
	{
		Unload();
	}
}