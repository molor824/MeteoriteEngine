namespace Meteorite;

using OpenTK.Graphics.OpenGL;

public class Mesh
{
    public Shader Shader = new();

	ushort[] _indices;
	Vertex[] _vertices;
	Color[]? _colors;
	int _vao, _vertexBuffer, _colorBuffer, _ebo;

	public Mesh(Vertex[] vertices, ushort[] indices)
	{
		_vertices = vertices;
		_indices = indices;
		Upload();
	}
	public Mesh(Vertex[] vertices, ushort[] indices, Color[]? vertexColors)
	{
		_vertices = vertices;
		_indices = indices;
		_colors = vertexColors;
		Upload();
    }

    public void Render(mat4 transform)
    {
	    GL.UseProgram(Shader.Program);
	    
	    unsafe
	    {
		    GL.UniformMatrix4(Shader.GetUniformLocation("transform"), 1, false, (float*)&transform);
	    }

		GL.BindVertexArray(_vao);
		GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedShort, _ebo);
	}
	void Upload()
	{
		if (_vao != 0) return;
		
		// Note to self: Always bind vertex array first
		_vao = GL.GenVertexArray();
		GL.BindVertexArray(_vao);

		unsafe
		{
			_ebo = GL.GenBuffer();

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
			GL.BufferData(
				BufferTarget.ElementArrayBuffer,
				sizeof(ushort) * _indices.Length,
				_indices,
				BufferUsageHint.StaticDraw
			);

			_vertexBuffer = GL.GenBuffer();

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
			GL.BufferData(
				BufferTarget.ArrayBuffer,
				sizeof(Vertex) * _vertices.Length,
				_vertices,
				BufferUsageHint.StaticDraw
			);

			GL.VertexAttribPointer(
				0, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), 0
			);
			GL.EnableVertexAttribArray(0);

			GL.VertexAttribPointer(
				1, 2, VertexAttribPointerType.Float, false, sizeof(Vertex), sizeof(vec3)
			);
			GL.EnableVertexAttribArray(1);

			if (_colors != null)
			{
				if (_colors.Length == _vertices.Length)
				{
					_colorBuffer = GL.GenBuffer();

					GL.BindBuffer(BufferTarget.ArrayBuffer, _colorBuffer);
					GL.BufferData(
						BufferTarget.ArrayBuffer,
						sizeof(vec3) * _colors.Length,
						_colors,
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
						$"Vertex colors length is not matching with vertices length ({_colors.Length} != {_vertices.Length}), didn't set color buffer"
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

		if (_colors != null) GL.DeleteBuffer(_colorBuffer);
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
	public vec3 Position;
	public vec2 TexCoord;

	public Vertex(vec3 position)
	{
		Position = position;
		TexCoord = new();
	}
	public Vertex(vec3 position, vec2 texcoord)
	{
		Position = position;
		TexCoord = texcoord;
	}
}

