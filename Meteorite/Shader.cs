using System.Runtime.CompilerServices;

namespace Meteorite;

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

public class Shader
{
    const string DefaultVertexShader =
@"#version 330

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec4 aColor;

out vec4 fColor;
out vec2 fTexCoord;

uniform mat4 transform;

void main()
{
    vec4 position = transform * vec4(aPos, 1);
	gl_Position = position;
    fTexCoord = aTexCoord;
    fColor = aColor;
}
";
    const string DefaultFragmentShader =
@"#version 330

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
    public static Shader Default = new(DefaultVertexShader, DefaultFragmentShader);

    internal int Program;

    public void SetMat4(string location, bool transpose, mat4 value)
    {
	    unsafe
	    {
		    GL.UniformMatrix4(GetUniformLocation(location), 1, transpose, (float*)&value);
	    }
    }
    internal int GetUniformLocation(string location)
    {
	    return GL.GetUniformLocation(Program, location);
    }
    void Upload(string vertexShader, string fragmentShader)
    {
	    if (Program != 0) return;

		var vs = GL.CreateShader(ShaderType.VertexShader);
		var fs = GL.CreateShader(ShaderType.FragmentShader);

        GL.ShaderSource(vs, vertexShader);
        GL.ShaderSource(fs, fragmentShader);

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

    public Shader(string vertexShader, string fragmentShader)
	{
        Upload(vertexShader, fragmentShader);
	}

    ~Shader()
    {
	    Unload();
    }
}
