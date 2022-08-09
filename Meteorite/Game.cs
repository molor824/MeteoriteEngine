namespace Meteorite;

using System.Diagnostics;
using System.Threading;
using System.Collections;
using System.Collections.ObjectModel;

public class Game : IEnumerable
{
    public int FixedFPS = 60;
    public int ObjectCount => _objects.Count;

    public Camera3D MainCamera = new();

    List<GameObject> _objects = new(0x100);
    List<GameObjectState> _objectStates = new();
    bool _shouldClose;

    public void AddObject(GameObject obj)
    {
        _objectStates.Add(new()
        {
            Value = obj,
            State = State.Added,
        });
    }
    public void AddObjects(params GameObject[] objs)
    {
        foreach (var obj in objs) { AddObject(obj); }
    }
    public void RemoveObject(GameObject obj)
    {
        _objectStates.Add(new()
        {
            Value = obj,
            State = State.Removed
        });
    }
    public void RemoveObjects(params GameObject[] objs)
    {
        foreach (var obj in objs) { RemoveObject(obj); }
    }
    public void RemoveObjects(Type type)
    {
        foreach (var obj in _objects)
        {
            if (obj.GetType() == type) { RemoveObject(obj); }
        }
    }
    public void RemoveObjects<T>() where T : GameObject
    {
        RemoveObjects(typeof(T));
    }
    public bool ContainsObject(GameObject obj)
    {
        foreach (var obj1 in _objects)
        {
            if (obj1 == obj) { return true; }
        }
        return false;
    }
    public bool ContainsType(Type type)
    {
        foreach (var obj in _objects)
        {
            if (obj.GetType() == type) { return true; }
        }
        return false;
    }
    public bool ContainsType<T>() where T : GameObject
    {
        return ContainsType(typeof(T));
    }
    public GameObject[] GetObjects(Type type)
    {
        var list = new List<GameObject>(_objects.Count);

        foreach (var obj in _objects)
        {
            if (obj.GetType() == type) { list.Add(obj); }
        }

        return list.ToArray();
    }
    public ReadOnlyCollection<T>? GetObjects<T>() where T : GameObject
    {
        return GetObjects(typeof(T)) as ReadOnlyCollection<T>;
    }

    void AddSetup(GameObject obj)
    {
        obj.Game = this;
        obj.Added();
    }
    void RemoveSetup(GameObject obj)
    {
        obj.Removed();
        obj.Game = null!;
    }
    void SyncObjects()
    {
        for (var i = 0; i < _objectStates.Count; i++)
        {
            var objState = _objectStates[i];
            var obj = objState.Value;

            if (objState.State == State.Added)
            {
                _objects.Add(obj);
                AddSetup(obj);
            }
            else
            {
                RemoveSetup(obj);
                _objects.Remove(obj);
            }
        }

        _objectStates.Clear();
    }
    void Start()
    {
        SyncObjects();
        foreach (var obj in this) { if (obj.Enabled) { obj.Start(); } }
    }
    void Update(float delta)
    {
        SyncObjects();
        foreach (var obj in this) { if (obj.Enabled) { obj.Update(delta); } }
    }
    void Render(float delta)
    {
        SyncObjects();
        foreach (var obj in this) { if (obj.Enabled) { obj.Render(delta); } }
    }
    void Close()
    {
        SyncObjects();
        foreach (var obj in this)
        {
            if (obj.Enabled)
            {
                obj.Close();
                obj.Removed();
            }
        }
    }
    public void Run(string name, int width, int height)
    {
        Raylib.SetConfigFlags(ConfigFlags.FLAG_VSYNC_HINT);
        Raylib.InitWindow(width, height, name);
        Raylib.SetTargetFPS(FixedFPS);

        MainCamera.projection = CameraProjection.CAMERA_ORTHOGRAPHIC;
        MainCamera.fovy = 10;
        MainCamera.position = new(0, 0, 10);
        MainCamera.target = new(0, 0, -1);
        MainCamera.up = new(0, 1, 0);

        Start();

        var updateThread = new Thread(FixedUpdate);

        updateThread.Start();

        while (!Raylib.WindowShouldClose())
        {
            var delta = Raylib.GetFrameTime();

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib_cs.Color.BLACK);
            Raylib.BeginMode3D(MainCamera);

            Render(delta);

            Raylib.EndMode3D();
            Raylib.EndDrawing();
        }

        _shouldClose = true;
        updateThread.Join();

        Close();

        Raylib.CloseWindow();
    }
    void FixedUpdate()
    {
        var stopwatch = new Stopwatch();

        stopwatch.Start();

        while (!_shouldClose)
        {
            var delta = 1.0 / FixedFPS;

            Update((float)delta);

            while (stopwatch.Elapsed.TotalSeconds < delta) { }

            stopwatch.Restart();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public IEnumerator<GameObject> GetEnumerator()
    {
        foreach (var obj in _objects)
        {
            yield return obj;
        }
    }
}
enum State
{
    Added,
    Removed
}
struct GameObjectState
{
    public GameObject Value;
    public State State;
}
