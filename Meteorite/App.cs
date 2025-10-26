using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace Meteorite;

public class App
{
    private IWindow _window;
    private double _fixedTimeLeft = 0.0;
    private GL _gl;
    private Dictionary<Type, Singleton> _singletons = [];

    public GL Gl => _gl;
    public double FixedDelta = 1.0 / 120.0;
    public Vector2D<int> WindowSize
    {
        get => _window.Size;
        set => _window.Size = value;
    }
    public IWindow WindowHandle => _window;

    public App()
    {
        _window = Window.Create(WindowOptions.Default with
        {
            Size = new(1280, 720),
            VSync = true
        });
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
    }

    private void OnLoad()
    {
        _gl = _window.CreateOpenGL();
    }
    
    private void OnFixedUpdate(double dt) { }

    private void OnUpdate(double dt)
    {
        _fixedTimeLeft += dt;
        while (_fixedTimeLeft >= FixedDelta)
        {
            _fixedTimeLeft -= FixedDelta;
            OnFixedUpdate(FixedDelta);
        }
    }

    private void OnRender(double dt)
    {
        _gl.ClearColor(0.1f, 0.2f, 0.3f, 1.0f);
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public void Run()
    {
        _window.Run();
    }
}