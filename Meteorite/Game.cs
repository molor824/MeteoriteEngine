using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Meteorite;

public static class Game
{
    public static string ResourcesDir
    {
        get => _resourcesDir;
        set
        {
            if (!Directory.Exists(value)) Log.Error("Path `{0}` does not exist!", value);

            _resourcesDir = value;
        }
    }

    public static float FixedUpdateDelta = 1f / 60;
    public static Camera MainCamera = new();
    public static int Width => _width;
    public static int Height => _height;
    public static vec2 WindowSize => new(_width, _height);
    public static event Action<int, int>? ResizeEvent;

    public static bool WindowShouldClose
    {
        get
        {
            unsafe
            {
                return GLFW.WindowShouldClose(RawWindow);
            }
        }
        set
        {
            unsafe
            {
                GLFW.SetWindowShouldClose(RawWindow, value);
            }
        }
    }
    public static unsafe Window* RawWindow => (Window*)_window;

    private static string _resourcesDir = "Resources";
    private static List<ISingleton> _singletons = new();
    private static bool _shouldClose;
    private static IntPtr _window;
    private static int _width, _height;

    static unsafe GLFWCallbacks.FramebufferSizeCallback _resize = (Window* window, int width, int height) =>
    {
        GL.Viewport(0, 0, width, height);
        _width = width;
        _height = height;
        
        ResizeEvent?.Invoke(width, height);

        Log.Print("Resized with {0}x{1} resolution", width, height);
    };

    /// <summary>
    /// Quickly adds node as child to the main root node.
    /// Shorthand for <c>Node.MainRoot.AddChild(node)</c>.
    /// </summary>
    /// <param name="node">Node to add</param>
    /// <returns>Itself for method chaining</returns>
    public static void AddNode(Node node)
    {
        Node.MainRoot.AddChild(node);
    }

    /// <summary>
    /// Quickly adds multiple nodes as children to the main root node.
    /// Shorthand for <c>Node.MainRoot.AddChildren(nodes)</c>.
    /// </summary>
    /// <param name="nodes">Nodes to add</param>
    /// <returns>Itself for method chaining</returns>
    public static void AddNodes(params Node[] nodes)
    {
        Node.MainRoot.AddChildren(nodes);
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
            if (singleton is T t)
                return t;

        return null;
    }

    public static void New(string title, int width = 800, int height = 600)
    {
        unsafe
        {
            GLFW.Init();
            GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 3);
            GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 3);
            GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);

            var window = GLFW.CreateWindow(width, height, title, null, null);

            if (window == null)
            {
                GLFW.Terminate();
                throw Log.Panic("Failed to create window!");
            }

            GLFW.MakeContextCurrent(window);
            GLFW.SetFramebufferSizeCallback(window, _resize);
            GLFW.SwapInterval(1);

            GL.LoadBindings(new GLFWBindingsContext());
            
            _resize(null, width, height);
            _window = (IntPtr)window;
            
            GL.VertexAttrib4(2, 1f, 1f, 1f, 1f);
        }

        Log.Print("OpenGL Version: {0}\nGLSL Version: {1}\nRenderer: {2}", GL.GetString(StringName.Version),
            GL.GetString(StringName.ShadingLanguageVersion), GL.GetString(StringName.Renderer));
    }

    public static void SetWindowSize(int width, int height)
    {
        unsafe
        {
            GLFW.SetWindowSize(RawWindow, width, height);
        }

        _width = width;
        _height = height;
    }

    static void SwapBuffers()
    {
        unsafe
        {
            GLFW.SwapBuffers(RawWindow);
        }
    }

    static void Start(Node root)
    {
        root.Start();
        for (var i = 0; i < root.ChildrenCount; i++) Start(root.GetChild(i));
    }

    static void Update(Node root, float delta)
    {
        root.Update(delta);
        for (var i = 0; i < root.ChildrenCount; i++) Update(root.GetChild(i), delta);
    }

    static void Render(Node root, float delta)
    {
        root.Render(delta);
        for (var i = 0; i < root.ChildrenCount; i++) Render(root.GetChild(i), delta);
    }

    static void UIRender(Node root, float delta)
    {
        root.UIRender(delta);
        for (var i = 0; i < root.ChildrenCount; i++) UIRender(root.GetChild(i), delta);
    }

    static void Close(Node root)
    {
        root.Close();
        for (var i = 0; i < root.ChildrenCount; i++) Close(root.GetChild(i));
    }

    public static void Run()
    {
        Start(Node.MainRoot);
        foreach (var singleton in _singletons) singleton.Start();

        var updateThread = new Thread(UpdateLoop);

        updateThread.Start();

        var stopwatch = Stopwatch.StartNew();

        while (!WindowShouldClose)
        {
            var elapsed = (float)stopwatch.Elapsed.TotalSeconds;

            stopwatch.Restart();

            GL.Enable(EnableCap.DepthTest);

            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            foreach (var singleton in _singletons) singleton.Render(elapsed);
            MainCamera.Render(0);
            Render(Node.MainRoot, elapsed);
            
            UIRender(Node.MainRoot, elapsed);
            
            GLFW.PollEvents();
            SwapBuffers();
        }

        foreach (var singleton in _singletons) singleton.Close();
        Close(Node.MainRoot);

        _shouldClose = true;
        GLFW.Terminate();

        updateThread.Join();

        Log.Success("Window closed!");
    }

    static void UpdateLoop()
    {
        var stopwatch = Stopwatch.StartNew();
        var lastElapsed = stopwatch.ElapsedTicks;

        while (!_shouldClose)
        {
            var deltaTick = (long)(FixedUpdateDelta * Stopwatch.Frequency);

            if (stopwatch.ElapsedTicks - lastElapsed < deltaTick) continue;

            lastElapsed += deltaTick;

            foreach (var singleton in _singletons) singleton.Update(FixedUpdateDelta);
            Update(Node.MainRoot, FixedUpdateDelta);
        }
    }


    public static object ZZZ__DO_NOT_TOUCH_THIS_PROPERTY_PLZ__
    {
        get => throw new Exception("Told ya not to touch it, stupid");
        set => throw new Exception(
            "Here' your prize for breaking the game engine: https://youtu.be/hiRacdl02w4"
        );
    }
}