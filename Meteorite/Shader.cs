namespace Meteorite;

public class Shader : IDisposable
{
    internal Raylib_cs.Shader Raw;

    internal Shader() { }
    internal Shader(Raylib_cs.Shader shader)
    {
        Raw = shader;
    }
    public Shader(uint id, IntPtr locs)
    {
        unsafe
        {
            Raw.id = id;
            Raw.locs = (int*)locs;
        }
    }
    public Shader(string vsPath, string fsPath)
    {
        Raw = Raylib.LoadShader(vsPath, fsPath);
    }
    public static Shader FromMemory(string vertexShader, string fragmentShader)
    {
        return new()
        {
            Raw = Raylib.LoadShaderFromMemory(vertexShader, fragmentShader)
        };
    }
    public void Unload()
    {
        Raylib.UnloadShader(Raw);
    }
    void IDisposable.Dispose()
    {
        Unload();
    }
}