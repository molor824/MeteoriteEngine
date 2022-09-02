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

    internal void UseProgram()
    {
        GL.UseProgram(Program);
    }
    internal void CreateProgram()
	{
		if (Program != 0)
		{
			Log.Warn("Program is already defined");
			return;
		}

		var vs = GL.CreateShader(ShaderType.VertexShader);
		var fs = GL.CreateShader(ShaderType.FragmentShader);

		GL.ShaderSource(vs, _vertexShader);
		GL.ShaderSource(fs, _fragmentShader);

		GL.CompileShader(vs);
		GL.CompileShader(fs);

		CheckShaderError(vs);
		CheckShaderError(fs);

		Program = GL.CreateProgram();

		GL.AttachShader(Program, vs);
		GL.AttachShader(Program, fs);

		GL.LinkProgram(Program);
		CheckProgramError(Program);
	}
	static void CheckShaderError(int shader)
	{
		GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);

		if (success == 0)
		{
			Log.Error("Shader error:\n{0}", GL.GetShaderInfoLog(shader));
		}
	}
	static void CheckProgramError(int program)
	{
		GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);

		if (success == 0)
		{
			Log.Error("Program error:\n{0}", GL.GetProgramInfoLog(program));
		}
	}
	public static Shader FromPath(string vsPath, string fsPath)
	{
		return new(File.ReadAllText(vsPath), File.ReadAllText(fsPath));
	}
    public Shader() { }
	public Shader(string vertexShader, string fragmentShader)
	{
		_vertexShader = vertexShader;
		_fragmentShader = fragmentShader;
	}
}
