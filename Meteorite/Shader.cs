using System.Runtime.Versioning;

namespace Meteorite;

using OpenTK.Graphics.OpenGL;

public class Shader
{
    string _vertexShader =
@"#version 330 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aColor;

out vec4 fColor;
out vec2 fTexCoord;

uniform mat4 transform;

void main()
{
    gl_Position = transform * vec4(aPos, 1);
    fTexCoord = aTexCoord;
    fColor = vec4(aColor, 1);
}
";
    string _fragmentShader =
@"#version 330 core

out vec4 FragColor;

in vec2 fTexCoord;
in vec4 fColor;

uniform sampler2D texture0;
uniform vec4 textureColor;

void main()
{
    FragColor = texture(texture0, fTexCoord) * fColor * textureColor;
}
";

    internal int Program;

    internal int GetUniformLocation(string location)
    {
	    return GL.GetUniformLocation(Program, location);
    }
    void Upload()
    {
	    if (Program != 0) return;

		var vs = GL.CreateShader(ShaderType.VertexShader);
		var fs = GL.CreateShader(ShaderType.FragmentShader);

		GL.ShaderSource(vs, _vertexShader);
		GL.ShaderSource(fs, _fragmentShader);

		GL.CompileShader(vs);
		GL.CompileShader(fs);

		if (CheckShaderError(vs) is string verr)
		{
			Log.Error("Vertex shader error: {0}", verr);
			return;
		}

		if (CheckShaderError(fs) is string ferr)
		{
			Log.Error("Fragment shader error: {0}", ferr);
			return;
		}

		Program = GL.CreateProgram();

		GL.AttachShader(Program, vs);
		GL.AttachShader(Program, fs);

		GL.LinkProgram(Program);
		
		GL.DeleteShader(vs);
		GL.DeleteShader(fs);

		if (CheckProgramError(Program) is string perr)
		{
			Program = 0;
			Log.Error("Program error: {0}", perr);
			return;
		}
		
		Log.Success("[ID: {0}] Shader program loaded!", Program);
	}

    void Unload()
    {
	    GL.DeleteProgram(Program);
    }
	static string? CheckShaderError(int shader)
	{
		GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);

		if (success == 0) return GL.GetShaderInfoLog(shader);
		return null;
	}
	static string? CheckProgramError(int program)
	{
		GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);

		if (success == 0) return GL.GetProgramInfoLog(program);
		return null;
	}
	public static Shader FromPath(string vsPath, string fsPath)
	{
		return new(ResourceLoader.LoadTextFile(vsPath), ResourceLoader.LoadTextFile(fsPath));
	}

	public Shader()
	{
		Upload();
	}
    public Shader(string vertexShader, string fragmentShader)
	{
		_vertexShader = vertexShader;
		_fragmentShader = fragmentShader;
		Upload();
	}

    ~Shader()
    {
	    Unload();
    }
}
