namespace Meteorite;

using OpenTK.Graphics.OpenGL;

public class Shader
{
	string _vertexShader = "", _fragmentShader = "";

	public int Program;

	public void CreateProgram()
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
	Shader() { }
	public Shader(string vertexShader, string fragmentShader)
	{
		_vertexShader = vertexShader;
		_fragmentShader = fragmentShader;
	}
}
