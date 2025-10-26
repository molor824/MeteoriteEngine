namespace Meteorite;

public class World : Singleton
{
    private List<Instance> _instances = [];
}

public class Instance : IDisposable
{
    public World? World => _world;
    
    private bool _updated, _fixedUpdated;
    private World? _world;
    private bool _disposed;

    public void Start(World world)
    {
        if (_world != null) return;
        _world = world;
        OnStart();
    }

    public void Update(double dt)
    {
        if (_updated) return;
        _updated = true;
        OnUpdate(dt);
    }

    public void FixedUpdate(double dt)
    {
        if (_fixedUpdated) return;
        _fixedUpdated = true;
        OnFixedUpdate(dt);
    }

    public void ClearFlags()
    {
        _updated = _fixedUpdated = false;
    }

    protected virtual void OnStart() { }
    protected virtual void OnUpdate(double dt) { }
    protected virtual void OnFixedUpdate(double dt) { }

    protected virtual void OnDispose() { }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        OnDispose();
        GC.SuppressFinalize(this);
    }

    ~Instance()
    {
        Dispose();
    }
}