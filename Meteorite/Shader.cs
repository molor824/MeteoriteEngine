using Meteorite.Mathematics;

namespace Meteorite;

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

public class Shader
{
    internal int Program;

    internal void SetVec4(int location, Vector4 value)
    {
	    GL.Uniform4(location, value.X, value.Y, value.Z, value.W);
    }
    internal void SetVec4(int location, Color value)
    {
	    GL.Uniform4(location, value.R, value.G, value.B, value.A);
    }
    internal void SetMat4(int location, bool transpose, Mat4x4 value)
    {
	    unsafe
	    {
		    GL.UniformMatrix4(location, 1, transpose, (float*)&value);
	    }
    }

    internal void Use()
    {
	    GL.UseProgram(Program);
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
	    var version = $"#version {GL.GetInteger(GetPName.MajorVersion)}{GL.GetInteger(GetPName.MinorVersion)}0\n";
        Upload(version + vertexShader, version + fragmentShader);
	}

    ~Shader()
    {
	    Unload();
    }
}
