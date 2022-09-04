using OpenTK.Mathematics;

namespace Meteorite;

using OpenTK.Graphics.OpenGL;

public class Mesh
{
    int _vao, _vertexBuffer, _colorBuffer, _ebo, _indexCount;

	public Mesh(Vertex[] vertices, ushort[] indices)
	{
        Upload(vertices, indices, null);
	}
    public Mesh(Vertex[] vertices, ushort[] indices, Color4[] vertexColors)
	{
        Upload(vertices, indices, vertexColors);
    }

    public void Render()
    {
		GL.BindVertexArray(_vao);
        GL.DrawElements(PrimitiveType.Triangles, _indexCount, DrawElementsType.UnsignedShort, 0);
	}
    void Upload(Vertex[] vertices, ushort[] indices, Color4[]? vertexColors)
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
                sizeof(Vertex) * vertices.Length,
                vertices,
				BufferUsageHint.StaticDraw
			);

			GL.VertexAttribPointer(
				0, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), 0
			);
			GL.EnableVertexAttribArray(0);

			GL.VertexAttribPointer(
				1, 2, VertexAttribPointerType.Float, false, sizeof(Vertex), sizeof(Vector3)
			);
			GL.EnableVertexAttribArray(1);

            if (vertexColors != null)
			{
                if (vertexColors.Length == vertices.Length)
				{
					_colorBuffer = GL.GenBuffer();

					GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
					GL.BufferData(
						BufferTarget.ArrayBuffer,
                        sizeof(Color4) * vertexColors.Length,
                        vertexColors,
						BufferUsageHint.StaticDraw
					);

					GL.VertexAttribPointer(
						2, 4, VertexAttribPointerType.Float, false, 0, 0
					);
					GL.EnableVertexAttribArray(2);
				}
				else
				{
					Log.Error(
                        $"Vertex colors length is not matching with vertices length ({vertexColors.Length} != {vertices.Length}), didn't set color buffer"
					);
				}
			}
		}
		
		Log.Success("[ID: {0}] Mesh loaded!", _vao);
	}
	void Unload()
	{
		GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
		GL.BindVertexArray(0);

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
public struct Vertex
{
	public Vector3 Position;
	public Vector2 TexCoord;

	public Vertex(Vector3 position)
	{
		Position = position;
		TexCoord = new();
	}
	public Vertex(Vector3 position, Vector2 texcoord)
	{
		Position = position;
		TexCoord = texcoord;
	}
}

