namespace Meteorite;

using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Threading;

public class Game
{
    public static Game Main => _main;
    public float FixedUpdateDelta = 1f / 60;
    public Camera? MainCamera;
    public int Width => _width;
    public int Height => _height;

    bool WindowShouldClose
    {
        get { unsafe { return GLFW.WindowShouldClose((Window*)_window); } }
        set { unsafe { GLFW.SetWindowShouldClose((Window*)_window, value); } }
    }

    private static List<ISingleton> _singletons = new();
    bool _shouldClose;
    IntPtr _window;
    int _width, _height;
    static Game _main = null!;
    internal unsafe Window* RawWindow => (Window*)_window;

    unsafe void Resize(Window* window, int width, int height)
    {
        GL.Viewport(0, 0, width, height);
        _width = width;
        _height = height;

        Log.Print("Resized with {0}x{1} resolution", width, height);
    }

    /// <summary>
    /// Quickly adds node as child to the main root node.
    /// Shorthand for <c>Node.MainRoot.AddChild(node)</c>.
    /// </summary>
    /// <param name="node">Node to add</param>
    /// <returns>Itself for method chaining</returns>
    public Game AddNode(Node node)
    {
        Node.MainRoot.AddChild(node);
        return this;
    }
    /// <summary>
    /// Quickly adds multiple nodes as children to the main root node.
    /// Shorthand for <c>Node.MainRoot.AddChildren(nodes)</c>.
    /// </summary>
    /// <param name="nodes">Nodes to add</param>
    /// <returns>Itself for method chaining</returns>
    public Game AddNodes(params Node[] nodes)
    {
        Node.MainRoot.AddChildren(nodes);
        return this;
    }
    public static void AddSingleton(ISingleton singleton) => _singletons.Add(singleton);
    public static void AddSingleton<T>() where T : ISingleton, new() => _singletons.Add(new T());
    public static bool RemoveSingleton(ISingleton singleton) => _singletons.Remove(singleton);
    public static bool RemoveSingleton(int index)
    {
        if (index < 0 || index >= _singletons.Count) return false;
        _singletons.RemoveAt(index);
        return true;
    }

    public static T? GetSingleton<T>() where T : class, ISingleton
    {
        foreach (var singleton in _singletons)
            if (singleton is T t) return t;

        return null;
    }
    public Game(string title, int width = 800, int height = 600)
    {
        if (_main != null) throw Log.Panic("Cannot create more than 1 instance of Game!");

        GLFW.Init();
        GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 3);
        GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 3);
        GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);

        unsafe
        {
            var window = GLFW.CreateWindow(width, height, title, null, null);

            if (window == null)
            {
                GLFW.Terminate();
                throw Log.Panic("Failed to create window!");
            }


            GLFW.MakeContextCurrent(window);
            GLFW.SetFramebufferSizeCallback(window, Resize);
            GLFW.SwapInterval(1);

            GL.LoadBindings(new GLFWBindingsContext());
            GL.Enable(EnableCap.DepthTest);
            GL.VertexAttrib3(2, 1f, 1f, 1f);
            Resize(null, width, height);

            _window = (IntPtr)window;
        }

        Log.Print("Opengl Version: " + GL.GetString(StringName.Version));
        Log.Print("GLSL Version: " + GL.GetString(StringName.ShadingLanguageVersion));
        Log.Print("Renderer: " + GL.GetString(StringName.Renderer));

        _main = this;
    }
    public void SetWindowSize(int width, int height)
    {
        unsafe { GLFW.SetWindowSize(RawWindow, width, height); }
        _width = width;
        _height = height;
    }
    void SwapBuffers()
    {
        unsafe { GLFW.SwapBuffers(RawWindow); }
    }
    void Start(Node root)
    {
        root.Start();
        for (var i = 0; i < root.ChildrenCount; i++) Start(root.GetChild(i));
    }
    void Update(Node root, float delta)
    {
        root.Update(delta);
        for (var i = 0; i < root.ChildrenCount; i++) Update(root.GetChild(i), delta);
    }
    void Render(Node root, float delta)
    {
        root.Render(delta);
        for (var i = 0; i < root.ChildrenCount; i++) Render(root.GetChild(i), delta);
    }

    void Close(Node root)
    {
        root.Close();
        for (var i = 0; i < root.ChildrenCount; i++) Close(root.GetChild(i));
    }
    public void Run()
    {
        Start(Node.MainRoot);
        foreach (var singleton in _singletons) singleton.Start();

        var updateThread = new Thread(UpdateLoop);
        var stopwatch = Stopwatch.StartNew();
        updateThread.Start();

        while (!WindowShouldClose)
        {
            var elapsed = (float)stopwatch.Elapsed.TotalSeconds;

            stopwatch.Restart();

            Render(Node.MainRoot, elapsed);
            foreach (var singleton in _singletons) singleton.Render(elapsed);

            GLFW.PollEvents();
            SwapBuffers();
        }

        Close(Node.MainRoot);
        foreach (var singleton in _singletons) singleton.Close();
        
        _shouldClose = true;
        GLFW.Terminate();
        
        updateThread.Join();
        
        Log.Print("Window closed succesfully!");
    }
    void UpdateLoop()
    {
        var stopwatch = Stopwatch.StartNew();
        var lastElapsed = stopwatch.ElapsedTicks;
        
        while (!_shouldClose)
        {
            var deltaTick = (long)(FixedUpdateDelta * Stopwatch.Frequency);
            
            if (stopwatch.ElapsedTicks - lastElapsed < deltaTick) continue;

            lastElapsed += deltaTick;

            Update(Node.MainRoot, FixedUpdateDelta);
            foreach (var singleton in _singletons) singleton.Update(FixedUpdateDelta);
        }
    }




























    public object ZZZ__DO_NOT_TOUCH_THIS_PROPERTY_PLZ__
    {
        get => throw new Exception("Told ya not to touch it, stupid");
        set => throw new Exception(
            "Here' your prize for breaking the game engine: https://youtu.be/hiRacdl02w4"
        );
    }
}
enum State
{
    Added,
    Removed
}
struct GameObjectState
{
    public Node Value;
    public State State;
}
